namespace SCMS.WorkerService.TienPVK;

public static class PathHelper
{
    /// <summary>
    ///     
    /// Creates a new folder path in the ProgramData directory with optional subfolder and returns the full file path.
    /// 
    /// </summary>
    public static string NewFolderPath(
        string folderName,
        string? subFolderName,
        string fileName)
    {
        // Step 1: Base = ProgramData folder
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        // Step 2: Build path normally
        var finalDir = Path.Combine(baseDir, folderName);

        // Step 3: Add subfolder ONLY when not null or empty
        if (!string.IsNullOrWhiteSpace(subFolderName))
        {
            finalDir = Path.Combine(finalDir, subFolderName);
        }

        // Step 4: Ensure folder exists
        Directory.CreateDirectory(finalDir);

        // Step 5: Return full file path
        return Path.Combine(finalDir, fileName);
    }
}
