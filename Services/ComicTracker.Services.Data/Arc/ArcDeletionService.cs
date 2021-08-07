namespace ComicTracker.Services.Data.Arc
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Arc.Contracts;

    public class ArcDeletionService : IArcDeletionService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ArcDeletionService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int DeleteArc(int arcId)
        {
            var arc = this.dbContext.Arcs.Find(arcId);

            if (arc == null)
            {
                return -1;
            }

            var seriesId = arc.SeriesId;

            this.dbContext.Delete(arc);
            this.dbContext.SaveChanges();

            return seriesId;
        }
    }
}
