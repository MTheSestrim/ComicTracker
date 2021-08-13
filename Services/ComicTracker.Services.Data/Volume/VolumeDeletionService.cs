namespace ComicTracker.Services.Data.Volume
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Volume.Contracts;

    public class VolumeDeletionService : IVolumeDeletionService
    {
        private readonly ComicTrackerDbContext dbContext;

        public VolumeDeletionService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int? DeleteVolume(int volumeId)
        {
            var volume = this.dbContext.Volumes.Find(volumeId);

            if (volume == null)
            {
                return null;
            }

            var seriesId = volume.SeriesId;

            this.dbContext.Delete(volume);
            this.dbContext.SaveChanges();

            return seriesId;
        }
    }
}
