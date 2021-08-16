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

    public class ListIssueService : IListIssueService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ListIssueService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddIssueToList(string userId, int id)
        {
            var issue = this.dbContext.Issues.Find(id);

            if (issue == null)
            {
                throw new KeyNotFoundException("Issue with given id does not exist.");
            }

            var user = this.dbContext.Users
                .Include(u => u.UsersIssues)
                .FirstOrDefault(u => u.Id == userId);

            if (user.UsersIssues.Any(uv => uv.Issue == issue))
            {
                throw new InvalidOperationException("User has already added given issue to their list.");
            }

            var userIssue = new UserIssue { User = user, Issue = issue };

            user.UsersIssues.Add(userIssue);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveIssueFromList(string userId, int id)
        {
            var issue = this.dbContext.Issues.Find(id);

            if (issue == null)
            {
                throw new KeyNotFoundException("Issue with given id does not exist.");
            }

            var user = this.dbContext.Users
                .Include(u => u.UsersIssues)
                .FirstOrDefault(u => u.Id == userId);

            if (!user.UsersIssues.Any(uv => uv.Issue == issue))
            {
                throw new InvalidOperationException("User does not have given issue in their list.");
            }

            var userIssue = new UserIssue { User = user, Issue = issue };

            user.UsersIssues.Remove(userIssue);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
