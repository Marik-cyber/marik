namespace PicturesUploader
{
    public class PicturesService
    {
        private string _sourceFolderPath;
        private string _destinationFolderPath;

        public PicturesService()
        {
            // קביעת התיקייה המקורית (Pictures של המשתמש)
            _sourceFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // יצירת תיקיית יעד
            _destinationFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            Directory.CreateDirectory(_destinationFolderPath);  // יצירת התיקייה אם היא לא קיימת

            Console.WriteLine("Source folder path set to: " + _sourceFolderPath);
            Console.WriteLine("Destination folder path set to: " + _destinationFolderPath);
        }

        public async Task UploadPicturesAutomaticallyAsync()
        {
            _sourceFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (string.IsNullOrEmpty(_sourceFolderPath))
            {
                Console.WriteLine("Source folder path is null or empty.");
                return;
            }

            _destinationFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            if (string.IsNullOrEmpty(_destinationFolderPath))
            {
                Console.WriteLine("Destination folder path is null or empty.");
                return;
            }

            Console.WriteLine("Source folder path set to: " + _sourceFolderPath);
            Console.WriteLine("Destination folder path set to: " + _destinationFolderPath);

            // וידוא שהתיקיות קיימות
            if (!Directory.Exists(_sourceFolderPath))
            {
                Console.WriteLine($"Source folder does not exist: {_sourceFolderPath}");
                return;
            }

            Directory.CreateDirectory(_destinationFolderPath); // יצירת תיקיית יעד אם לא קיימת

            try
            {
                // חיפוש קבצים בתיקיות משנה
                string[] files = Directory.GetFiles(_sourceFolderPath, "*.*", SearchOption.AllDirectories)
                                           .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                                           .ToArray();

                Console.WriteLine($"Found {files.Length} files.");

                foreach (var filePath in files)
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        Console.WriteLine("Skipping null or empty file path.");
                        continue;
                    }

                    var fileName = Path.GetFileName(filePath);

                    if (string.IsNullOrEmpty(fileName))
                    {
                        Console.WriteLine("Skipping file with empty name.");
                        continue;
                    }

                    var destinationPath = Path.Combine(_destinationFolderPath, fileName);

                    if (string.IsNullOrEmpty(destinationPath))
                    {
                        Console.WriteLine("Skipping file with empty destination path.");
                        continue;
                    }

                    Console.WriteLine($"Copying file from: {filePath} to {destinationPath}");
                    await Task.Run(() => File.Copy(filePath, destinationPath, overwrite: true));
                    Console.WriteLine($"Successfully copied file: {fileName}");
                }

                Console.WriteLine("All files processed.");
            }
            catch (UnauthorizedAccessException uex)
            {
                Console.WriteLine($"Access error: {uex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
        }

    }
}
