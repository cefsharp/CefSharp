// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS stylesheet metainformation.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CSSStyleSheetHeader : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The stylesheet identifier.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleSheetId"), IsRequired = (true))]
        public string StyleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// Owner frame identifier.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frameId"), IsRequired = (true))]
        public string FrameId
        {
            get;
            set;
        }

        /// <summary>
        /// Stylesheet resource URL.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sourceURL"), IsRequired = (true))]
        public string SourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// URL of source map associated with the stylesheet (if any).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sourceMapURL"), IsRequired = (false))]
        public string SourceMapURL
        {
            get;
            set;
        }

        /// <summary>
        /// Stylesheet origin.
        /// </summary>
        public CefSharp.DevTools.CSS.StyleSheetOrigin Origin
        {
            get
            {
                return (CefSharp.DevTools.CSS.StyleSheetOrigin)(StringToEnum(typeof(CefSharp.DevTools.CSS.StyleSheetOrigin), origin));
            }

            set
            {
                origin = (EnumToString(value));
            }
        }

        /// <summary>
        /// Stylesheet origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("origin"), IsRequired = (true))]
        internal string origin
        {
            get;
            set;
        }

        /// <summary>
        /// Stylesheet title.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("title"), IsRequired = (true))]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The backend id for the owner node of the stylesheet.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ownerNode"), IsRequired = (false))]
        public int? OwnerNode
        {
            get;
            set;
        }

        /// <summary>
        /// Denotes whether the stylesheet is disabled.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("disabled"), IsRequired = (true))]
        public bool Disabled
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the sourceURL field value comes from the sourceURL comment.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("hasSourceURL"), IsRequired = (false))]
        public bool? HasSourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this stylesheet is created for STYLE tag by parser. This flag is not set for
        /// document.written STYLE tags.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isInline"), IsRequired = (true))]
        public bool IsInline
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this stylesheet is mutable. Inline stylesheets become mutable
        /// after they have been modified via CSSOM API.
        /// <link> element's stylesheets are never mutable. Constructed stylesheets
        /// (new CSSStyleSheet()) are mutable immediately after creation.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isMutable"), IsRequired = (true))]
        public bool IsMutable
        {
            get;
            set;
        }

        /// <summary>
        /// Line offset of the stylesheet within the resource (zero based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startLine"), IsRequired = (true))]
        public long StartLine
        {
            get;
            set;
        }

        /// <summary>
        /// Column offset of the stylesheet within the resource (zero based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("startColumn"), IsRequired = (true))]
        public long StartColumn
        {
            get;
            set;
        }

        /// <summary>
        /// Size of the content (in characters).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("length"), IsRequired = (true))]
        public long Length
        {
            get;
            set;
        }

        /// <summary>
        /// Line offset of the end of the stylesheet within the resource (zero based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endLine"), IsRequired = (true))]
        public long EndLine
        {
            get;
            set;
        }

        /// <summary>
        /// Column offset of the end of the stylesheet within the resource (zero based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endColumn"), IsRequired = (true))]
        public long EndColumn
        {
            get;
            set;
        }
    }
}