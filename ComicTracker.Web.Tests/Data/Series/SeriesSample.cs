namespace ComicTracker.Tests.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Models.Entities;

    public class SeriesSample
    {
        public static IEnumerable<Series> FiftySeries => Enumerable
            .Range(1, 50)
            .Select(i => new Series() 
            {
                UsersSeries = new List<UserSeries>() { new UserSeries { UserId = (i % 5).ToString() } }
            });

        public static Series SeriesWithId(int id) => new() { Id = id };

        public static Series SeriesWithIdAndScore(int id, int score, string userId)
        {
            var series = new Series() { Id = id };
            series.UsersSeries.Add(new UserSeries { Score = score, UserId = userId });

            return series;
        }
    }
}
