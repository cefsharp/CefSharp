// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// EntryPreview
    /// </summary>
    public class EntryPreview
    {
        /// <summary>
        /// Preview of the key. Specified for map-like collection entries.
        /// </summary>
        public ObjectPreview Key
        {
            get;
            set;
        }

        /// <summary>
        /// Preview of the value.
        /// </summary>
        public ObjectPreview Value
        {
            get;
            set;
        }
    }
}