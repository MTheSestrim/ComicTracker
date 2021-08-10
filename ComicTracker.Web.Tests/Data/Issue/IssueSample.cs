namespace ComicTracker.Web.Tests.Data.Issue
{
    using ComicTracker.Data.Models.Entities;

    public class IssueSample
    {
        public static Issue IssueWithId(int id) => new() { Id = id };
    }
}
