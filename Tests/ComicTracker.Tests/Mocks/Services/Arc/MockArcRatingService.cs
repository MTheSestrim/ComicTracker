namespace ComicTracker.Tests.Mocks.Services.Arc
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class MockArcRatingService : IArcRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockArcRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> RateArc(string userId, RateApiRequestModel model)
        {
            var arc = this.dbContext.Arcs.Find(model.Id);

            if (arc != null)
            {
                var userArc = arc.UsersArcs.FirstOrDefault(ua => ua.UserId == userId);

                if (userArc == null)
                {
                    userArc = new UserArc
                    {
                        UserId = userId,
                        ArcId = model.Id,
                        Score = model.Score,
                    };

                    arc.UsersArcs.Add(userArc);

                    this.dbContext.Update(arc);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userArc.Score = model.Score;

                    this.dbContext.Update(arc);
                    await this.dbContext.SaveChangesAsync();
                }

                return model.Score;
            }

            return null;
        }
    }
}
