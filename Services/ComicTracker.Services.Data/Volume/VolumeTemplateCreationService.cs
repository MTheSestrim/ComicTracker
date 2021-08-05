namespace ComicTracker.Services.Data.Volume
{
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    public class VolumeTemplateCreationService : IVolumeTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public VolumeTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateTemplateVolumes(TemplateCreateApiRequestModel model)
        {
            if (this.dbContext.Series.Find(model.SeriesId).Volumes.Count > 0)
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
