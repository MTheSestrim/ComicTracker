namespace ComicTracker.Tests.Services.Volume
{
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    public class MockVolumeTemplateCreationService : IVolumeTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockVolumeTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateTemplateVolumes(TemplateCreateApiRequestModel model)
        {
            if (model.NumberOfEntities < 1)
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
