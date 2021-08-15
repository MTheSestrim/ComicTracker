namespace ComicTracker.Services.Data.Genre.Contracts
{
    using System.Threading.Tasks;

    public interface IGenreCreationService
    {
        Task<bool> CreateGenre(string name);
    }
}
