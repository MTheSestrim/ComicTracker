namespace ComicTracker.Tests.Mocks.Services.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;

    public class MockListVolumeService : IListVolumeService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockListVolumeService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddVolumeToList(string userId, int id)
        {
            var volume = this.dbContext.Volumes.Find(id);

            if (volume == null)
            {
                throw new KeyNotFoundException("Volume with given id does not exist.");
            }

            if (volume.UsersVolumes.Any(ua => ua.UserId == userId))
            {
                throw new InvalidOperationException("User has already added given volume to their list.");
            }

            var userVolume = new UserVolume { UserId = userId, Volume = volume };

            volume.UsersVolumes.Add(userVolume);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveVolumeFromList(string userId, int id)
        {
            var volume = this.dbContext.Volumes.Find(id);

            if (volume == null)
            {
                throw new KeyNotFoundException("Volume with given id does not exist.");
            }

            var userVolume = volume.UsersVolumes.FirstOrDefault(ua => ua.UserId == userId && ua.VolumeId == id);

            if (userVolume == null)
            {
                throw new InvalidOperationException("User does not have given volume in their list.");
            }

            volume.UsersVolumes.Remove(userVolume);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
