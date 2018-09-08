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
        NormalContent = 0,
        DisplayedInsecureContent = 1 << 0,
        RanInsecureContent = 1 << 1,
    }
}
