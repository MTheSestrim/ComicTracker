namespace ComicTracker.Services.Data.Genre
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Genre.Contracts;

    public class GenreCreationService : IGenreCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public GenreCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateGenre(string name)
        {
            if (this.dbContext.Genres.Any(g => g.Name == name))
            {
                return false;
            }

            var genre = new Genre { Name = name };

            this.dbContext.Genres.Add(genre);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
