namespace ComicTracker.Tests.Data.Issue
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Models.Entities;

    public class IssueSample
    {
        public static Issue IssueWithId(int id) => new () { Id = id };

        public static Issue SREIssueWithIdAndNumber(int id, int number, int seriesId) => new ()
        {
            Id = id,
            Number = number,
            SeriesId = seriesId,
        };

        public static Issue IssueWithIdAndArc(int id, int arcId) => new ()
        {
            Id = id,
            ArcId = arcId,
        };

        public static Issue IssueWithIdAndVolume(int id, int volumeId) => new ()
        {
            Id = id,
            VolumeId = volumeId,
        };

        public static IEnumerable<Issue> TenSREIssuesWithIdsAndNumbers(int seriesId) => Enumerable.Range(1, 10)
            .Select(i => new Issue
            {
                Id = i,
                SeriesId = seriesId,
                Number = i,
            });

        public static Issue IssueWithIdAndScore(int id, int? score, string userId)
        {
            var issue = new Issue() { Id = id };
            issue.UsersIssues.Add(new UserIssue { Score = score, UserId = userId });

            return issue;
        }
    }
}
