// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Enums
{
    /// <summary>
    /// Configuration options for registering a custom scheme.
    /// These values are used when calling AddCustomScheme.
    /// </summary>
    [Flags]
    public enum SchemeOptions
    {
        /// <summary>
        /// Register scheme without options set
        /// </summary>
        None = 0,

        /// <summary>
        /// If Standard is set the scheme will be treated as a
        /// standard scheme. Standard schemes are subject to URL canonicalization and
        /// parsing rules as defined in the Common Internet Scheme Syntax RFC 1738
        /// Section 3.1 available at http://www.ietf.org/rfc/rfc1738.txt
        ///
        /// In particular, the syntax for standard scheme URLs must be of the form:
        /// <pre>
        ///  [scheme]://[username]:[password]@[host]:[port]/[url-path]
        /// </pre> Standard scheme URLs must have a host component that is a fully
        /// qualified domain name as defined in Section 3.5 of RFC 1034 [13] and
        /// Section 2.1 of RFC 1123. These URLs will be canonicalized to
        /// "scheme://host/path" in the simplest case and
        /// "scheme://username:password@host:port/path" in the most explicit case. For
        /// example, "scheme:host/path" and "scheme:///host/path" will both be
        /// canonicalized to "scheme://host/path". The origin of a standard scheme URL
        /// is the combination of scheme, host and port (i.e., "scheme://host:port" in
        /// the most explicit case).
        ///
        /// For non-standard scheme URLs only the "scheme:" component is parsed and
        /// canonicalized. The remainder of the URL will be passed to the handler as-
        /// is. For example, "scheme:///some%20text" will remain the same. Non-standard
        /// scheme URLs cannot be used as a target for form submission.
        /// </summary>
        Standard = 1 << 0,

        /// <summary>
        /// If Local is set the scheme will be treated with the same
        /// security rules as those applied to "file" URLs. Normal pages cannot link to
        /// or access local URLs. Also, by default, local URLs can only perform
        /// XMLHttpRequest calls to the same URL (origin + path) that originated the
        /// request. To allow XMLHttpRequest calls from a local URL to other URLs with
        /// the same origin set the CefSettings.FileAccessFromFileUrlsAllowed
        /// value to true. To allow XMLHttpRequest calls from a local URL to all
        /// origins set the CefSettings.UniversalAccessFromFileUrlsAllowed value
        /// to true.
        /// </summary>
        Local = 1 << 1,

        /// <summary>
        /// If DisplayIsolated is set the scheme can only be
        /// displayed from other content hosted with the same scheme. For example,
        /// pages in other origins cannot create iframes or hyperlinks to URLs with the
        /// scheme. For schemes that must be accessible from other schemes don't set
        /// this, set CorsEnabled, and use CORS "Access-Control-Allow-Origin" headers
        /// to further restrict access.
        /// </summary>
        DisplayIsolated = 1 << 2,

        /// <summary>
        /// If Secure is set the scheme will be treated with the same
        /// security rules as those applied to "https" URLs. For example, loading this
        /// scheme from other secure schemes will not trigger mixed content warnings.
        /// </summary>
        Secure = 1 << 3,

        /// <summary>
        /// If CorsEnabled is set the scheme can be sent CORS requests.
        /// This value should be set in most cases where Standard is set.
        /// </summary>
        CorsEnabled = 1 << 4,

        /// <summary>
        /// If CspBypassing is set the scheme can bypass Content-Security-Policy (CSP) checks.
        /// This value should not be set in most cases where Standard is set.
        /// </summary>
        CspBypassing = 1 << 5,

        /// <summary>
        /// If FetchEnabled is set the scheme can perform Fetch API requests.
        /// </summary>
        FetchEnabled = 1 << 6,
    }
}
