namespace ComicTracker.Tests.Mocks.Services.Genre
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Genre.Contracts;

    public class MockGenreRetrievalService : IGenreRetrievalService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockGenreRetrievalService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs() =>
            this.dbContext.Genres
            .ToList()
            .OrderBy(g => g.Name)
            .Select(i => new
            {
                i.Id,
                i.Name,
            }).ToList()
            .Select(i => new KeyValuePair<string, string>(i.Id.ToString(), i.Name));
    }
}
