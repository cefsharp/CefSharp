// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Cookie parameter object
    /// </summary>
    public class CookieParam
    {
        /// <summary>
        /// Cookie name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// The request-URI to associate with the setting of the cookie. This value can affect the
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie domain.
        /// </summary>
        public string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie path.
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is secure.
        /// </summary>
        public bool? Secure
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is http-only.
        /// </summary>
        public bool? HttpOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie SameSite type.
        /// </summary>
        public string SameSite
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie expiration date, session cookie if not set
        /// </summary>
        public long? Expires
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie Priority.
        /// </summary>
        public string Priority
        {
            get;
            set;
        }
    }
}