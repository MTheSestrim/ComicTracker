namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeRatingService : IVolumeRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public VolumeRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateVolume(string userId, int volumeId, int score)
        {
            var volume = await this.dbContext.Volumes
                .Include(v => v.UsersVolumes)
                .FirstOrDefaultAsync(v => v.Id == volumeId);

            if (volume != null)
            {
                var userVolume = volume.UsersVolumes.FirstOrDefault(uv => uv.UserId == userId);

                if (userVolume == null)
                {
                    userVolume = new UserVolume
                    {
                        UserId = userId,
                        VolumeId = volumeId,
                        Score = score,
                    };

                    volume.UsersVolumes.Add(userVolume);

                    this.dbContext.Volumes.Update(volume);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userVolume.Score = score;

                    this.dbContext.Volumes.Update(volume);
                    await this.dbContext.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
