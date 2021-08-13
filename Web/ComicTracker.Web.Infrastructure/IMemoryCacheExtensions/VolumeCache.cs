namespace ComicTracker.Web.Infrastructure.IMemoryCacheExtensions
{
    using System;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Volume.Models;

    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public static class VolumeCache
    {
        public static VolumeDetailsServiceModel GetVolumeDetails(this IMemoryCache cache, int volumeId)
            => cache.Get<VolumeDetailsServiceModel>(GetCacheKey(volumeId));

        public static void SetVolumeDetails(
            this IMemoryCache cache,
            VolumeDetailsServiceModel volume,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            cache.Set(GetCacheKey(volume.Id), volume, cacheOptions);

            cacheKeyHolder.AddKey(nameof(VolumeDetailsServiceModel), volume.Id);
        }

        public static void RemoveVolumeDetails(this IMemoryCache cache, int volumeId)
            => cache.Remove(GetCacheKey(volumeId));

        public static void RemoveAllVolumeDetails(
            this IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            var keys = cacheKeyHolder.GetKeys(nameof(VolumeDetailsServiceModel));

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    RemoveVolumeDetails(cache, key);
                }

                cacheKeyHolder.Clear(nameof(VolumeDetailsServiceModel));
            }
        }

        private static string GetCacheKey(int volumeId) => VolumeDetailsCacheKey + volumeId.ToString();
    }
}
