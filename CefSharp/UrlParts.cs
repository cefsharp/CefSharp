// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class UrlParts
    {
        /// <summary>
        /// The complete URL specification.
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// Scheme component not including the colon (e.g., "http").
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// User name component.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password component.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Host component. This may be a hostname, an IPv4 address or an IPv6 literal
        /// surrounded by square brackets (e.g., "[2001:db8::1]").
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port number component.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Origin contains just the scheme, host, and port from a URL. Equivalent to
        /// clearing any username and password, replacing the path with a slash, and
        /// clearing everything after that. This value will be empty for non-standard
        /// URLs.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Path component including the first slash following the host.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Query string component (i.e., everything following the '?').
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Fragment (hash) identifier component (i.e., the string following the '#').
        /// </summary>
        public string Fragment { get; set; }
    }
}
