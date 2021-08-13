namespace ComicTracker.Web.Infrastructure.IMemoryCacheExtensions
{
    using System;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Arc.Models;

    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public static class ArcCache
    {
        public static ArcDetailsServiceModel GetArcDetails(this IMemoryCache cache, int arcId)
            => cache.Get<ArcDetailsServiceModel>(GetCacheKey(arcId));

        public static void SetArcDetails(
            this IMemoryCache cache,
            ArcDetailsServiceModel arc,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            cache.Set(GetCacheKey(arc.Id), arc, cacheOptions);

            cacheKeyHolder.AddKey(nameof(ArcDetailsServiceModel), arc.Id);
        }

        public static void RemoveArcDetails(this IMemoryCache cache, int arcId)
            => cache.Remove(GetCacheKey(arcId));

        public static void RemoveAllArcDetails(this IMemoryCache cache, ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var keys = cacheKeyHolder.GetKeys(nameof(ArcDetailsServiceModel));

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    RemoveArcDetails(cache, key);
                }

                cacheKeyHolder.Clear(nameof(ArcDetailsServiceModel));
            }
        }

        private static string GetCacheKey(int arcId) => ArcDetailsCacheKey + arcId.ToString();
    }
}
