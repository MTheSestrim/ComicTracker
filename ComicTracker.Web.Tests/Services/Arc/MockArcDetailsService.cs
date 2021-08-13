namespace ComicTracker.Tests.Services.Arc
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Arc.Models;

    public class MockArcDetailsService : IArcDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockArcDetailsService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ArcDetailsServiceModel GetArc(int arcId, string userId)
        {
            var currentArc = dbContext.Arcs.Find(arcId);

            if (currentArc == null)
            {
                return null;
            }

            var serviceModel = new ArcDetailsServiceModel { Id = currentArc.Id };

            return serviceModel;
        }
    }
}
