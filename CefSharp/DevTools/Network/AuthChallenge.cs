// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Authorization challenge for HTTP status code 401 or 407.
    /// </summary>
    public class AuthChallenge
    {
        /// <summary>
        /// Source of the authentication challenge.
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Origin of the challenger.
        /// </summary>
        public string Origin
        {
            get;
            set;
        }

        /// <summary>
        /// The authentication scheme used, such as basic or digest
        /// </summary>
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// The realm of the challenge. May be empty.
        /// </summary>
        public string Realm
        {
            get;
            set;
        }
    }
}