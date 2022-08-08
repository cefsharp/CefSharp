// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Media access permissions used by <see cref="IPermissionHandler.OnRequestMediaAccessPermission(IWebBrowser, IBrowser, IFrame, string, MediaAccessPermissionType, IMediaAccessCallback)"/>.
    /// </summary>
    [Flags]
    public enum MediaAccessPermissionType : uint
    {
        /// <summary>
        ///  No permission.
        /// </summary>
        None = 0,

        /// <summary>
        /// Device audio capture permission.
        /// </summary>
        AudioCapture = 1 << 0,

        /// <summary>
        /// Device video capture permission.
        /// </summary>
        VideoCapture = 1 << 1,

        /// <summary>
        /// Desktop audio capture permission.
        /// </summary>
        DesktopAudioCapture = 1 << 2,

        /// <summary>
        /// Desktop video capture permission.
        /// </summary>
        DesktopVideoCapture = 1 << 3,
    }
}
