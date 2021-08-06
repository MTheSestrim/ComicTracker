namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeEditingInfoService : IVolumeEditingInfoService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public VolumeEditingInfoService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public EditInfoSeriesRelatedEntityServiceModel GetVolume(int volumeId)
        {
            var currentVolume = this.dbContext.Volumes
               .AsNoTracking()
               .ProjectTo<EditInfoSeriesRelatedEntityServiceModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefault(v => v.Id == volumeId);

            if (currentVolume == null)
            {
                return null;
            }

            return currentVolume;
        }
    }
}
