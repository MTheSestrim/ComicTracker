namespace ComicTracker.Services.Data.Arc
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ArcEditingInfoService : IArcEditingInfoService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public ArcEditingInfoService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public EditInfoSeriesRelatedEntityServiceModel GetArc(int arcId)
        {
            var currentArc = this.dbContext.Arcs
               .AsNoTracking()
               .ProjectTo<EditInfoSeriesRelatedEntityServiceModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefault(a => a.Id == arcId);

            if (currentArc == null)
            {
                return null;
            }

            return currentArc;
        }
    }
}
