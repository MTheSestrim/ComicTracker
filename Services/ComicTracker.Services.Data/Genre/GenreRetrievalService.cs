namespace ComicTracker.Services.Data.Genre
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Genre.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class GenreRetrievalService : IGenreRetrievalService
    {
        private readonly ComicTrackerDbContext dbContext;

        public GenreRetrievalService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs() =>
            this.dbContext.Genres
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .Select(i => new
                {
                    i.Id,
                    i.Name,
                }).ToList()
            .Select(i => new KeyValuePair<string, string>(i.Id.ToString(), i.Name));
    }
}
