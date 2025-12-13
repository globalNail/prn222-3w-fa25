using SCMS.Service.TienPVK;
using SCMS.WorkerService.TienPVK.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SCMS.WorkerService.TienPVK
{

    //rubric: install, delete service, showing in services.msc, event viewer, file log
    /*
        sc create "Clubs_WorkerService" binPath= "C:\Users\khanhtien\source\prn222_3w\prn222-3w-fa25\FA25_PRN222_3W1_ASM2_SE170488_TienPVK\SCMS.WorkerService.TienPVK\bin\Debug\net8.0\SCMS.WorkerService.TienPVK.exe"

        sc start "Clubs_WorkerService"

        sc stop "Clubs_WorkerService"

        sc delete "Clubs_WorkerService"
    */

    /*
     * Worker service to log club data periodically.
     * This service retrieves all club entities, writes them to:
     * - JSON log file
     * - Excel file
     * And sends an email report with the Excel file attached.
     * It runs as a background service within the application.
 */
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ClubsTienPvkService _service;
        private readonly ExcelExportService _excelService;
        private readonly EmailService _emailService;

        public Worker(
            ILogger<Worker> logger,
            ClubsTienPvkService service,
            ExcelExportService excelService,
            EmailService emailService
            )
        {
            _logger = logger;
            _service = service;
            _excelService = excelService;
            _emailService = emailService;
        }

        // doc data entity chinh cua minh, log file, export excel, send email

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    if (_logger.IsEnabled(LogLevel.Information))
            //    {
            //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    }
            //    await Task.Delay(1000, stoppingToken);
            //}

            // Validate email configuration on startup
            _emailService.ValidateConfiguration();

            while (!stoppingToken.IsCancellationRequested)
            {
                await this.ProcessDataAsync(stoppingToken);

                // Run daily at midnight (24 hours)
                await Task.Delay(1000 * 60 * 60 * 24, stoppingToken);
            }
        }

        private async Task ProcessDataAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Starting data processing at: {time}", DateTime.Now);

                var items = await _service.GetAllAsync();

                if (items == null || items.Count == 0)
                {
                    _logger.LogWarning("No data found to process");
                    await WriteEmptyLogAsync();
                    return;
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                // 1. Write JSON log file
                await WriteLogFileAsync(items, timestamp, stoppingToken);

                // 2. Export to Excel
                string excelFilePath = await ExportToExcelAsync(items, timestamp);

                // 3. Send email with Excel attachment
                if (!string.IsNullOrEmpty(excelFilePath) && File.Exists(excelFilePath))
                {
                    await SendReportEmailAsync(excelFilePath, items.Count);
                }

                _logger.LogInformation("Data processing completed successfully at: {time}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data processing");
            }
        }

        private async Task WriteEmptyLogAsync()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"worker_log_{timestamp}.txt";
            var logFilePath = PathHelper.NewFolderPath(
                folderName: "Club Worker",
                subFolderName: "logs",
                fileName: fileName);

            using (var writer = File.Open(logFilePath, FileMode.Append, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(writer))
                {
                    await streamWriter.WriteLineAsync($"{DateTime.Now}: Data is Empty");
                    await writer.FlushAsync();
                }
            }
        }

        private async Task WriteLogFileAsync(List<SCMS.Domain.TienPVK.Models.ClubsTienPvk> items, string timestamp, CancellationToken stoppingToken)
        {
            var jsonOption = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var content = JsonSerializer.Serialize(items, jsonOption);
            string fileName = $"worker_log_{timestamp}.txt";
            var logFilePath = PathHelper.NewFolderPath(
                folderName: "Club Worker",
                subFolderName: "logs",
                fileName: fileName
                );

            _logger.LogInformation("Writing JSON log file: {logFilePath}", logFilePath);

            using (var writer = File.Open(logFilePath, FileMode.Append, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(writer))
                {
                    await streamWriter.WriteLineAsync($"{DateTime.Now}: {content}");
                    await writer.FlushAsync();
                }
            }

            _logger.LogInformation("JSON log file written successfully");
        }

        private async Task<string> ExportToExcelAsync(List<SCMS.Domain.TienPVK.Models.ClubsTienPvk> items, string timestamp)
        {
            try
            {
                string fileName = $"clubs_report_{timestamp}.xlsx";
                var excelFilePath = PathHelper.NewFolderPath(
                    folderName: "Club Worker",
                    subFolderName: "excel_reports",
                    fileName: fileName);

                _logger.LogInformation("Exporting data to Excel: {excelFilePath}", excelFilePath);

                bool success = await _excelService.ExportClubsToExcelAsync(items, excelFilePath);

                if (success)
                {
                    _logger.LogInformation("Excel export completed successfully");
                    return excelFilePath;
                }
                else
                {
                    _logger.LogError("Excel export failed");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to Excel");
                return string.Empty;
            }
        }

        private async Task SendReportEmailAsync(string excelFilePath, int recordCount)
        {
            try
            {
                _logger.LogInformation("Sending report email with attachment: {excelFilePath}", excelFilePath);

                bool success = await _emailService.SendReportEmailAsync(
                    excelFilePath,
                    "Clubs Data Report",
                    recordCount);

                if (success)
                {
                    _logger.LogInformation("Report email sent successfully");
                }
                else
                {
                    _logger.LogError("Failed to send report email");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending report email");
            }
        }
    }
}
