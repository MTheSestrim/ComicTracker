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

    public class ListArcService : IListArcService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ListArcService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddArcToList(string userId, int id)
        {
            var arc = this.dbContext.Arcs.Find(id);

            if (arc == null)
            {
                throw new KeyNotFoundException("Arc with given id does not exist.");
            }

            var user = this.dbContext.Users
                .Include(u => u.UsersArcs)
                .FirstOrDefault(u => u.Id == userId);

            if (user.UsersArcs.Any(uv => uv.Arc == arc))
            {
                throw new InvalidOperationException("User has already added given arc to their list.");
            }

            var userArc = new UserArc { User = user, Arc = arc };

            user.UsersArcs.Add(userArc);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveArcFromList(string userId, int id)
        {
            var arc = this.dbContext.Arcs.Find(id);

            if (arc == null)
            {
                throw new KeyNotFoundException("Arc with given id does not exist.");
            }

            var user = this.dbContext.Users
                .Include(u => u.UsersArcs)
                .FirstOrDefault(u => u.Id == userId);

            if (!user.UsersArcs.Any(uv => uv.Arc == arc))
            {
                throw new InvalidOperationException("User does not have given arc in their list.");
            }

            var userArc = new UserArc { User = user, Arc = arc };

            user.UsersArcs.Remove(userArc);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
