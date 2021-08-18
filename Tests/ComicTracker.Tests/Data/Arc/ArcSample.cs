namespace ComicTracker.Tests.Data.Arc
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Models.Entities;

    public class ArcSample
    {
        public static Arc ArcWithId(int id) => new () { Id = id };

        public static Arc SREArcWithIdAndNumber(int id, int number, int seriesId) => new ()
        {
            Id = id,
            Number = number,
            SeriesId = seriesId,
        };

        public static Arc SREArcWithIdAndNumberAndVolumes(
            int id,
            int number,
            int seriesId,
            int minRange,
            int maxRange)
        {
            var arcsVolumes = Enumerable.Range(minRange, maxRange - minRange + 1)
                .Select(i => new ArcVolume
                {
                    ArcId = id,
                    VolumeId = i,
                }).ToList();

            return new Arc
            {
                Id = id,
                Number = number,
                SeriesId = seriesId,
                ArcsVolumes = arcsVolumes,
            };
        }

        public static Arc SREArcWithIdAndNumberAndIssues(
            int id,
            int number,
            int seriesId,
            int minRange,
            int maxRange)
        {
            var issues = Enumerable.Range(minRange, maxRange - minRange + 1)
                .Select(i => new Issue
                {
                    Id = i,
                    Number = i,
                    VolumeId = id,
                    SeriesId = seriesId,
                }).ToList();

            return new Arc
            {
                Id = id,
                Number = number,
                SeriesId = seriesId,
                Issues = issues,
            };
        }

        public static Arc ArcWithIdAndScore(int id, int? score, string userId)
        {
            var arc = new Arc() { Id = id };
            arc.UsersArcs.Add(new UserArc { Score = score, UserId = userId });

            return arc;
        }

        public static IEnumerable<Arc> TenSREArcsWithIdsAndNumbers(int seriesId) => Enumerable.Range(1, 10)
            .Select(i => new Arc
            {
                Id = i,
                SeriesId = seriesId,
                Number = i,
            });
    }
}
