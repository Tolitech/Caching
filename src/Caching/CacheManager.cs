using System;
using Microsoft.Extensions.Caching.Memory;
using Tolitech.CodeGenerator.Caching.Models;

namespace Tolitech.CodeGenerator.Caching
{
    public sealed class CacheManager
    {
        private static readonly MemoryCache _cache;
        private static readonly List<RegionInfo> _regions;

        private const string demiliter = "#";

        static CacheManager()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _regions = new List<RegionInfo>();
        }

        public static void Add(string regionName, string keyName, object item, TimeSpan ts)
        {
            if (string.IsNullOrEmpty(keyName))
                return;

            lock (_regions)
            {
                // Adding the region if it does not already exist.
                var region = _regions
                    .Where(x => x.RegionName.ToLower() == regionName.ToLower())
                    .FirstOrDefault();

                if (region == null)
                {
                    region = new RegionInfo(regionName);
                    _regions.Add(region);
                }

                // Adding the key name if it does not already exist.
                var cacheInfo = region.Caches
                    .Where(x => x.Key.ToLower() == keyName.ToLower())
                    .FirstOrDefault();

                if (cacheInfo == null)
                {
                    cacheInfo = new CacheInfo(keyName.ToLower(), DateTime.Now, DateTime.Now.Add(ts));
                    region.AddCache(cacheInfo);
                }
                else
                {
                    cacheInfo.Set(DateTime.Now, DateTime.Now.Add(ts));
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(ts))
                };

                cacheEntryOptions.RegisterPostEvictionCallback(EvictionCallback);

                _cache.Set(GetKey(regionName, keyName), item, cacheEntryOptions);
            }
        }

        public static object Get(string regionName, string keyName)
        {
            return _cache.Get(GetKey(regionName, keyName));
        }

        public static void Remove(string regionName)
        {
            lock (_regions)
            {
                // Getting the region.
                var region = _regions.Where(x => x.RegionName.ToLower() == regionName.ToLower()).FirstOrDefault();

                // If there is region, traversing each item and removing the data from the cache.
                if (region != null)
                {
                    for (int i = region.Caches.Count - 1; i >= 0; i--)
                    {
                        var cacheInfo = region.Caches.ElementAt(i);
                        Remove(regionName, cacheInfo.Key);
                    }
                }
            }
        }

        public static void Remove(string regionName, string keyName)
        {
            _cache.Remove(GetKey(regionName, keyName));
        }

        public static void RemoveAll()
        {
            for (int i = _regions.Count - 1; i >= 0; i--)
            {
                var region = _regions[i];
                Remove(region.RegionName);
            }
        }

        public static void Refresh()
        {
            lock (_regions)
            {
                foreach (var region in _regions)
                {
                    foreach (var cache in region.Caches.ToList())
                    {
                        if (cache.ExpirationDate < DateTime.Now)
                            Get(region.RegionName, cache.Key);
                    }
                }
            }
        }

        public static IReadOnlyCollection<RegionInfo> Regions { get { return _regions.AsReadOnly(); } }

        #region Private methods

        /// <summary>
        /// Remove callback
        /// </summary>
        /// <param name="args">args</param>
        private static void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            lock (_regions)
            {
                // Getting region and key name.
                var keys = key
                    .ToString()
                    .Split(new string[] { demiliter }, StringSplitOptions.None);

                // Getting the region.
                var region = _regions
                    .Where(x => x.RegionName.ToLower() == keys[0].ToLower())
                    .FirstOrDefault();

                // If the region exists and contains the key, removing it.
                if (region != null)
                {
                    var cacheInfo = region.GetCache(keys[1]);

                    if (cacheInfo != null)
                    {
                        region.RemoveCache(cacheInfo);

                        if (region.Caches.Count == 0)
                        {
                            _regions.Remove(region);
                        }
                    }
                }
            }
        }

        private static string GetKey(string regionName, string keyName)
        {
            return regionName.ToLower() + demiliter + keyName.ToLower();
        }

        #endregion
    }
}