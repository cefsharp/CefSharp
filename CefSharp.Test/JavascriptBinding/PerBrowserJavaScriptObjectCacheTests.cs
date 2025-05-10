using System.Collections.Generic;
using System.Linq;
using CefSharp.Internals;
using Xunit;

namespace CefSharp.Test.JavascriptBinding
{
    public class PerBrowserJavaScriptObjectCacheTests
    {
        private const int TestBrowserId = 1;

        [Fact]
        public void ClearCacheRemovesCacheForBrowserId()
        {
            var cache = new PerBrowserJavaScriptObjectCache();
            cache.InsertOrUpdate(TestBrowserId, new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "TestObject" }
                  });

            cache.ClearCache(TestBrowserId);

            Assert.Empty(cache.GetCacheValues(TestBrowserId));
        }

        [Fact]
        public void InsertOrUpdateAddsOrUpdatesObjectsInCache()
        {
            var cache = new PerBrowserJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "Object1" },
                      new JavascriptObject { Name = "Object2" }
                  };

            cache.InsertOrUpdate(TestBrowserId, javascriptObjects);

            var cachedObjects = cache.GetCacheValues(TestBrowserId);
            Assert.Equal(2, cachedObjects.Count);
        }

        [Fact]
        public void GetCacheValuesReturnsEmptyCollectionWhenNoCacheExists()
        {
            var cache = new PerBrowserJavaScriptObjectCache();

            var cachedObjects = cache.GetCacheValues(TestBrowserId);

            Assert.Empty(cachedObjects);
        }

        [Fact]
        public void GetCacheReturnsCacheDictionaryForBrowserId()
        {
            var cache = new PerBrowserJavaScriptObjectCache();
            var javascriptObjects = new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "Object1" }
                  };
            cache.InsertOrUpdate(TestBrowserId, javascriptObjects);

            var cachedDictionary = cache.GetCache(TestBrowserId);

            Assert.Single(cachedDictionary);
            Assert.True(cachedDictionary.ContainsKey("Object1"));
        }

        [Fact]
        public void ClearCacheDoesNotAffectOtherBrowserIds()
        {
            var cache = new PerBrowserJavaScriptObjectCache();
            cache.InsertOrUpdate(TestBrowserId, new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "Object1" }
                  });
            cache.InsertOrUpdate(2, new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "Object2" }
                  });

            cache.ClearCache(TestBrowserId);

            Assert.Empty(cache.GetCacheValues(TestBrowserId));
            Assert.NotEmpty(cache.GetCacheValues(2));
        }

        [Fact]
        public void InsertOrUpdateOverwritesExistingCacheForBrowserId()
        {
            var cache = new PerBrowserJavaScriptObjectCache();

            cache.InsertOrUpdate(TestBrowserId, new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "NewObject", Value = 1 }
                  });

            cache.InsertOrUpdate(TestBrowserId, new List<JavascriptObject>
                  {
                      new JavascriptObject { Name = "NewObject", Value = 2 }
                  });

            var cachedObjects = cache.GetCacheValues(TestBrowserId);
            Assert.Single(cachedObjects);
            Assert.Equal(2, cachedObjects.First().Value);
        }

        [Fact]
        public void GetCacheReturnsEmptyForNonExistentBrowserId()
        {
            var cache = new PerBrowserJavaScriptObjectCache();

            var cachedDictionary = cache.GetCache(999);

            Assert.Empty(cachedDictionary);
        }

        [Fact]
        public void InsertOrUpdateHandlesEmptyObjectList()
        {
            var cache = new PerBrowserJavaScriptObjectCache();

            cache.InsertOrUpdate(TestBrowserId, new List<JavascriptObject>());

            var cachedObjects = cache.GetCacheValues(TestBrowserId);
            Assert.Empty(cachedObjects);
        }
    }
}
