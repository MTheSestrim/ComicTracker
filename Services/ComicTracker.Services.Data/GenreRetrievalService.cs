namespace ComicTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class GenreRetrievalService : IGenreRetrievalService
    {
        private readonly IDeletableEntityRepository<Genre> genreRepository;

        public GenreRetrievalService(IDeletableEntityRepository<Genre> genresRepository)
        {
            this.genreRepository = genresRepository;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync() =>
            (await this.genreRepository.AllAsNoTracking().Select(i => new
                {
                    i.Id,
                    i.Name,
                }).ToListAsync())
            .Select(i => new KeyValuePair<string, string>(i.Id.ToString(), i.Name));
    }
}
