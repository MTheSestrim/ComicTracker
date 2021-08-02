namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeRatingService : IVolumeRatingService
    {
        private readonly IDeletableEntityRepository<Volume> volumesRepository;

        public VolumeRatingService(IDeletableEntityRepository<Volume> volumesRepository)
        {
            this.volumesRepository = volumesRepository;
        }

        public async Task<int> RateVolume(string userId, int volumeId, int score)
        {
            var volume = await this.volumesRepository.All()
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

                    this.volumesRepository.Update(volume);
                    await this.volumesRepository.SaveChangesAsync();
                }
                else
                {
                    userVolume.Score = score;

                    this.volumesRepository.Update(volume);
                    await this.volumesRepository.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
