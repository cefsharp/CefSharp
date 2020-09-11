// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// Table of details of an element in the DOM tree with a LayoutObject.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class LayoutTreeSnapshot : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Index of the corresponding node in the `NodeTreeSnapshot` array returned by `captureSnapshot`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeIndex"), IsRequired = (true))]
        public int[] NodeIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Array of indexes specifying computed style strings, filtered according to the `computedStyles` parameter passed to `captureSnapshot`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styles"), IsRequired = (true))]
        public int[] Styles
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
        /// Contents of the LayoutText, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (true))]
        public int[] Text
        {
            get;
            set;
        }

        /// <summary>
        /// Stacking context information.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("stackingContexts"), IsRequired = (true))]
        public CefSharp.DevTools.DOMSnapshot.RareBooleanData StackingContexts
        {
            get;
            set;
        }

        /// <summary>
        /// Global paint order index, which is determined by the stacking order of the nodes. Nodes
        /// that are painted together will have the same index. Only provided if includePaintOrder in
        /// captureSnapshot was true.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("paintOrders"), IsRequired = (false))]
        public int[] PaintOrders
        {
            get;
            set;
        }

        /// <summary>
        /// The offset rect of nodes. Only available when includeDOMRects is set to true
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offsetRects"), IsRequired = (false))]
        public long[] OffsetRects
        {
            get;
            set;
        }

        /// <summary>
        /// The scroll rect of nodes. Only available when includeDOMRects is set to true
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollRects"), IsRequired = (false))]
        public long[] ScrollRects
        {
            get;
            set;
        }

        /// <summary>
        /// The client rect of nodes. Only available when includeDOMRects is set to true
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("clientRects"), IsRequired = (false))]
        public long[] ClientRects
        {
            get;
            set;
        }
    }
}