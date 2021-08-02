namespace ComicTracker.Services.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using static ComicTracker.Services.FileUploadLocator;

    public class SeriesCreationService : ISeriesCreationService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;
        private readonly IDeletableEntityRepository<Genre> genresRepository;

        public SeriesCreationService(
            IDeletableEntityRepository<Series> seriesRepository,
            IDeletableEntityRepository<Genre> genresRepository)
        {
            this.seriesRepository = seriesRepository;
            this.genresRepository = genresRepository;
        }

        public async Task<int> CreateSeriesAsync(CreateSeriesServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.genresRepository.All()
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            Series newSeries = null;

            if (model.CoverImage == null)
            {
                newSeries = new Series
                {
                    Name = model.Name,
                    Description = model.Description,
                    CoverPath = model.CoverPath,
                    Ongoing = model.Ongoing,
                    Genres = selectedGenres,
                };
            }
            else
            {
                var uniqueFileName = await GetUploadedFileNameAsync(model.CoverImage);

                newSeries = new Series
                {
                    Name = model.Name,
                    Description = model.Description,
                    CoverPath = uniqueFileName,
                    Ongoing = model.Ongoing,
                    Genres = selectedGenres,
                };
            }

            await this.seriesRepository.AddAsync(newSeries);
            await this.seriesRepository.SaveChangesAsync();

            return newSeries.Id;
        }
    }
}
