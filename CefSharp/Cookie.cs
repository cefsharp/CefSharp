// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using CefSharp.Enums;

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
        public DateTime Creation { get; set; }
        /// <summary>
        /// The cookie last access date. This is automatically populated by the system on access. 
        /// </summary>		
        public DateTime LastAccess { get; set; }
        /// <summary>
        /// Same site.
        /// </summary>
        public CookieSameSite SameSite { get; set; }
        /// <summary>
        /// Priority
        /// </summary>
        public CookiePriority Priority { get; set; }
    }
}
