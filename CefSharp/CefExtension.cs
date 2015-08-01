// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Represents a new V8 extension to be registered.
    /// </summary>
    public sealed class CefExtension
    {
        /// <summary>
        /// Gets the name of the extension.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the javascript extension code
        /// </summary>
        public string JavascriptCode { get; set; }

        /// <summary>
        /// Creates a new CwefExtension instance with a given name.
        /// </summary>
        /// <param name="name">Name of the CefExtension</param>
        public CefExtension(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var ext = (CefExtension)obj;
            return Name.Equals(ext.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        } 
    }
}
