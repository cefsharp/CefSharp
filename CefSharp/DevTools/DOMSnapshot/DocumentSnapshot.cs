// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// Document snapshot.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DocumentSnapshot : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Document URL that `Document` or `FrameOwner` node points to.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("documentURL"), IsRequired = (true))]
        public int DocumentURL
        {
            get;
            set;
        }

        /// <summary>
        /// Document title.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("title"), IsRequired = (true))]
        public int Title
        {
            get;
            set;
        }

        /// <summary>
        /// Base URL that `Document` or `FrameOwner` node uses for URL completion.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("baseURL"), IsRequired = (true))]
        public int BaseURL
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the document's content language.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentLanguage"), IsRequired = (true))]
        public int ContentLanguage
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the document's character set encoding.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("encodingName"), IsRequired = (true))]
        public int EncodingName
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType` node's publicId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("publicId"), IsRequired = (true))]
        public int PublicId
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType` node's systemId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("systemId"), IsRequired = (true))]
        public int SystemId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame ID for frame owner elements and also for the document node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frameId"), IsRequired = (true))]
        public int FrameId
        {
            get;
            set;
        }

        /// <summary>
        /// A table with dom nodes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodes"), IsRequired = (true))]
        public CefSharp.DevTools.DOMSnapshot.NodeTreeSnapshot Nodes
        {
            get;
            set;
        }

        /// <summary>
        /// The nodes in the layout tree.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("layout"), IsRequired = (true))]
        public CefSharp.DevTools.DOMSnapshot.LayoutTreeSnapshot Layout
        {
            get;
            set;
        }

        /// <summary>
        /// The post-layout inline text nodes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("textBoxes"), IsRequired = (true))]
        public CefSharp.DevTools.DOMSnapshot.TextBoxSnapshot TextBoxes
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal scroll offset.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollOffsetX"), IsRequired = (false))]
        public long? ScrollOffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical scroll offset.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollOffsetY"), IsRequired = (false))]
        public long? ScrollOffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Document content width.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentWidth"), IsRequired = (false))]
        public long? ContentWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Document content height.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentHeight"), IsRequired = (false))]
        public long? ContentHeight
        {
            get;
            set;
        }
    }
}