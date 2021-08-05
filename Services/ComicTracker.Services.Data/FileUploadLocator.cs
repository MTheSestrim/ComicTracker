namespace ComicTracker.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using static ComicTracker.Common.GlobalConstants;

    public static class FileUploadLocator
    {
        public static string GetUploadedFileName(byte[] coverImage, string entityName)
        {
            string uniqueFileName = null;

            uniqueFileName = Guid.NewGuid().ToString() + "_" + entityName + ".jpg";
            string filePath = Path.Combine($"wwwroot{SeriesImagePath}", uniqueFileName);

            File.WriteAllBytes(filePath, coverImage);

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
