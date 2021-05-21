// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Fluent
{
    public class DownloadHandler : Handler.DownloadHandler
    {
        public Action<IWebBrowser, IBrowser, DownloadItem, IBeforeDownloadCallback> onBeforeDownload;
        public Action<IWebBrowser, IBrowser, DownloadItem, IDownloadItemCallback> onDownloadUpdated;

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
        /// <returns><see cref="IDownloadHandler"/> instance.</returns>
        public static IDownloadHandler UseFolder(string folder)
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
                .Build();
        }

        /// <summary>
        /// Creates a new <see cref="IDownloadHandler"/> instances
        /// where a default "Save As" dialog is displayed to the user.
        /// </summary>
        /// <returns><see cref="IDownloadHandler"/> instance.</returns>
        public static IDownloadHandler AskUser()
        {
            return Create()
                .OnBeforeDownload((chromiumWebBrowser, browser, item, callback) =>
                {
                    using (callback)
                    {
                        callback.Continue("", showDialog: true);
                    }
                })
                .Build();
        }

        /// <summary>
        /// Use <see cref="Create"/> to create a new instance of the fluent builder
        /// </summary>
        internal DownloadHandler()
        {

        }

        public void SetOnBeforeDownload(Action<IWebBrowser, IBrowser, DownloadItem, IBeforeDownloadCallback> action)
        {
            onBeforeDownload = action;
        }

        public void SetOnDownloadUpdated(Action<IWebBrowser, IBrowser, DownloadItem, IDownloadItemCallback> action)
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
