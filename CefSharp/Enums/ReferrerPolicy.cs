// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public enum ReferrerPolicy
    {
        /// <summary>
        /// Always send the complete Referrer value.
        /// </summary>
        Always = 0,

        /// <summary>
        /// Use the default policy. This is OriginWhenCrossOrigin
        /// when the `--reduced-referrer-granularity` command-line flag is specified
        /// and NoReferrerWhenDowngrade otherwise.
        /// </summary>
        Default,

        /// <summary>
        /// When navigating from HTTPS to HTTP do not send the Referrer value.
        /// Otherwise, send the complete Referrer value.
        /// </summary>
        NoReferrerWhenDowngrade,

        /// <summary>
        /// Never send the Referrer value.
        /// </summary>
        Never,

        /// <summary>
        /// Only send the origin component of the Referrer value.
        /// </summary>
        Origin,

        /// <summary>
        /// When navigating cross-origin only send the origin component of the Referrer value. Otherwise, send the complete Referrer value.
        /// </summary>
        OriginWhenCrossOrigin
    }
}
