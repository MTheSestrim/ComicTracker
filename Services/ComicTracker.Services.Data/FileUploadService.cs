namespace ComicTracker.Services.Data
{
    using System;
    using System.IO;

    using ComicTracker.Services.Data.Contracts;

    using static ComicTracker.Common.GlobalConstants;

    public class FileUploadService : IFileUploadService
    {
        public string GetUploadedFileName(byte[] coverImage, string entityName)
        {
            string uniqueFileName = null;

            uniqueFileName = Guid.NewGuid().ToString() + "_" + entityName + ".jpg";
            string filePath = Path.Combine($"wwwroot{SeriesImagePath}", uniqueFileName);

            File.WriteAllBytes(filePath, coverImage);

            return SeriesImagePath + uniqueFileName;
        }

        public void DeleteCover(string coverPath)
        {
            var filePath = $"wwwroot{coverPath}";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
