namespace ComicTracker.Web.Tests.Data.Arc
{
    using ComicTracker.Data.Models.Entities;

    public class ArcSample
    {
        public static Arc ArcWithId(int id) => new() { Id = id };
    }
}
