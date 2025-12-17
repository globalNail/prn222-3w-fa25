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
        private DateTime _lastEmailSentDate;

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
            _lastEmailSentDate = DateTime.MinValue;
        }

        // doc data entity chinh cua minh, log file, export excel, send email

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _emailService.ValidateConfiguration();
            _logger.LogInformation("Worker service started. Logging every 3 seconds, email reports sent daily.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessDataAsync(stoppingToken);
                await Task.Delay(3000, stoppingToken);
            }
        }

        private async Task ProcessDataAsync(CancellationToken stoppingToken)
        {
            try
            {
                var items = await _service.GetAllAsync();
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                if (items == null || items.Count == 0)
                {
                    await WriteLogAsync("No data available", timestamp);
                    return;
                }

                await WriteLogAsync(items, timestamp);

                if (_lastEmailSentDate.Date < DateTime.Now.Date)
                {
                    string excelFilePath = await ExportToExcelAsync(items, timestamp);
                    if (!string.IsNullOrEmpty(excelFilePath))
                    {
                        await SendReportEmailAsync(excelFilePath, items.Count);
                        _lastEmailSentDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data");
            }
        }

        private async Task WriteLogAsync(object data, string timestamp)
        {
            try
            {
                string content = data is string str ? str : JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                });

                string fileName = $"worker_log_{timestamp}.txt";
                string logFilePath = PathHelper.NewFolderPath("Vu419", "logs", fileName);

                EnsureDirectoryExists(logFilePath);

                await File.WriteAllTextAsync(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n{content}\n");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write log file");
            }
        }

        private void EnsureDirectoryExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private async Task<string> ExportToExcelAsync(List<SCMS.Domain.TienPVK.Models.ClubsTienPvk> items, string timestamp)
        {
            try
            {
                string fileName = $"clubs_report_{timestamp}.xlsx";
                string excelFilePath = PathHelper.NewFolderPath("Vu419", "excel_reports", fileName);

                EnsureDirectoryExists(excelFilePath);

                bool success = await _excelService.ExportClubsToExcelAsync(items, excelFilePath);
                return success ? excelFilePath : string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel export failed");
                return string.Empty;
            }
        }

        private async Task SendReportEmailAsync(string excelFilePath, int recordCount)
        {
            try
            {
                bool success = await _emailService.SendReportEmailAsync(excelFilePath, "Clubs Data Report", recordCount);
                if (success)
                {
                    _logger.LogInformation("Daily email report sent with {count} records", recordCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email send failed");
            }
        }
    }
}
