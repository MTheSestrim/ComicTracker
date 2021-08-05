namespace ComicTracker.Services.Data.Arc
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ArcRatingService : IArcRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ArcRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateArc(string userId, RateApiRequestModel model)
        {
            var arc = await this.dbContext.Arcs
                .Include(a => a.UsersArcs)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

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

                    this.dbContext.Arcs.Update(arc);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userArc.Score = model.Score;

                    this.dbContext.Arcs.Update(arc);
                    await this.dbContext.SaveChangesAsync();
                }

                return model.Score;
            }

            return 0;
        }
    }
}
