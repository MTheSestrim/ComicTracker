namespace ComicTracker.Tests.Mocks.Services.Arc
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MockArcEditingInfoService : IArcEditingInfoService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public MockArcEditingInfoService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public EditInfoSeriesRelatedEntityServiceModel GetArc(int arcId)
        {
            var currentArc = this.dbContext.Arcs
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
