namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class IssueController : BaseController
    {
        private readonly IIssueDetailsService issueDetailsService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public IssueController(
            IIssueDetailsService issueDetailsService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.issueDetailsService = issueDetailsService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        public IActionResult Index(int id)
        {
            var currentIssue = this.cache.GetIssueDetails(id);

            if (currentIssue == null)
            {
                currentIssue = this.issueDetailsService.GetIssue(id, this.User.GetId());

                if (currentIssue == null)
                {
                    return this.NotFound(currentIssue);
                }

                this.cache.SetIssueDetails(currentIssue, this.cacheKeyHolder);
            }

            return this.View(currentIssue);
        }
    }
}
