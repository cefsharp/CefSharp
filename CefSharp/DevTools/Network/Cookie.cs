// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Cookie object
    /// </summary>
    public class Cookie
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
        /// Cookie expiration date as the number of seconds since the UNIX epoch.
        /// </summary>
        public long Expires
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie size.
        /// </summary>
        public int Size
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is http-only.
        /// </summary>
        public bool HttpOnly
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is secure.
        /// </summary>
        public bool Secure
        {
            get;
            set;
        }

        /// <summary>
        /// True in case of session cookie.
        /// </summary>
        public bool Session
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
        /// Cookie Priority
        /// </summary>
        public string Priority
        {
            get;
            set;
        }
    }
}