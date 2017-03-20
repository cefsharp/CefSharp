// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Supported SSL version values. See net/ssl/ssl_connection_status_flags.h
    /// for more information.
    /// </summary>
    public enum SslVersion
    {
        Unknown = 0,  // Unknown SSL version.
        Ssl2 = 1,
        Ssl3 = 2,
        Tls1 = 3,
        Tls1_1 = 4,
        Tls1_2 = 5,
        // Reserve 6 for TLS 1.3.
        Quic = 7,
    }
}
