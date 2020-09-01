// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object containing abbreviated remote object value.
    /// </summary>
    public class ObjectPreview
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public string Type
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

        /// <summary>
        /// String representation of the object.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// True iff some of the properties or entries of the original object did not fit.
        /// </summary>
        public bool Overflow
        {
            get;
            set;
        }

        /// <summary>
        /// List of the properties.
        /// </summary>
        public System.Collections.Generic.IList<PropertyPreview> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// List of the entries. Specified for `map` and `set` subtype values only.
        /// </summary>
        public System.Collections.Generic.IList<EntryPreview> Entries
        {
            get;
            set;
        }
    }
}