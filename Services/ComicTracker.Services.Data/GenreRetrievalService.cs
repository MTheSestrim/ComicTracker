namespace ComicTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;

    public class GenreRetrievalService : IGenreRetrievalService
    {
        private readonly IDeletableEntityRepository<Genre> genreRepository;

        public GenreRetrievalService(IDeletableEntityRepository<Genre> genresRepository)
        {
            this.genreRepository = genresRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs() =>
            this.genreRepository.AllAsNoTracking().Select(i => new
            {
                i.Id,
                i.Name,
            }).ToList().Select(i => new KeyValuePair<string, string>(i.Id.ToString(), i.Name));
    }
}
