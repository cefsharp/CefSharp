// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Supported SSL content status flags. See content/public/common/ssl_status.h
    /// for more information.
    /// </summary>
    public enum SslContentStatus
    {
        /// <summary>
        /// HTTP page, or HTTPS page with no insecure content..
        /// </summary>
        NormalContent = 0,
        /// <summary>
        /// HTTPS page containing "displayed" HTTP resources (e.g. images, CSS).
        /// </summary>
        DisplayedInsecureContent = 1 << 0,
        /// <summary>
        /// HTTPS page containing "executed" HTTP resources (i.e. script)
        /// </summary>
        RanInsecureContent = 1 << 1,
    }
}
