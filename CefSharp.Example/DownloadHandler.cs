﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example
{
    public class DownloadHandler : IDownloadHandler
    {
        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            
        }
    }
}
