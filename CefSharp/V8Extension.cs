// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Represents a new V8 extension to be registered.
    /// </summary>
    public sealed class V8Extension
    {
        /// <summary>
        /// Gets the name of the extension.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the javascript extension code
        /// </summary>
        public string JavascriptCode { get; private set; }

        /// <summary>
        /// Creates a new CwefExtension instance with a given name.
        /// </summary>
        /// <param name="name">Name of the CefExtension</param>
        /// <param name="javascriptCode">The javascript extension code.</param>
        public V8Extension(string name, string javascriptCode)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(javascriptCode))
            {
                throw new ArgumentNullException("javascriptCode");
            }

            Name = name;
            JavascriptCode = javascriptCode;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var ext = (V8Extension)obj;
            return Name.Equals(ext.Name);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
