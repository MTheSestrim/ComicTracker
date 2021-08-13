namespace ComicTracker.Web.Infrastructure.IMemoryCacheExtensions
{
    using System;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public static class SeriesCache
    {
        public static SeriesDetailsServiceModel GetSeriesDetails(this IMemoryCache cache, int seriesId)
            => cache.Get<SeriesDetailsServiceModel>(GetCacheKey(seriesId));

        public static void SetSeriesDetails(
            this IMemoryCache cache,
            SeriesDetailsServiceModel series,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(3));

            cache.Set(GetCacheKey(series.Id), series, cacheOptions);

            cacheKeyHolder.AddKey(nameof(SeriesDetailsServiceModel), series.Id);
        }

        public static void RemoveSeriesDetails(this IMemoryCache cache, int seriesId)
            => cache.Remove(GetCacheKey(seriesId));

        public static void RemoveAllSeriesDetails(
            this IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var keys = cacheKeyHolder.GetKeys(nameof(SeriesDetailsServiceModel));

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    RemoveSeriesDetails(cache, key);
                }

                cacheKeyHolder.Clear(nameof(SeriesDetailsServiceModel));
            }
        }

        private static string GetCacheKey(int seriesId) => SeriesDetailsCacheKey + seriesId.ToString();
    }
}
