// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp.Fluent
{
    /// <summary>
    /// Called before a download begins.
    /// </summary>
    /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
    /// <param name="browser">The browser instance</param>
    /// <param name="downloadItem">Represents the file being downloaded.</param>
    /// <param name="callback">Callback interface used to asynchronously continue a download.</param>
    public delegate void OnBeforeDownloadDelegate(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback);

    /// <summary>
    /// Called when a download's status or progress information has been updated. This may be called multiple times before and after <see cref="OnBeforeDownload"/>.
    /// </summary>
    /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
    /// <param name="browser">The browser instance</param>
    /// <param name="downloadItem">Represents the file being downloaded.</param>
    /// <param name="callback">The callback used to Cancel/Pause/Resume the process</param>
    public delegate void OnDownloadUpdatedDelegate(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback);

    /// <summary>
    /// A <see cref="IDownloadHandler"/> implementation used by <see cref="DownloadHandlerBuilder"/>
    /// to provide a fluent means of creating a <see cref="IDownloadHandler"/>.
    /// </summary>
    public class DownloadHandler : Handler.DownloadHandler
    {
        private OnBeforeDownloadDelegate onBeforeDownload;
        private OnDownloadUpdatedDelegate onDownloadUpdated;

        /// <summary>
        /// Create a new DownloadHandler Builder
        /// </summary>
        /// <returns>Fluent DownloadHandler Builder</returns>
        public static DownloadHandlerBuilder Create()
        {
            return new DownloadHandlerBuilder();
        }

        /// <summary>
        /// Creates a new <see cref="IDownloadHandler"/> instances
        /// where all downloads are automatically downloaded to the specified folder.
        /// No dialog is dispolayed to the user.
        /// </summary>
        /// <param name="folder">folder where files are download.</param>
        /// <param name="downloadUpdated">optional delegate for download updates, track progress, completion etc.</param>
        /// <returns><see cref="IDownloadHandler"/> instance.</returns>
        public static IDownloadHandler UseFolder(string folder, OnDownloadUpdatedDelegate downloadUpdated = null)
        {
            return Create()
                .OnBeforeDownload((chromiumWebBrowser, browser, item, callback) =>
                {
                    using (callback)
                    {
                        var path = Path.Combine(folder, item.SuggestedFileName);
                        
                        callback.Continue(path, showDialog: false);
                    }
                })
                .OnDownloadUpdated(downloadUpdated)
                .Build();
        }

        /// <summary>
        /// Creates a new <see cref="IDownloadHandler"/> instances
        /// where a default "Save As" dialog is displayed to the user.
        /// </summary>
        /// <param name="downloadUpdated">optional delegate for download updates, track progress, completion etc.</param>
        /// <returns><see cref="IDownloadHandler"/> instance.</returns>
        public static IDownloadHandler AskUser(OnDownloadUpdatedDelegate downloadUpdated = null)
        {
            return Create()
                .OnBeforeDownload((chromiumWebBrowser, browser, item, callback) =>
                {
                    using (callback)
                    {
                        callback.Continue("", showDialog: true);
                    }
                })
                .OnDownloadUpdated(downloadUpdated)
                .Build();
        }

        /// <summary>
        /// Use <see cref="Create"/> to create a new instance of the fluent builder
        /// </summary>
        internal DownloadHandler()
        {

        }

        internal void SetOnBeforeDownload(OnBeforeDownloadDelegate action)
        {
            onBeforeDownload = action;
        }

        internal void SetOnDownloadUpdated(OnDownloadUpdatedDelegate action)
        {
            onDownloadUpdated = action;
        }

        /// <inheritdoc/>
        protected override void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            onBeforeDownload?.Invoke(chromiumWebBrowser, browser, downloadItem, callback);
        }

        /// <inheritdoc/>
        protected override void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            onDownloadUpdated?.Invoke(chromiumWebBrowser, browser, downloadItem, callback);
        }
    }
}
