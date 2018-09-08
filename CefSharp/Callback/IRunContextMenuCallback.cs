// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for continuation of custom context menu display.
    /// </summary>
    public interface IRunContextMenuCallback : IDisposable
    {
        /// <summary>
        /// Complete context menu display by selecting the specified commandId and eventFlags;
        /// </summary>
        /// <param name="commandId">the command Id</param>
        /// <param name="eventFlags">the event flags</param>
        void Continue(CefMenuCommand commandId, CefEventFlags eventFlags);

        /// <summary>
        /// Cancel context menu display.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Gets a value indicating whether the callback has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
