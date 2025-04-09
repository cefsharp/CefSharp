// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Render Process JavaScript Binding (JSB) object cache
    /// Legacy Behaviour, objects are cache per process.
    /// </summary>
    public class LegacyJavaScriptObjectCache : IJavaScriptObjectCache
    {
        private readonly Dictionary<string, JavascriptObject> cache
            = new Dictionary<string, JavascriptObject>();

        /// <inheritdoc/>
        public void ClearCache(int browserId)
        {
            // NO OP
        }

        /// <inheritdoc/>
        public void InsertOrUpdate(int browserId, IList<JavascriptObject> javascriptObjects)
        {
            foreach (var obj in javascriptObjects)
            {
                cache[obj.Name] = obj;
            }
        }

        /// <inheritdoc/>
        public ICollection<JavascriptObject> GetCacheValues(int browserId)
        {
            return cache.Values;
        }

        /// <inheritdoc/>
        public Dictionary<string, JavascriptObject> GetCache(int browserId)
        {
            return cache;
        }
    }
}
