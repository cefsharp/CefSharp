// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Async extensions for different interfaces
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Deletes all cookies that matches all the provided parameters asynchronously.
        /// If both <paramref name="url"/> and <paramref name="name"/> are empty, all cookies will be deleted.
        /// </summary>
        /// <param name="cookieManager">cookie manager</param>
        /// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
        /// <param name="name">The name of the cookie. If an empty string is provided, any URL will be matched.</param>
        /// <returns>Returns -1 if a non-empty invalid URL is specified, or if cookies cannot be accessed;
        /// otherwise, a task that represents the delete operation. The value of the TResult will be the number of cookies that were deleted or -1 if unknown.</returns>
        public static Task<int> DeleteCookiesAsync(this ICookieManager cookieManager, string url = null, string name = null)
        {
            if (cookieManager == null)
            {
                throw new NullReferenceException("cookieManager");
            }

            if (cookieManager.IsDisposed)
            {
                throw new ObjectDisposedException("cookieManager");
            }

            var callback = new TaskDeleteCookiesCallback();
            if (cookieManager.DeleteCookies(url, name, callback))
            {
                return callback.Task;
            }

            //There was a problem deleting cookies
            return Task.FromResult(TaskDeleteCookiesCallback.InvalidNoOfCookiesDeleted);
        }

        /// <summary>
        /// Sets a cookie given a valid URL and explicit user-provided cookie attributes.
        /// This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// </summary>
        /// <param name="cookieManager">cookie manager</param>
        /// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
        /// <param name="cookie">the cookie to be set</param>
        /// <returns>returns false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used);
        /// otherwise task that represents the set operation. The value of the TResult parameter contains a bool to indicate success.</returns>
        public static Task<bool> SetCookieAsync(this ICookieManager cookieManager, string url, Cookie cookie)
        {
            if (cookieManager == null)
            {
                throw new NullReferenceException("cookieManager");
            }

            if (cookieManager.IsDisposed)
            {
                throw new ObjectDisposedException("cookieManager");
            }

            var callback = new TaskSetCookieCallback();
            if (cookieManager.SetCookie(url, cookie, callback))
            {
                return callback.Task;
            }

            //There was a problem setting cookies
            return Task.FromResult(false);
        }

        /// <summary>
        /// Visits all cookies. The returned cookies are sorted by longest path, then by earliest creation date.
        /// </summary>
        /// <param name="cookieManager">cookie manager</param>
        /// <returns>A task that represents the VisitAllCookies operation. The value of the TResult parameter contains a List of cookies
        /// or null if cookies cannot be accessed.</returns>
        public static Task<List<Cookie>> VisitAllCookiesAsync(this ICookieManager cookieManager)
        {
            var cookieVisitor = new TaskCookieVisitor();

            if (cookieManager.VisitAllCookies(cookieVisitor))
            {
                return cookieVisitor.Task;
            }

            return Task.FromResult<List<Cookie>>(null);
        }

        /// <summary>
        /// Visits a subset of the cookies. The results are filtered by the given url scheme, host, domain and path. 
        /// If <paramref name="includeHttpOnly"/> is true, HTTP-only cookies will also be included in the results. The returned cookies 
        /// are sorted by longest path, then by earliest creation date.
        /// </summary>
        /// <param name="cookieManager">cookie manager</param>
        /// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
        /// <param name="includeHttpOnly">A flag that determines whether HTTP-only cookies will be shown in results.</param>
        /// <returns>A task that represents the VisitUrlCookies operation. The value of the TResult parameter contains a List of cookies.
        /// or null if cookies cannot be accessed.</returns>
        public static Task<List<Cookie>> VisitUrlCookiesAsync(this ICookieManager cookieManager, string url, bool includeHttpOnly)
        {
            var cookieVisitor = new TaskCookieVisitor();

            if (cookieManager.VisitUrlCookies(url, includeHttpOnly, cookieVisitor))
            {
                return cookieVisitor.Task;
            }

            return Task.FromResult<List<Cookie>>(null);
        }

        /// <summary>
        /// Flush the backing store (if any) to disk.
        /// </summary>
        /// <param name="cookieManager">cookieManager instance</param>
        /// <returns>A task that represents the FlushStore operation. Result indicates if the flush completed successfully.
        /// Will return false if the cookikes cannot be accessed.</returns>
        public static Task<bool> FlushStoreAsync(this ICookieManager cookieManager)
        {
            var handler = new TaskCompletionCallback();

            if (cookieManager.FlushStore(handler))
            {
                return handler.Task;
            }

            //returns null if cookies cannot be accessed.
            return Task.FromResult(false);
        }

        /// <summary>
        /// Retrieve a snapshot of current navigation entries
        /// </summary>
        /// <param name="browserHost">browserHost</param>
        /// <param name="currentOnly">If true the List will only contain the current navigation entry.
        /// If false the List will include all navigation entries will be included. Default is false</param>
        public static Task<List<NavigationEntry>> GetNavigationEntriesAsync(this IBrowserHost browserHost, bool currentOnly = false)
        {
            var visitor = new TaskNavigationEntryVisitor();

            browserHost.GetNavigationEntries(visitor, currentOnly);

            return visitor.Task;
        }
    }
}
