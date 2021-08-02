namespace ComicTracker.Services.Data.Series
{
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class SeriesDeletionService : ISeriesDeletionService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public SeriesDeletionService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public async Task<bool> DeleteSeries(int seriesId)
        {
            var series = await this.seriesRepository.All().FirstOrDefaultAsync(s => s.Id == seriesId);

            if (series == null)
            {
                return false;
            }

            this.seriesRepository.Delete(series);
            await this.seriesRepository.SaveChangesAsync();

            return true;
        }
    }
}
