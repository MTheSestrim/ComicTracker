namespace ComicTracker.Tests.Mocks.Services.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;

    public class MockListArcService : IListArcService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockListArcService(ComicTrackerDbContext dbContext)
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

            if (arc.UsersArcs.Any(ua => ua.UserId == userId))
            {
                throw new InvalidOperationException("User has already added given arc to their list.");
            }

            var userArc = new UserArc { UserId = userId, Arc = arc };

            arc.UsersArcs.Add(userArc);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveArcFromList(string userId, int id)
        {
            var arc = this.dbContext.Arcs.Find(id);

            if (arc == null)
            {
                throw new KeyNotFoundException("Arc with given id does not exist.");
            }

            var userArc = arc.UsersArcs.FirstOrDefault(ua => ua.UserId == userId && ua.ArcId == id);

            if (userArc == null)
            {
                throw new InvalidOperationException("User does not have given arc in their list.");
            }

            arc.UsersArcs.Remove(userArc);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
