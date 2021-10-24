// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Policy for how the Referrer HTTP header value will be sent during navigation.
    /// If the `--no-referrers` command-line flag is specified then the policy value
    /// will be ignored and the Referrer value will never be sent.
    /// Must be kept synchronized with net::URLRequest::ReferrerPolicy from Chromium.
    /// </summary>
    public enum ReferrerPolicy
    {
        /// <summary>
        /// Clear the referrer header if the header value is HTTPS but the request
        /// destination is HTTP. This is the default behavior.
        /// </summary>
        ClearReferrerOnTransitionFromSecureToInsecure,

        /// <summary>
        /// Default which is equivalent to <see cref="ClearReferrerOnTransitionFromSecureToInsecure"/>
        /// </summary>
        Default = ClearReferrerOnTransitionFromSecureToInsecure,

        /// <summary>
        /// A slight variant on <see cref="ClearReferrerOnTransitionFromSecureToInsecure"/>:
        /// If the request destination is HTTP, an HTTPS referrer will be cleared. If
        /// the request's destination is cross-origin with the referrer (but does not
        /// downgrade), the referrer's granularity will be stripped down to an origin
        /// rather than a full URL. Same-origin requests will send the full referrer.
        /// </summary>
        ReduceReferrerGranularityOnTransitionCrossOrigin,

        /// <summary>
        /// Strip the referrer down to an origin when the origin of the referrer is
        /// different from the destination's origin.
        /// </summary>
        OriginOnlyOnTransitionCrossOrigin,

        /// <summary>
        /// Never change the referrer.
        /// </summary>
        NeverClearReferrer,

        /// <summary>
        /// Strip the referrer down to the origin regardless of the redirect location.
        /// </summary>
        Origin,

        /// <summary>
        /// Clear the referrer when the request's referrer is cross-origin with the
        /// request's destination.
        /// </summary>
        ClearReferrerOnTransitionCrossOrigin,

        /// <summary>
        /// Strip the referrer down to the origin, but clear it entirely if the
        /// referrer value is HTTPS and the destination is HTTP.
        /// </summary>
        OriginClearOnTransitionFromSecureToInsecure,

        /// <summary>
        /// Always clear the referrer regardless of the request destination.
        /// </summary>
        NoReferrer,

        /// <summary>
        /// Always the last value in this enumeration.
        /// </summary>
        LastValue = NoReferrer,
    }
}
