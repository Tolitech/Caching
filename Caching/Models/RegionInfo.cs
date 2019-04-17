using System;
using System.Collections.Generic;
using System.Linq;

namespace Tolitech.CodeGenerator.Caching.Models
{
    public class RegionInfo
    {
        private readonly List<CacheInfo> _caches;

        public RegionInfo(string regionName)
        {
            _caches = new List<CacheInfo>();
            RegionName = regionName;
        }

        public string RegionName { get; private set; }

        public IReadOnlyCollection<CacheInfo> Caches { get { return _caches.AsReadOnly(); } }

        internal void AddCache(CacheInfo cache)
        {
            _caches.Add(cache);
        }

        internal void RemoveCache(CacheInfo cache)
        {
            _caches.Remove(cache);
        }

        internal CacheInfo GetCache(string key)
        {
            return _caches.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
        }
    }
}
