namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeRatingService : IVolumeRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public VolumeRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateVolume(string userId, RateApiRequestModel model)
        {
            var volume = await this.dbContext.Volumes
                .Include(v => v.UsersVolumes)
                .FirstOrDefaultAsync(v => v.Id == model.Id);

            if (volume != null)
            {
                var userVolume = volume.UsersVolumes.FirstOrDefault(uv => uv.UserId == userId);

                if (userVolume == null)
                {
                    userVolume = new UserVolume
                    {
                        UserId = userId,
                        VolumeId = model.Id,
                        Score = model.Score,
                    };

                    volume.UsersVolumes.Add(userVolume);

                    this.dbContext.Volumes.Update(volume);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userVolume.Score = model.Score;

                    this.dbContext.Volumes.Update(volume);
                    await this.dbContext.SaveChangesAsync();
                }

                return model.Score;
            }

            return 0;
        }
    }
}
