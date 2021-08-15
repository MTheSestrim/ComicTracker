namespace ComicTracker.Services.Data.Genre.Contracts
{
    using System.Threading.Tasks;

    public interface IGenreDeletionService
    {
        Task<bool> DeleteGenre(int id);
    }
}
