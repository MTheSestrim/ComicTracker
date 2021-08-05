namespace ComicTracker.Services.Data
{
    using System;
    using System.IO;

    using static ComicTracker.Common.GlobalConstants;

    public static class FileUploadLocator
    {
        public static string GetUploadedFileNameAsync(byte[] imageData, string fileName)
        {

            string uniqueFileName = null;

            uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
            string filePath = Path.Combine($"wwwroot{SeriesImagePath}", uniqueFileName);

            File.WriteAllBytes(filePath, imageData);

            return SeriesImagePath + uniqueFileName;
        }

        public static void DeleteCover(string coverPath)
        {
            var filePath = $"wwwroot{coverPath}";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
