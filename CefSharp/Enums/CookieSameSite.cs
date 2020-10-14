// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Cookie same site values.
    /// </summary>
    ///<remarks>
    /// See https://source.chromium.org/chromium/chromium/src/+/master:net/cookies/cookie_constants.h
    ///</remarks>
    public enum CookieSameSite
    {
        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Cookies will be sent in all contexts, i.e sending cross-origin is allowed.
        /// None used to be the default value, but recent browser versions made Lax the default value to have reasonably robust defense against some classes of cross-site request forgery (CSRF) attacks.
        /// </summary>
        NoRestriction,
        /// <summary>
        /// Cookies are allowed to be sent with top-level navigations and will be sent along with GET request initiated by third party website. This is the default value in modern browsers.
        /// </summary>
        LaxMode,
        /// <summary>
        /// Cookies will only be sent in a first-party context and not be sent along with requests initiated by third party websites.
        /// </summary>
        StrictMode
    }
}
