namespace ComicTracker.Services.Data.Arc
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class ArcRatingService : IArcRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ArcRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateArc(string userId, int arcId, int score)
        {
            var arc = await this.dbContext.Arcs
                .Include(a => a.UsersArcs)
                .FirstOrDefaultAsync(a => a.Id == arcId);

            if (arc != null)
            {
                var userArc = arc.UsersArcs.FirstOrDefault(ua => ua.UserId == userId);

                if (userArc == null)
                {
                    userArc = new UserArc
                    {
                        UserId = userId,
                        ArcId = arcId,
                        Score = score,
                    };

                    arc.UsersArcs.Add(userArc);

                    this.dbContext.Arcs.Update(arc);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userArc.Score = score;

                    this.dbContext.Arcs.Update(arc);
                    await this.dbContext.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
