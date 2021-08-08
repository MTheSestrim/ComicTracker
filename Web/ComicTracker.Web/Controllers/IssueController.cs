namespace ComicTracker.Web.Controllers
{
    using System;

    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Issues.Models;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public class IssueController : BaseController
    {
        private readonly IIssueDetailsService issueDetailsService;
        private readonly IMemoryCache cache;

        public IssueController(IIssueDetailsService issueDetailsService, IMemoryCache cache)
        {
            this.issueDetailsService = issueDetailsService;
            this.cache = cache;
        }

        public IActionResult Index(int id)
        {
            var cacheKey = IssueDetailsCacheKey + id.ToString();

            var currentIssue = this.cache.Get<IssueDetailsServiceModel>(cacheKey);

            if (currentIssue == null)
            {
                currentIssue = this.issueDetailsService.GetIssue(id, this.User.GetId());

                if (currentIssue == null)
                {
                    return this.NotFound(currentIssue);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                this.cache.Set(cacheKey, currentIssue, cacheOptions);
            }

            return this.View(currentIssue);
        }
    }
}
