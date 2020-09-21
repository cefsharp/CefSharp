// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// Details of an element in the DOM tree with a LayoutObject.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class LayoutTreeNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The index of the related DOM node in the `domNodes` array returned by `getSnapshot`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("domNodeIndex"), IsRequired = (true))]
        public int DomNodeIndex
        {
            get;
            set;
        }

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
        /// Contents of the LayoutText, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("layoutText"), IsRequired = (false))]
        public string LayoutText
        {
            get;
            set;
        }

        /// <summary>
        /// The post-layout inline text nodes, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("inlineTextNodes"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOMSnapshot.InlineTextBox> InlineTextNodes
        {
            get;
            set;
        }

        /// <summary>
        /// Index into the `computedStyles` array returned by `getSnapshot`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleIndex"), IsRequired = (false))]
        public int? StyleIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Global paint order index, which is determined by the stacking order of the nodes. Nodes
        /// that are painted together will have the same index. Only provided if includePaintOrder in
        /// getSnapshot was true.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("paintOrder"), IsRequired = (false))]
        public int? PaintOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate the element begins a new stacking context.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isStackingContext"), IsRequired = (false))]
        public bool? IsStackingContext
        {
            get;
            set;
        }
    }
}