namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeTemplateCreationService : IVolumeTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public VolumeTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateTemplateVolumes(TemplateCreateApiRequestModel model)
        {
            if (this.dbContext.Series.Include(s => s.Volumes).Select(s => s.Volumes).Any())
            {
                return -1;
            }

            var templateVolumes = new Volume[model.NumberOfEntities];

            for (int i = 0; i < model.NumberOfEntities; i++)
            {
                var templateVolume = new Volume
                {
                    Number = i + 1,
                    SeriesId = model.SeriesId,
                };

                templateVolumes[i] = templateVolume;
            }

            this.dbContext.Volumes.AddRange(templateVolumes);
            this.dbContext.SaveChanges();

            return model.NumberOfEntities;
        }
    }
}
