// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// EntryPreview
    /// </summary>
    public class EntryPreview : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Preview of the key. Specified for map-like collection entries.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("key"), IsRequired = (false))]
        public ObjectPreview Key
        {
            get;
            set;
        }

        /// <summary>
        /// Preview of the value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public ObjectPreview Value
        {
            get;
            set;
        }
    }
}