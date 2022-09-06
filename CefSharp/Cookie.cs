// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Diagnostics;
using CefSharp.Enums;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Class used to Represent a cookie.
    /// The built in .Net Cookie class isn't used as some of it's properties have
    /// internal setters
    /// </summary>
    [DebuggerDisplay("Domain = {Domain}, Path = {Path}, Name = {Name}, Secure = {Secure}, HttpOnly = {HttpOnly}," +
                     "Creation = {Creation}, Expires = {Expires}, LastAccess = {LastAccess}", Name = "Cookie")]
    public sealed class Cookie
    {
        /// <summary>
        /// The cookie name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The cookie value. 
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// If domain is empty a host cookie will be created instead of a domain cookie. Domain cookies are stored with a leading "."
        /// and are visible to sub-domains whereas host cookies are not. 
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Ss non-empty only URLs at or below the path will get the cookie value. 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// If true the cookie will only be sent for HTTPS requests. 
        /// </summary>
        public bool Secure { get; set; }
        /// <summary>
        /// Ss true the cookie will only be sent for HTTP requests. 
        /// </summary>
        public bool HttpOnly { get; set; }
        /// <summary>
        /// Expires or null if no expiry
        /// </summary>
        public DateTime? Expires { get; set; }
        /// <summary>
        /// The cookie creation date. This is automatically populated by the system on cookie creation. 
        /// </summary>
        public DateTime Creation { get; private set; }
        /// <summary>
        /// The cookie last access date. This is automatically populated by the system on access. 
        /// </summary>		
        public DateTime LastAccess { get; private set; }
        /// <summary>
        /// Same site.
        /// </summary>
        public CookieSameSite SameSite { get; set; }
        /// <summary>
        /// Priority
        /// </summary>
        public CookiePriority Priority { get; set; }

        /// <summary>
        /// Used internally to set <see cref="Creation"/>.
        /// <see cref="Creation"/> can only be set when fecting a Cookie from Chromium
        /// </summary>
        /// <param name="baseTime">
        /// Represents a wall clock time in UTC. Values are not guaranteed to be monotonically
        /// non-decreasing and are subject to large amounts of skew. Time is stored internally
        /// as microseconds since the Windows epoch (1601).
        /// </param>
        /// <remarks>
        /// Hidden from intellisense as only meant to be used internally, unfortunately
        /// VC++ makes it hard to use internal classes from C#
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetCreationDate(long baseTime)
        {
            Creation = CefTimeUtils.FromBaseTimeToDateTime(baseTime);
        }

        /// <summary>
        /// Used internally to set <see cref="LastAccess"/>.
        /// <see cref="LastAccess"/> can only be set when fecting a Cookie from Chromium
        /// </summary>
        /// <param name="baseTime">
        /// Represents a wall clock time in UTC. Values are not guaranteed to be monotonically
        /// non-decreasing and are subject to large amounts of skew. Time is stored internally
        /// as microseconds since the Windows epoch (1601).
        /// </param>
        /// <remarks>
        /// Hidden from intellisense as only meant to be used internally, unfortunately
        /// VC++ makes it hard to use internal classes from C#
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetLastAccessDate(long baseTime)
        {
            LastAccess = CefTimeUtils.FromBaseTimeToDateTime(baseTime);
        }
    }
}
