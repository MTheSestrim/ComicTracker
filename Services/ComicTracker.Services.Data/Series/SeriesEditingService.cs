namespace ComicTracker.Services.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;
    using Microsoft.EntityFrameworkCore;

    using static ComicTracker.Services.FileUploadLocator;

    public class SeriesEditingService : ISeriesEditingService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;
        private readonly IDeletableEntityRepository<Genre> genresRepository;

        public SeriesEditingService(
            IDeletableEntityRepository<Series> seriesRepository,
            IDeletableEntityRepository<Genre> genresRepository)
        {
            this.seriesRepository = seriesRepository;
            this.genresRepository = genresRepository;
        }

        public async Task<int> EditSeriesAsync(EditSeriesServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.genresRepository.All()
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var currentSeries = this.seriesRepository.All().Include(s => s.Genres).FirstOrDefault(s => s.Id == model.Id);

            if (currentSeries == null)
            {
                return -1;
            }

            currentSeries.Name = model.Title;
            currentSeries.Description = model.Description;
            currentSeries.Ongoing = model.Ongoing;
            currentSeries.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = await GetUploadedFileNameAsync(model.CoverImage);

                // Delete old cover image and replace it with the new one.
                DeleteCover(currentSeries.CoverPath);
                currentSeries.CoverPath = uniqueFileName;
            }
            else if (model.CoverPath != null)
            {
                currentSeries.CoverPath = model.CoverPath;
            }

            this.seriesRepository.Update(currentSeries);
            await this.seriesRepository.SaveChangesAsync();

            return currentSeries.Id;
        }
    }
}
