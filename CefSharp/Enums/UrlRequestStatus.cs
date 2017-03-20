// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


namespace CefSharp
{
    /// <summary>
    /// Flags that represent CefURLRequest status.
    /// </summary>
    public enum UrlRequestStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Request succeeded.
        /// </summary>
        Success,
        /// <summary>
        /// An IO request is pending, and the caller will be informed when it is completed.
        /// </summary>
        IoPending,
        /// <summary>
        /// Request was canceled programatically.
        /// </summary>
        Canceled,
        /// <summary>
        /// Request failed for some reason.
        /// </summary>
        Failed
    }
}
