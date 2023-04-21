// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Render Process JavaScript Binding (JSB) object cache
    /// Stores bound objects per CefBrowser.
    /// </summary>
    public class PerBrowserJavaScriptObjectCache : IJavaScriptObjectCache
    {
        private readonly Dictionary<int, Dictionary<string, JavascriptObject>> cache
            = new Dictionary<int, Dictionary<string, JavascriptObject>>();

        /// <inheritdoc/>
        public void ClearCache(int browserId)
        {
            cache.Remove(browserId);
        }

        /// <inheritdoc/>
        public void InsertOrUpdate(int browserId, IList<JavascriptObject> javascriptObjects)
        {
            var dict = GetCacheInternal(browserId);

            foreach (var obj in javascriptObjects)
            {
                dict[obj.Name] = obj;
            }
        }

        /// <inheritdoc/>
        public ICollection<JavascriptObject> GetCacheValues(int browserId)
        {
            if (cache.TryGetValue(browserId, out var dict))
            {
                return dict.Values;
            }

            return new List<JavascriptObject>();
        }

        /// <inheritdoc/>
        public Dictionary<string, JavascriptObject> GetCache(int browserId)
        {
            var dict = GetCacheInternal(browserId);

            return dict;
        }

        private Dictionary<string, JavascriptObject> GetCacheInternal(int browserId)
        {
            Dictionary<string, JavascriptObject> dict;

            if (!cache.TryGetValue(browserId, out dict))
            {
                dict = new Dictionary<string, JavascriptObject>();

                cache.Add(browserId, dict);
            }

            return dict;
        }
    }
}
