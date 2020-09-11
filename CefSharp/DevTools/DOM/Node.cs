// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// DOM interaction is implemented in terms of mirror objects that represent the actual DOM nodes.
    /// DOMNode is a base node mirror type.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Node : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Node identifier that is passed into the rest of the DOM messages as the `nodeId`. Backend
        /// will only push node with given `id` once. It is aware of all requested nodes and will only
        /// fire DOM events for nodes known to the client.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeId"), IsRequired = (true))]
        public int NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// The id of the parent node if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parentId"), IsRequired = (false))]
        public int? ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// The BackendNodeId for this node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (true))]
        public int BackendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeType.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeType"), IsRequired = (true))]
        public int NodeType
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeName.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeName"), IsRequired = (true))]
        public string NodeName
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s localName.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("localName"), IsRequired = (true))]
        public string LocalName
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeValue.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeValue"), IsRequired = (true))]
        public string NodeValue
        {
            get;
            set;
        }

        /// <summary>
        /// Child count for `Container` nodes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("childNodeCount"), IsRequired = (false))]
        public int? ChildNodeCount
        {
            get;
            set;
        }

        /// <summary>
        /// Child nodes of this node when requested with children.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("children"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOM.Node> Children
        {
            get;
            set;
        }

        /// <summary>
        /// Attributes of the `Element` node in the form of flat array `[name1, value1, name2, value2]`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("attributes"), IsRequired = (false))]
        public string[] Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Document URL that `Document` or `FrameOwner` node points to.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("documentURL"), IsRequired = (false))]
        public string DocumentURL
        {
            get;
            set;
        }

        /// <summary>
        /// Base URL that `Document` or `FrameOwner` node uses for URL completion.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("baseURL"), IsRequired = (false))]
        public string BaseURL
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType`'s publicId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("publicId"), IsRequired = (false))]
        public string PublicId
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType`'s systemId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("systemId"), IsRequired = (false))]
        public string SystemId
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType`'s internalSubset.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("internalSubset"), IsRequired = (false))]
        public string InternalSubset
        {
            get;
            set;
        }

        /// <summary>
        /// `Document`'s XML version in case of XML documents.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("xmlVersion"), IsRequired = (false))]
        public string XmlVersion
        {
            get;
            set;
        }

        /// <summary>
        /// `Attr`'s name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (false))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// `Attr`'s value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public string Value
        {
            get;
            set;
        }

        public CefSharp.DevTools.DOM.PseudoType? PseudoType
        {
            get
            {
                return (CefSharp.DevTools.DOM.PseudoType? )(StringToEnum(typeof(CefSharp.DevTools.DOM.PseudoType? ), pseudoType));
            }

            set
            {
                pseudoType = (EnumToString(value));
            }
        }

        /// <summary>
        /// Pseudo element type for this node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pseudoType"), IsRequired = (false))]
        internal string pseudoType
        {
            get;
            set;
        }

        public CefSharp.DevTools.DOM.ShadowRootType? ShadowRootType
        {
            get
            {
                return (CefSharp.DevTools.DOM.ShadowRootType? )(StringToEnum(typeof(CefSharp.DevTools.DOM.ShadowRootType? ), shadowRootType));
            }

            set
            {
                shadowRootType = (EnumToString(value));
            }
        }

        /// <summary>
        /// Shadow root type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shadowRootType"), IsRequired = (false))]
        internal string shadowRootType
        {
            get;
            set;
        }

        /// <summary>
        /// Frame ID for frame owner elements.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frameId"), IsRequired = (false))]
        public string FrameId
        {
            get;
            set;
        }

        /// <summary>
        /// Content document for frame owner elements.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentDocument"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.Node ContentDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow root list for given element host.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shadowRoots"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOM.Node> ShadowRoots
        {
            get;
            set;
        }

        /// <summary>
        /// Content document fragment for template elements.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("templateContent"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.Node TemplateContent
        {
            get;
            set;
        }

        /// <summary>
        /// Pseudo elements associated with this node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pseudoElements"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOM.Node> PseudoElements
        {
            get;
            set;
        }

        /// <summary>
        /// Import document for the HTMLImport links.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("importedDocument"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.Node ImportedDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Distributed nodes for given insertion point.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("distributedNodes"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOM.BackendNode> DistributedNodes
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the node is SVG.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isSVG"), IsRequired = (false))]
        public bool? IsSVG
        {
            get;
            set;
        }
    }
}