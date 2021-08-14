namespace ComicTracker.Services.Data.Series
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using Microsoft.EntityFrameworkCore;

    public class SeriesEditingInfoService : ISeriesEditingInfoService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public SeriesEditingInfoService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public EditInfoSeriesServiceModel GetSeries(int seriesId)
        {
            var currentSeries = this.dbContext.Series
               .AsNoTracking()
               .Where(s => s.Id == seriesId)
               .ProjectTo<EditInfoSeriesServiceModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefault();

            if (currentSeries == null)
            {
                return null;
            }

            return currentSeries;
        }
    }
}
