namespace ComicTracker.Tests.Data.Issue
{
    using ComicTracker.Data.Models.Entities;

    public class IssueSample
    {
        public static Issue IssueWithId(int id) => new() { Id = id };

        public static Issue IssueWithIdAndScore(int id, int? score, string userId)
        {
            var issue = new Issue() { Id = id };
            issue.UsersIssues.Add(new UserIssue { Score = score, UserId = userId });

            return issue;
        }
    }
}
