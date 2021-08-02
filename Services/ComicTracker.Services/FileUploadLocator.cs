namespace ComicTracker.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using static ComicTracker.Common.GlobalConstants;

    public static class FileUploadLocator
    {
        public static async Task<string> GetUploadedFileNameAsync(IFormFile coverImage)
        {
            string uniqueFileName = null;

            uniqueFileName = Guid.NewGuid().ToString() + "_" + coverImage.FileName;
            string filePath = Path.Combine($"wwwroot{SeriesImagePath}", uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await coverImage.CopyToAsync(fileStream);
            }

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
