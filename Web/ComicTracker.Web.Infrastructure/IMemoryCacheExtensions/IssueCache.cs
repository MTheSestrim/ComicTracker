namespace ComicTracker.Web.Infrastructure.IMemoryCacheExtensions
{
    using System;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Issue.Models;

    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public static class IssueCache
    {
        public static IssueDetailsServiceModel GetIssueDetails(this IMemoryCache cache, int issueId)
            => cache.Get<IssueDetailsServiceModel>(GetCacheKey(issueId));

        public static void SetIssueDetails(
            this IMemoryCache cache,
            IssueDetailsServiceModel issue,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            cache.Set(GetCacheKey(issue.Id), issue, cacheOptions);

            cacheKeyHolder.AddKey(nameof(IssueDetailsServiceModel), issue.Id);
        }

        public static void RemoveIssueDetails(this IMemoryCache cache, int issueId)
            => cache.Remove(GetCacheKey(issueId));

        public static void RemoveAllIssueDetails(
            this IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var keys = cacheKeyHolder.GetKeys(nameof(IssueDetailsServiceModel));

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    RemoveIssueDetails(cache, key);
                }

                cacheKeyHolder.Clear(nameof(IssueDetailsServiceModel));
            }
        }

        private static string GetCacheKey(int issueId) => IssueDetailsCacheKey + issueId.ToString();
    }
}
