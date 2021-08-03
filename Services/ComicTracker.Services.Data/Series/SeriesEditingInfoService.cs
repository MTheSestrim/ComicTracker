namespace ComicTracker.Services.Data.Series
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    public class SeriesEditingInfoService : ISeriesEditingInfoService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;
        private readonly IMapper mapper;

        public SeriesEditingInfoService(IDeletableEntityRepository<Series> seriesRepository, IMapper mapper)
        {
            this.seriesRepository = seriesRepository;
            this.mapper = mapper;
        }

        public EditInfoSeriesServiceModel GetSeries(int seriesId)
        {
            var currentSeries = this.seriesRepository
               .AllAsNoTracking()
               .ProjectTo<EditInfoSeriesServiceModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefault(s => s.Id == seriesId);

            if (currentSeries == null)
            {
                return null;
            }

            return currentSeries;
        }
    }
}
