// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.Internals
{
    /// <summary>
    /// Render Process JavaScript Binding (JSB) object cache
    /// </summary>
    public interface IJavaScriptObjectCache
    {
        /// <summary>
        /// Remove the Browser specific Cache
        /// </summary>
        /// <param name="browserId">browser Id</param>
        void ClearCache(int browserId);
        /// <summary>
        /// Gets the browser specific cache (dictionary) based on it's Id
        /// </summary>
        /// <param name="browserId">browser Id</param>
        /// <returns>Dictionary of cache <see cref="JavascriptObject"/>'s.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Dictionary<string, JavascriptObject> GetCache(int browserId);
        /// <summary>
        /// Gets a collection of <see cref="JavascriptObject"/>s
        /// for the given <paramref name="browserId"/>
        /// </summary>
        /// <param name="browserId">browser Id</param>
        /// <returns>Collection of current bound objects for the browser</returns>
        /// <exception cref="InvalidOperationException"></exception>
        ICollection<JavascriptObject> GetCacheValues(int browserId);
        /// <summary>
        /// Insert or Update the <paramref name="javascriptObject"/> within the Cache
        /// </summary>
        /// <param name="browserId">browser id</param>
        /// <param name="javascriptObject">JavaScript object</param>
        /// <exception cref="InvalidOperationException"></exception>
        void InsertOrUpdate(int browserId, IList<JavascriptObject> javascriptObjects);
    }
}
