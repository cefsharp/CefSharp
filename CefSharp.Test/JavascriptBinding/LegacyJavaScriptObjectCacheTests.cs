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
            // Arrange  
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" },
               new JavascriptObject { Name = "Object2" }
            };

            // Act  
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            // Assert  
            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.Contains(javascriptObjects[0], cachedValues);
            Assert.Contains(javascriptObjects[1], cachedValues);
        }

        [Fact]
        public void GetCacheValuesShouldReturnAllCachedObjects()
        {
            // Arrange  
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" },
               new JavascriptObject { Name = "Object2" }
            };
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            // Act  
            var cachedValues = cache.GetCacheValues(BrowserId);

            // Assert  
            Assert.Equal(2, cachedValues.Count);
        }

        [Fact]
        public void GetCacheShouldReturnUnderlyingDictionary()
        {
            // Arrange  
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            // Act  
            var cachedDictionary = cache.GetCache(BrowserId);

            // Assert  
            Assert.Single(cachedDictionary);
            Assert.True(cachedDictionary.ContainsKey("Object1"));
        }

        [Fact]
        public void InsertOrUpdateShouldReplaceExistingObjects()
        {
            // Arrange  
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

            // Act  
            cache.InsertOrUpdate(BrowserId, updatedObjects);

            // Assert  
            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.DoesNotContain(initialObjects[0], cachedValues);
            Assert.Contains(updatedObjects[0], cachedValues);
        }

        [Fact]
        public void InsertOrUpdateShouldAppendObjectsWithDifferentNames()
        {
            // Arrange  
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

            // Act  
            cache.InsertOrUpdate(BrowserId, updatedObjects);

            // Assert  
            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.Contains(initialObjects[0], cachedValues);
            Assert.Contains(updatedObjects[0], cachedValues);
        }

        [Fact]
        public void ClearCacheShouldDoNothing()
        {
            // Arrange  
            var cache = new LegacyJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
            {
               new JavascriptObject { Name = "Object1" }
            };
            cache.InsertOrUpdate(BrowserId, javascriptObjects);

            // Act  
            cache.ClearCache(BrowserId);

            // Assert  
            var cachedValues = cache.GetCacheValues(BrowserId);
            Assert.Contains(javascriptObjects[0], cachedValues);
        }
    }
}
