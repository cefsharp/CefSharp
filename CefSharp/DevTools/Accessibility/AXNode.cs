// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// A node in the accessibility tree.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AXNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Unique identifier for this node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeId"), IsRequired = (true))]
        public string NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this node is ignored for accessibility
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ignored"), IsRequired = (true))]
        public bool Ignored
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of reasons why this node is hidden.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ignoredReasons"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXProperty> IgnoredReasons
        {
            get;
            set;
        }

        /// <summary>
        /// This `Node`'s role, whether explicit or implicit.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("role"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue Role
        {
            get;
            set;
        }

        /// <summary>
        /// The accessible name for this `Node`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue Name
        {
            get;
            set;
        }

        /// <summary>
        /// The accessible description for this `Node`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("description"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue Description
        {
            get;
            set;
        }

        /// <summary>
        /// The value for this `Node`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue Value
        {
            get;
            set;
        }

        /// <summary>
        /// All other properties
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("properties"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXProperty> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// IDs for each of this node's child nodes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("childIds"), IsRequired = (false))]
        public string[] ChildIds
        {
            get;
            set;
        }

        /// <summary>
        /// The backend ID for the associated DOM node, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendDOMNodeId"), IsRequired = (false))]
        public int? BackendDOMNodeId
        {
            get;
            set;
        }
    }
}