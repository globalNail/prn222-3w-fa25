using OfficeOpenXml;
using SCMS.Domain.TienPVK.Models;

namespace SCMS.WorkerService.TienPVK.Services
{
    /// <summary>
    /// Service for exporting data to Excel files using EPPlus
    /// </summary>
    public class ExcelExportService
    {
        private readonly ILogger<ExcelExportService> _logger;

        public ExcelExportService(ILogger<ExcelExportService> logger)
        {
            _logger = logger;
            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// Exports club data to an Excel file
        /// </summary>
        /// <param name="clubs">List of clubs to export</param>
        /// <param name="filePath">Path where the Excel file will be saved</param>
        /// <returns>True if export successful, false otherwise</returns>
        public async Task<bool> ExportClubsToExcelAsync(List<ClubsTienPvk> clubs, string filePath)
        {
            try
            {
                // Ensure directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Clubs Data");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Club ID";
                    worksheet.Cells[1, 2].Value = "Club Code";
                    worksheet.Cells[1, 3].Value = "Club Name";
                    worksheet.Cells[1, 4].Value = "Category Name";
                    worksheet.Cells[1, 5].Value = "Established Date";
                    worksheet.Cells[1, 6].Value = "Description";
                    worksheet.Cells[1, 7].Value = "Status";
                    worksheet.Cells[1, 8].Value = "Created Date";
                    worksheet.Cells[1, 9].Value = "Last Modified Date";

                    // Style headers
                    using (var range = worksheet.Cells[1, 1, 1, 9])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    // Add data
                    for (int i = 0; i < clubs.Count; i++)
                    {
                        var club = clubs[i];
                        int row = i + 2;

                        worksheet.Cells[row, 1].Value = club.ClubIdtienPvk;
                        worksheet.Cells[row, 2].Value = club.ClubCode;
                        worksheet.Cells[row, 3].Value = club.ClubName;
                        worksheet.Cells[row, 4].Value = club.Category.CategoryName;
                        worksheet.Cells[row, 5].Value = club.FoundedDate.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 6].Value = club.Description;
                        worksheet.Cells[row, 7].Value = club.Status;
                        worksheet.Cells[row, 8].Value = club.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[row, 9].Value = club.ModifiedAt?.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Add summary row
                    int summaryRow = clubs.Count + 3;
                    worksheet.Cells[summaryRow, 1].Value = "Total Clubs:";
                    worksheet.Cells[summaryRow, 2].Value = clubs.Count;
                    worksheet.Cells[summaryRow, 1, summaryRow, 2].Style.Font.Bold = true;

                    // Save the file
                    var fileInfo = new FileInfo(filePath);
                    await package.SaveAsAsync(fileInfo);

                    _logger.LogInformation("Successfully exported {Count} clubs to Excel file: {FilePath}", 
                        clubs.Count, filePath);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting clubs to Excel file: {FilePath}", filePath);
                return false;
            }
        }

        /// <summary>
        /// Exports generic data to Excel with custom column mapping
        /// </summary>
        public async Task<bool> ExportDataToExcelAsync<T>(
            List<T> data, 
            string filePath, 
            string sheetName,
            Dictionary<string, Func<T, object?>> columnMapping)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(sheetName);

                    // Add headers
                    int col = 1;
                    foreach (var header in columnMapping.Keys)
                    {
                        worksheet.Cells[1, col].Value = header;
                        col++;
                    }

                    // Style headers
                    using (var range = worksheet.Cells[1, 1, 1, columnMapping.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    }

                    // Add data
                    for (int i = 0; i < data.Count; i++)
                    {
                        int row = i + 2;
                        int column = 1;
                        
                        foreach (var mapping in columnMapping.Values)
                        {
                            var value = mapping(data[i]);
                            worksheet.Cells[row, column].Value = value;
                            column++;
                        }
                    }

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    var fileInfo = new FileInfo(filePath);
                    await package.SaveAsAsync(fileInfo);

                    _logger.LogInformation("Successfully exported {Count} records to Excel file: {FilePath}", 
                        data.Count, filePath);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting data to Excel file: {FilePath}", filePath);
                return false;
            }
        }
    }
}
