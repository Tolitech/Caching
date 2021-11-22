using System;
using System.Threading;
using Xunit;

namespace Tolitech.CodeGenerator.Caching.Tests
{
    public class CacheManagerTest
    {
        [Fact(DisplayName = "CacheManager - Add - Valid")]
        public void CacheManager_Add_Valid()
        {
            const string regionName = "RegionName";
            const string keyName = "KeyName";
            const string value = "Hello Cache";

            CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(10));
            Assert.True(CacheManager.Get(regionName, keyName).ToString() == value);
        }

        [Fact(DisplayName = "CacheManager - AddExistingCached - Valid")]
        public void CacheManager_AddExistingCached_Valid()
        {
            const string regionName = "RegionName";
            const string keyName = "KeyName";
            const string value1 = "Hello Cache 1";
            const string value2 = "Hello Cache 2";

            CacheManager.Add(regionName, keyName, value1, TimeSpan.FromSeconds(10));
            Assert.True(CacheManager.Get(regionName, keyName).ToString() == value1);

            CacheManager.Add(regionName, keyName, value2, TimeSpan.FromSeconds(10));
            Assert.True(CacheManager.Get(regionName, keyName).ToString() == value2);
        }

        [Fact(DisplayName = "CacheManager - AddWithExpiration - Valid")]
        public void CacheManager_AddWithExpiration_Valid()
        {
            const string regionName = "RegionName";
            const string keyName = "KeyName";
            const string value = "Hello Cache";

            CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(5));
            Assert.True(CacheManager.Get(regionName, keyName) != null);
            Thread.Sleep(6000);
            Assert.True(CacheManager.Get(regionName, keyName) == null);
        }

        [Fact(DisplayName = "CacheManager - AddWithKeyRegionNameEmpty - Valid")]
        public void CacheManager_AddWithRegionNameNull_Valid()
        {
            const string regionName = "";
            const string keyName = "KeyName";
            const string value = "Hello Cache";

            CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(10));
            Assert.True(CacheManager.Get(regionName, keyName) != null);
        }

        [Fact(DisplayName = "CacheManager - AddWithKeyNameEmpty - Invalid")]
        public void CacheManager_AddWithKeyNameNull_Valid()
        {
            const string regionName = "RegionName";
            const string keyName = "";
            const string value = "Hello Cache";

            CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(10));
            Assert.False(CacheManager.Get(regionName, keyName) != null);
        }

        [Fact(DisplayName = "CacheManager - RemoveRegion - Valid")]
        public void CacheManager_RemoveRegion_Valid()
        {
            const string regionName1 = "RegionName1";
            const string keyName1 = "KeyName1";
            const string value1 = "Hello Cache 1";

            const string regionName2 = "RegionName2";
            const string keyName2 = "KeyName2";
            const string value2 = "Hello Cache 2";

            CacheManager.Add(regionName1, keyName1, value1, TimeSpan.FromSeconds(10));
            CacheManager.Add(regionName2, keyName2, value2, TimeSpan.FromSeconds(10));
            CacheManager.Remove(regionName1);

            Assert.True(CacheManager.Get(regionName1, keyName1) == null);
            Assert.True(CacheManager.Get(regionName2, keyName2) != null);
        }

        [Fact(DisplayName = "CacheManager - RemoveKey - Valid")]
        public void CacheManager_RemoveKey_Valid()
        {
            const string regionName = "RegionName";
            const string keyName1 = "KeyName1";
            const string keyName2 = "KeyName2";
            const string value = "Hello Cache";

            CacheManager.Add(regionName, keyName1, value, TimeSpan.FromSeconds(10));
            CacheManager.Add(regionName, keyName2, value, TimeSpan.FromSeconds(10));
            CacheManager.Remove(regionName, keyName1);

            Assert.True(CacheManager.Get(regionName, keyName1) == null);
            Assert.True(CacheManager.Get(regionName, keyName2) != null);
        }

        [Fact(DisplayName = "CacheManager - RemoveAll - Valid")]
        public void CacheManager_RemoveAll_Valid()
        {
            const string regionName1 = "RegionNameRemoveAll1";
            const string keyName1 = "KeyNameRemoveAll1";
            const string value1 = "Hello Cache 1";

            const string regionName2 = "RegionNameRemoveAll2";
            const string keyName2 = "KeyNameRemoveAll2";
            const string value2 = "Hello Cache 2";

            CacheManager.Add(regionName1, keyName1, value1, TimeSpan.FromSeconds(10));
            CacheManager.Add(regionName2, keyName2, value2, TimeSpan.FromSeconds(10));
            CacheManager.RemoveAll();

            var value1Cached = CacheManager.Get(regionName1, keyName1);
            var value2Cached = CacheManager.Get(regionName2, keyName2);

            Assert.True(value1Cached == null);
            Assert.True(value2Cached == null);
        }

        [Fact(DisplayName = "CacheManager - Refresh - Valid")]
        public void CacheManager_Refresh_Valid()
        {
            const string regionName1 = "RegionName1";
            const string keyName1 = "KeyName1";
            const string value1 = "Hello Cache 1";

            CacheManager.Add(regionName1, keyName1, value1, TimeSpan.FromSeconds(10));
            var regions = CacheManager.Regions;

            CacheManager.Add(regionName1, keyName1, value1, TimeSpan.FromSeconds(0));
            CacheManager.Refresh();

            Assert.True(CacheManager.Get(regionName1, keyName1) == null);
        }
    }
}