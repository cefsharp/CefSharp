// Copyright © 2016 The CefSharp Authors. All rights reserved.
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
        /// <summary>
        /// Unknown SSL version.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// An enum constant representing the ssl 2 option.
        /// </summary>
        Ssl2 = 1,
        /// <summary>
        /// An enum constant representing the ssl 3 option.
        /// </summary>
        Ssl3 = 2,
        /// <summary>
        /// An enum constant representing the TLS 1.0 option.
        /// </summary>
        Tls1 = 3,
        /// <summary>
        /// An enum constant representing the TLS 1.1 option.
        /// </summary>
        Tls1_1 = 4,
        /// <summary>
        /// An enum constant representing the TLS 1.2 option.
        /// </summary>
        Tls1_2 = 5,
        /// <summary>
        /// An enum constant representing the TLS 1.3 option.
        /// </summary>
        Tls1_3 = 6,
        /// <summary>
        /// An enum constant representing the QUIC option.
        /// </summary>
        Quic = 7,
    }
}
