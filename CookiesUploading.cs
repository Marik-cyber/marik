namespace CookiesUploading
{
    public class UploadingService
    {
        private readonly string _uploadFolderPath;

        public UploadingService()
        {
            // יצירת תיקייה מרכזית ל-Uploads
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            Directory.CreateDirectory(_uploadFolderPath); // יוצרת את התיקייה אם אינה קיימת
        }

        public async Task<(bool IsSuccess, string FileName, string ErrorMessage)> SaveFileAsync(string fileName, string content)
        {
            try
            {
                // יצירת שם קובץ ייחודי
                var fullPath = Path.Combine(_uploadFolderPath, fileName);

                // כתיבת תוכן הקובץ
                await File.WriteAllTextAsync(fullPath, content);

                return (true,fileName, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error saving file: {ex.Message}");
            }
        }
    }
}