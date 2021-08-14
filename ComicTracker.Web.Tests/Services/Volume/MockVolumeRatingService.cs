namespace ComicTracker.Tests.Services.Volume
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    public class MockVolumeRatingService : IVolumeRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockVolumeRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> RateVolume(string userId, RateApiRequestModel model)
        {
            var volume = this.dbContext.Volumes.Find(model.Id);

            if (volume != null)
            {
                var userVolume = volume.UsersVolumes.FirstOrDefault(ua => ua.UserId == userId);

                if (userVolume == null)
                {
                    userVolume = new UserVolume
                    {
                        UserId = userId,
                        VolumeId = model.Id,
                        Score = model.Score,
                    };

                    volume.UsersVolumes.Add(userVolume);

                    this.dbContext.Update(volume);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userVolume.Score = model.Score;

                    this.dbContext.Update(volume);
                    await this.dbContext.SaveChangesAsync();
                }

                return model.Score;
            }

            return null;
        }
    }
}
