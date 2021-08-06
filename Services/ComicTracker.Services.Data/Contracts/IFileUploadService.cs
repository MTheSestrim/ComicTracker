namespace ComicTracker.Services.Data.Contracts
{
    public interface IFileUploadService
    {
        public string GetUploadedFileName(byte[] coverImage, string entityName);

        public void DeleteCover(string coverPath);
    }
}
