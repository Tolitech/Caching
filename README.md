# Tolitech.CodeGenerator.Caching
Caching library used in projects created by the Code Generator tool.

This project aims to facilitate the use of in-memory cache, organizing objects into regions and allowing them to be cleaned by their regions, keys or expiration time. 

Tolitech Code Generator Tool: [http://www.tolitech.com.br](https://www.tolitech.com.br/)

Examples:

```
const string regionName = "RegionName";
const string keyName = "KeyName";
const string value = "Hello Cache";
```

```
CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(10));
string valueCached = (string)CacheManager.Get(regionName, keyName); // the value is "Hello Cache"
```

```
CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(5));
var value1Cached = CacheManager.Get(regionName, keyName); // the value is "Hello Cache";

Thread.Sleep(6000);
var value2Cached = CacheManager.Get(regionName, keyName); // the value is null;
```

```
CacheManager.Add(regionName, keyName, value, TimeSpan.FromSeconds(5));
var value1Cached = CacheManager.Get(regionName, keyName); // the value is "Hello Cache";

CacheManager.Remove(regionName);
var value2Cached = CacheManager.Get(regionName, keyName); // the value is null;
```
