// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used to asynchronously continue a download.
    /// </summary>
    public interface IBeforeDownloadCallback : IDisposable
    {
        /// <summary>
        /// Call to continue the download.
        /// </summary>
        /// <param name="downloadPath">full file path for the download including the file name
        /// or leave blank to use the suggested name and the default temp directory</param>
        /// <param name="showDialog">Set to true if you do wish to show the default "Save As" dialog</param>
        void Continue(string downloadPath, bool showDialog);

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
