// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS media rule descriptor.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CSSMedia : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Media query text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (true))]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Source of the media query: "mediaRule" if specified by a @media rule, "importRule" if
        /// specified by an @import rule, "linkedSheet" if specified by a "media" attribute in a linked
        /// stylesheet's LINK tag, "inlineSheet" if specified by a "media" attribute in an inline
        /// stylesheet's STYLE tag.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("source"), IsRequired = (true))]
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the document containing the media query description.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sourceURL"), IsRequired = (false))]
        public string SourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// The associated rule (@media or @import) header range in the enclosing stylesheet (if
        /// available).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("range"), IsRequired = (false))]
        public CefSharp.DevTools.CSS.SourceRange Range
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the stylesheet containing this object (if exists).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleSheetId"), IsRequired = (false))]
        public string StyleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// Array of media queries.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mediaList"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.MediaQuery> MediaList
        {
            get;
            set;
        }
    }
}