// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Class used to Represent a cookie the built in .Net Cookie
    /// class isn't used as some of it's properties have internal setters
    /// </summary>
    public class Cookie
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
