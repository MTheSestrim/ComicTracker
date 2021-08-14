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

        public int? CreateTemplateVolumes(TemplateCreateApiRequestModel model)
        {
            var volumesWithSeriesId = this.dbContext.Series.Include(s => s.Volumes).Select(s => new { s.Id, s.Volumes }).FirstOrDefault(s => s.Id == model.SeriesId);

            if (volumesWithSeriesId.Volumes.Any() || model.NumberOfEntities < 1)
            {
                return null;
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
