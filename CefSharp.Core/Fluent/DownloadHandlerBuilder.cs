// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Fluent
{
    /// <summary>
    /// Fluent DownloadHandler Builder
    /// </summary>
    public class DownloadHandlerBuilder
    {
        private DownloadHandler handler = new DownloadHandler();

        /// <summary>
        /// See <see cref="IDownloadHandler.OnBeforeDownload(IWebBrowser, IBrowser, DownloadItem, IBeforeDownloadCallback)"/> for details.
        /// </summary>
        /// <param name="action">Action to be executed when <see cref="IDownloadHandler.OnBeforeDownload(IWebBrowser, IBrowser, DownloadItem, IBeforeDownloadCallback)"/>
        /// is called</param>
        /// <returns>
        /// Fluent Builder, call <see cref="DownloadHandlerBuilder.Build"/> to create
        /// a new <see cref="IDownloadHandler"/> instance
        /// </returns>
        public DownloadHandlerBuilder OnBeforeDownload(Action<IWebBrowser, IBrowser, DownloadItem, IBeforeDownloadCallback> action)
        {
            handler.SetOnBeforeDownload(action);

            return this;
        }

        /// <summary>
        /// See <see cref="IDownloadHandler.OnDownloadUpdated(IWebBrowser, IBrowser, DownloadItem, IDownloadItemCallback)"/> for details.
        /// </summary>
        /// <param name="action">Action to be executed when <see cref="IDownloadHandler.OnDownloadUpdated(IWebBrowser, IBrowser, DownloadItem, IDownloadItemCallback)"/>
        /// is called</param>
        /// <returns>
        /// Fluent Builder, call <see cref="DownloadHandlerBuilder.Build"/> to create
        /// a new <see cref="IDownloadHandler"/> instance
        /// </returns>
        public DownloadHandlerBuilder OnDownloadUpdated(Action<IWebBrowser, IBrowser, DownloadItem, IDownloadItemCallback> action)
        {
            handler.SetOnDownloadUpdated(action);

            return this;
        }

        /// <summary>
        /// Create a <see cref="IDownloadHandler"/> instance
        /// </summary>
        /// <returns> a <see cref="IDownloadHandler"/> instance</returns>
        public IDownloadHandler Build()
        {
            return handler;
        }
    }
}
