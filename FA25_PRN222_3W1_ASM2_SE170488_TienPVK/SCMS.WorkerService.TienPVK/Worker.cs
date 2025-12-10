using SCMS.Service.TienPVK.Implements;
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
     * This service retrieves all club entities and writes them to a log file in JSON format.
     * It runs as a background service within the application.
 */
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ClubsTienPvkService _service;

        public Worker(
            ILogger<Worker> logger,
            ClubsTienPvkService service
            )
        {
            _logger = logger;
            _service = service;
        }

        // doc data entity chinh cua minh, log file

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
            await this.WriteLogFileAsync(stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }

        private async Task WriteLogFileAsync(CancellationToken stoppingToken)
        {
            var items = await _service.GetAllAsync();
            
            var jsonOption = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var content = JsonSerializer.Serialize(items, jsonOption);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"worker_log_{timestamp}.txt";
            //var logFilePath = Path.Combine(AppContext.BaseDirectory, "DataLog", "club_worker_log.txt");
            var logFilePath = PathHelper.NewFolderPath(
                folderName: "Club Worker",
                subFolderName: "logs",
                fileName: fileName
                );

            _logger.LogInformation("Log file path: {logFilePath}", logFilePath);
            //using (var writer = new StreamWriter(logFilePath, append: true))
            //{
            //    await writer.WriteLineAsync($"{DateTimeOffset.Now}: {message}");
            //}
            if (items == null || items.Count == 0)
            {
                using (var writer = File.Open(logFilePath, FileMode.Append, FileAccess.Write))
                {
                    using (var streamWriter = new StreamWriter(writer))
                    {
                        await streamWriter.WriteLineAsync($"{DateTime.Now}: Data is Empty");
                        await writer.FlushAsync();
                    }
                }
                return;
            }

            using (var writer = File.Open(logFilePath, FileMode.Append, FileAccess.Write))
            {
                using ( var streamWriter = new StreamWriter(writer))
                {
                    await streamWriter.WriteLineAsync($"{DateTime.Now}: {content}");
                    await writer.FlushAsync();
                }
            }
        }
    }
}
