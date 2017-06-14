// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;

namespace CefSharp
{
    /// <summary>
    /// Class used to Represent a cookie the built in .Net Cookie
    /// class isn't used as some of it's properties have internal setters
    /// </summary>
    [DebuggerDisplay("Domain = {Domain}, Path = {Path}, Name = {Name}, Secure = {Secure}, HttpOnly = {HttpOnly}," +
                     "Creation = {Creation}, Expires = {Expires}, LastAccess = {LastAccess}", Name = "Cookie")] 
    public sealed class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime Creation { get; set; }
        public DateTime LastAccess { get; set; }
    }
}
