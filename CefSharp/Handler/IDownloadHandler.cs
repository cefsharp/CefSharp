// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Class used to handle file downloads.
    /// The methods of this class will called on the CEF UI thread. 
    /// </summary>
    public interface IDownloadHandler
    {
        /// <summary>
        /// Called before a download begins.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance</param>
        /// <param name="downloadItem">Represents the file being downloaded.</param>
        /// <param name="callback">Callback interface used to asynchronously continue a download.</param>
        void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback);

        /// <summary>
        /// Called when a download's status or progress information has been updated. This may be called multiple times before and after <see cref="OnBeforeDownload"/>.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance</param>
        /// <param name="downloadItem">Represents the file being downloaded.</param>
        /// <param name="callback">The callback used to Cancel/Pause/Resume the process</param>
        void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback);
    }
}
