// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    /// <summary>
    /// Information about a compositing layer.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Layer : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The unique id for this layer.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("layerId"), IsRequired = (true))]
        public string LayerId
        {
            get;
            set;
        }

        /// <summary>
        /// The id of parent (not present for root).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parentLayerId"), IsRequired = (false))]
        public string ParentLayerId
        {
            get;
            set;
        }

        /// <summary>
        /// The backend id for the node associated with this layer.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (false))]
        public int? BackendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Offset from parent layer, X coordinate.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offsetX"), IsRequired = (true))]
        public long OffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Offset from parent layer, Y coordinate.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offsetY"), IsRequired = (true))]
        public long OffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Layer width.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("width"), IsRequired = (true))]
        public long Width
        {
            get;
            set;
        }

        /// <summary>
        /// Layer height.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("height"), IsRequired = (true))]
        public long Height
        {
            get;
            set;
        }

        /// <summary>
        /// Transformation matrix for layer, default is identity matrix
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("transform"), IsRequired = (false))]
        public long[] Transform
        {
            get;
            set;
        }

        /// <summary>
        /// Transform anchor point X, absent if no transform specified
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("anchorX"), IsRequired = (false))]
        public long? AnchorX
        {
            get;
            set;
        }

        /// <summary>
        /// Transform anchor point Y, absent if no transform specified
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("anchorY"), IsRequired = (false))]
        public long? AnchorY
        {
            get;
            set;
        }

        /// <summary>
        /// Transform anchor point Z, absent if no transform specified
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("anchorZ"), IsRequired = (false))]
        public long? AnchorZ
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates how many time this layer has painted.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("paintCount"), IsRequired = (true))]
        public int PaintCount
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether this layer hosts any content, rather than being used for
        /// transform/scrolling purposes only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("drawsContent"), IsRequired = (true))]
        public bool DrawsContent
        {
            get;
            set;
        }

        /// <summary>
        /// Set if layer is not visible.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("invisible"), IsRequired = (false))]
        public bool? Invisible
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangles scrolling on main thread only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollRects"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.LayerTree.ScrollRect> ScrollRects
        {
            get;
            set;
        }

        /// <summary>
        /// Sticky position constraint information
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("stickyPositionConstraint"), IsRequired = (false))]
        public CefSharp.DevTools.LayerTree.StickyPositionConstraint StickyPositionConstraint
        {
            get;
            set;
        }
    }
}