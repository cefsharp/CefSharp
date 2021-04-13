// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Extended WebBrowserExtensions
    /// </summary>
    public static class WebBrowserExtensionsEx
    {
        /// <summary>
        /// Retrieve the current <see cref="NavigationEntry"/>. Contains information like
        /// <see cref="NavigationEntry.HttpStatusCode"/> and <see cref="NavigationEntry.SslStatus"/>
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{NavigationEntry}"/> that when executed returns the current <see cref="NavigationEntry"/> or null
        /// </returns>
        public static Task<NavigationEntry> GetVisibleNavigationEntryAsync(this IWebBrowser browser)
        {
            var host = browser.GetBrowserHost();

            if (host == null)
            {
                return Task.FromResult<NavigationEntry>(null);
            }

            if(Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
            {
                var entry = host.GetVisibleNavigationEntry();

                return Task.FromResult<NavigationEntry>(entry);
            }

            var tcs = new TaskCompletionSource<NavigationEntry>();

            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var entry = host.GetVisibleNavigationEntry();

                tcs.TrySetResultAsync(entry);
            });

            return tcs.Task;
        }
    }
}
