namespace ComicTracker.Tests.Data.Arc
{
    using ComicTracker.Data.Models.Entities;
    using System.Linq;

    public class ArcSample
    {
        public static Arc ArcWithId(int id) => new () { Id = id };

        public static Arc SREArcWithIdAndNumber(int id, int number, int seriesId) => new ()
        {
            Id = id,
            Number = number,
            SeriesId = seriesId,
        };

        public static Arc ArcWithIdAndScore(int id, int? score, string userId)
        {
            var arc = new Arc() { Id = id };
            arc.UsersArcs.Add(new UserArc { Score = score, UserId = userId });

            return arc;
        }
    }
}
