namespace ComicTracker.Services.Data.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class ListVolumeService : IListVolumeService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ListVolumeService(ComicTrackerDbContext dbContext)
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

            var user = this.dbContext.Users
                .Include(u => u.UsersVolumes)
                .FirstOrDefault(u => u.Id == userId);

            if (user.UsersVolumes.Any(uv => uv.Volume == volume))
            {
                throw new InvalidOperationException("User has already added given volume to their list.");
            }

            var userVolume = new UserVolume { User = user, Volume = volume };

            user.UsersVolumes.Add(userVolume);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveVolumeFromList(string userId, int id)
        {
            var volume = this.dbContext.Volumes.Find(id);

            if (volume == null)
            {
                throw new KeyNotFoundException("Volume with given id does not exist.");
            }

            var user = this.dbContext.Users
                .Include(u => u.UsersVolumes)
                .FirstOrDefault(u => u.Id == userId);

            if (!user.UsersVolumes.Any(uv => uv.Volume == volume))
            {
                throw new InvalidOperationException("User does not have given volume in their list.");
            }

            var userVolume = new UserVolume { User = user, Volume = volume };

            user.UsersVolumes.Remove(userVolume);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
