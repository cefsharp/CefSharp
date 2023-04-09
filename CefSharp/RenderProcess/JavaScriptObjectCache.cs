// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using CefSharp.Internals;

namespace CefSharp.RenderProcess
{
    /// <summary>
    /// JavaScriptObjectCache is used in the RenderProcess to cache
    /// JavaScript bound objects at a CefBrowser level
    /// </summary>
    public class JavaScriptObjectCache 
    {
        private readonly Dictionary<int, Dictionary<string, JavascriptObject>> cache
            = new Dictionary<int, Dictionary<string, JavascriptObject>>();

        /// <summary>
        /// Remove the Browser specific Cache
        /// </summary>
        /// <param name="browserId">browser Id</param>
        public void ClearCache(int browserId)
        {
            cache.Remove(browserId);
        }

        /// <summary>
        /// Insert or Update the <paramref name="javascriptObject"/> within the Cache
        /// </summary>
        /// <param name="browserId">browser id</param>
        /// <param name="javascriptObject">JavaScript object</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void InsertOrUpdate(int browserId,  IList<JavascriptObject> javascriptObjects)
        {
            var dict = GetCacheInternal(browserId);

            foreach (var obj in javascriptObjects)
            {
                if (dict.ContainsKey(obj.Name))
                {
                    dict.Remove(obj.Name);
                }

                dict.Add(obj.Name, obj);
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="JavascriptObject"/>s
        /// for the given <paramref name="browserId"/>
        /// </summary>
        /// <param name="browserId">browser Id</param>
        /// <returns>Collection of current bound objects for the browser</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ICollection<JavascriptObject> GetCacheValues(int browserId)
        {
            if (cache.TryGetValue(browserId, out var dict))
            {
                return dict.Values;
            }

            return new List<JavascriptObject>();
        }

        /// <summary>
        /// Gets the browser specific cache (dictionary) based on it's Id
        /// </summary>
        /// <param name="browserId">browser Id</param>
        /// <returns>Dictionary of cache <see cref="JavascriptObject"/>'s.</returns>
        /// <exception cref="InvalidOperationException"></exception>
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
