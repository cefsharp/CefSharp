// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// Table of details of the post layout rendered text positions. The exact layout should not be regarded as
    /// stable and may change between versions.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class TextBoxSnapshot : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Index of the layout tree node that owns this box collection.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("layoutIndex"), IsRequired = (true))]
        public int[] LayoutIndex
        {
            get;
            set;
        }

        /// <summary>
        /// The absolute position bounding box.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("bounds"), IsRequired = (true))]
        public long[] Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// The starting index in characters, for this post layout textbox substring. Characters that
        /// would be represented as a surrogate pair in UTF-16 have length 2.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("start"), IsRequired = (true))]
        public int[] Start
        {
            get;
            set;
        }

        /// <summary>
        /// The number of characters in this post layout textbox substring. Characters that would be
        /// represented as a surrogate pair in UTF-16 have length 2.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("length"), IsRequired = (true))]
        public int[] Length
        {
            get;
            set;
        }
    }
}