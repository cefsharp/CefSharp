﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IDownloadHandler
    {
        /// <summary>
        /// Called before a download begins.
        /// </summary>
        /// <param name="browser">The browser instance</param>
        /// <param name="downloadItem">Represents the file being downloaded.</param>
        /// <param name="callback">Callback interface used to asynchronously continue a download.</param>
        void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback);

        /// <summary>
        /// Called when a download's status or progress information has been updated. This may be called multiple times before and after <see cref="OnBeforeDownload"/>.
        /// </summary>
        /// <param name="browser">The browser instance</param>
        /// <param name="downloadItem">Represents the file being downloaded.</param>
        /// <param name="callback">The callback used to Cancel/Pause/Resume the process</param>
        void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback);
    }
}
