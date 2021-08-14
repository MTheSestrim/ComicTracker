namespace ComicTracker.Services.Data.Issue.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueRatingService
    {
        Task<int?> RateIssue(string userId, RateApiRequestModel model);
    }
}
