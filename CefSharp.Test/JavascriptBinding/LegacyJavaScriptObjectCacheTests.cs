using System.Collections.Generic;
using CefSharp.Internals;
using Xunit;

namespace CefSharp.Test.JavascriptBinding
{
    public class LegacyJavaScriptObjectCacheTests
    {
        private const int BrowserId = 1;

        [Fact]
        public void InsertOrUpdateShouldAddObjectsToCache()
        {
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" },
               new JavascriptObject { Name = "Object2" }
            };

            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.Contains(javascriptObjects[0], cachedValues);
            Assert.Contains(javascriptObjects[1], cachedValues);
        }

        [Fact]
        public void GetCacheValuesShouldReturnAllCachedObjects()
        {
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" },
               new JavascriptObject { Name = "Object2" }
            };
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            var cachedValues = cache.GetCacheValues(BrowserId);

            Assert.Equal(2, cachedValues.Count);
        }

        [Fact]
        public void GetCacheShouldReturnUnderlyingDictionary()
        {
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            var cachedDictionary = cache.GetCache(BrowserId);

            Assert.Single(cachedDictionary);
            Assert.True(cachedDictionary.ContainsKey("Object1"));
        }

        [Fact]
        public void InsertOrUpdateShouldReplaceExistingObjects()
        {
            var cache = new LegacyJavaScriptObjectCache();
            var initialObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            var updatedObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            cache.InsertOrUpdate(BrowserId, initialObjects);

            cache.InsertOrUpdate(BrowserId, updatedObjects);

            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.DoesNotContain(initialObjects[0], cachedValues);
            Assert.Contains(updatedObjects[0], cachedValues);
        }

        [Fact]
        public void InsertOrUpdateShouldAppendObjectsWithDifferentNames()
        {
            var cache = new LegacyJavaScriptObjectCache();
            var initialObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            var updatedObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object2" }
            };
            cache.InsertOrUpdate(BrowserId, initialObjects);

            cache.InsertOrUpdate(BrowserId, updatedObjects);

            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.Contains(initialObjects[0], cachedValues);
            Assert.Contains(updatedObjects[0], cachedValues);
        }

        [Fact]
        public void ClearCacheShouldDoNothing()
        {
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            cache.ClearCache(BrowserId);

            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.Contains(javascriptObjects[0], cachedValues);
        }
    }
}
