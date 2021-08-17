namespace ComicTracker.Tests.Data.Arc
{
    using ComicTracker.Data.Models.Entities;

    public class ArcSample
    {
        public static Arc ArcWithId(int id) => new () { Id = id };

        public static Arc ArcWithIdAndScore(int id, int? score, string userId)
        {
            var arc = new Arc() { Id = id };
            arc.UsersArcs.Add(new UserArc { Score = score, UserId = userId });

            return arc;
        }
    }
}
