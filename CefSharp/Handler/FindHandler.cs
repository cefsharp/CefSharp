// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Structs;

namespace CefSharp.Handler
{
    /// <summary>
    /// Implement this interface to handle events related to find results.
    /// The methods of this class will be called on the CEF UI thread.
    /// </summary>
    public class FindHandler : IFindHandler
    {
        /// <summary>
        /// Called to report find results returned by <see cref="IBrowserHost.Find"/>
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="identifier">is the identifier passed to Find()</param>
        /// <param name="count">is the number of matches currently identified</param>
        /// <param name="selectionRect">is the location of where the match was found (in window coordinates)</param>
        /// <param name="activeMatchOrdinal">is the current position in the search results</param>
        /// <param name="finalUpdate">is true if this is the last find notification.</param>
        void IFindHandler.OnFindResult(IWebBrowser chromiumWebBrowser, IBrowser browser, int identifier, int count, Rect selectionRect, int activeMatchOrdinal, bool finalUpdate)
        {
            OnFindResult(chromiumWebBrowser, browser, identifier, count, selectionRect, activeMatchOrdinal, finalUpdate);
        }

        /// <summary>
        /// Called to report find results returned by <see cref="IBrowserHost.Find"/>
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="identifier">is the identifier passed to Find()</param>
        /// <param name="count">is the number of matches currently identified</param>
        /// <param name="selectionRect">is the location of where the match was found (in window coordinates)</param>
        /// <param name="activeMatchOrdinal">is the current position in the search results</param>
        /// <param name="finalUpdate">is true if this is the last find notification.</param>
        protected virtual void OnFindResult(IWebBrowser chromiumWebBrowser, IBrowser browser, int identifier, int count, Rect selectionRect, int activeMatchOrdinal, bool finalUpdate)
        {

        }
    }
}
