// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// PropertyPreview
    /// </summary>
    public class PropertyPreview
    {
        /// <summary>
        /// Property name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Object type. Accessor means that the property itself is an accessor property.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// User-friendly property value string.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Nested value preview.
        /// </summary>
        public ObjectPreview ValuePreview
        {
            get;
            set;
        }

        /// <summary>
        /// Object subtype hint. Specified for `object` type values only.
        /// </summary>
        public string Subtype
        {
            get;
            set;
        }
    }
}