namespace ComicTracker.Tests.Mocks.Services.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;

    public class MockListIssueService : IListIssueService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockListIssueService(ComicTrackerDbContext dbContext)
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

            if (issue.UsersIssues.Any(ua => ua.UserId == userId))
            {
                throw new InvalidOperationException("User has already added given issue to their list.");
            }

            var userIssue = new UserIssue { UserId = userId, Issue = issue };

            issue.UsersIssues.Add(userIssue);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveIssueFromList(string userId, int id)
        {
            var issue = this.dbContext.Issues.Find(id);

            if (issue == null)
            {
                throw new KeyNotFoundException("Issue with given id does not exist.");
            }

            var userIssue = issue.UsersIssues.FirstOrDefault(ua => ua.UserId == userId && ua.IssueId == id);

            if (userIssue == null)
            {
                throw new InvalidOperationException("User does not have given issue in their list.");
            }

            issue.UsersIssues.Remove(userIssue);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
