namespace ComicTracker.Services.Data.Genre
{
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Genre.Contracts;

    public class GenreDeletionService : IGenreDeletionService
    {
        private readonly ComicTrackerDbContext dbContext;

        public GenreDeletionService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> DeleteGenre(int id)
        {
            var genre = this.dbContext.Genres.Find(id);

            if (genre == null)
            {
                return false;
            }

            this.dbContext.Remove(genre);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
