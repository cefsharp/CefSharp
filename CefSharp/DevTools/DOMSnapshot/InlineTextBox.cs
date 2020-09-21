// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// Details of post layout rendered text positions. The exact layout should not be regarded as
    /// stable and may change between versions.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class InlineTextBox : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The bounding box in document coordinates. Note that scroll offset of the document is ignored.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("boundingBox"), IsRequired = (true))]
        public CefSharp.DevTools.DOM.Rect BoundingBox
        {
            get;
            set;
        }

        /// <summary>
        /// The starting index in characters, for this post layout textbox substring. Characters that
        /// would be represented as a surrogate pair in UTF-16 have length 2.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startCharacterIndex"), IsRequired = (true))]
        public int StartCharacterIndex
        {
            get;
            set;
        }

        /// <summary>
        /// The number of characters in this post layout textbox substring. Characters that would be
        /// represented as a surrogate pair in UTF-16 have length 2.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("numCharacters"), IsRequired = (true))]
        public int NumCharacters
        {
            get;
            set;
        }
    }
}