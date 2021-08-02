namespace ComicTracker.Services.Data.Arc
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class ArcRatingService : IArcRatingService
    {
        private readonly IDeletableEntityRepository<Arc> arcsRepository;

        public ArcRatingService(IDeletableEntityRepository<Arc> arcsRepository)
        {
            this.arcsRepository = arcsRepository;
        }

        public async Task<int> RateArc(string userId, int arcId, int score)
        {
            var arc = await this.arcsRepository.All()
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

                    this.arcsRepository.Update(arc);
                    await this.arcsRepository.SaveChangesAsync();
                }
                else
                {
                    userArc.Score = score;

                    this.arcsRepository.Update(arc);
                    await this.arcsRepository.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
