namespace ComicTracker.Tests.Mocks.Services.Volume
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Services.Data.Volume.Models;

    public class MockVolumeDetailsService : IVolumeDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockVolumeDetailsService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public VolumeDetailsServiceModel GetVolume(int volumeId, string userId)
        {
            var currentVolume = dbContext.Volumes.Find(volumeId);

            if (currentVolume == null)
            {
                return null;
            }

            var serviceModel = new VolumeDetailsServiceModel { Id = currentVolume.Id };

            return serviceModel;
        }
    }
}
