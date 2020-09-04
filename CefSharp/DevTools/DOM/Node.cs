// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// DOM interaction is implemented in terms of mirror objects that represent the actual DOM nodes.
    public class Node
    {
        /// <summary>
        /// Node identifier that is passed into the rest of the DOM messages as the `nodeId`. Backend
        public int NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// The id of the parent node if any.
        /// </summary>
        public int? ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// The BackendNodeId for this node.
        /// </summary>
        public int BackendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeType.
        /// </summary>
        public int NodeType
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeName.
        /// </summary>
        public string NodeName
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s localName.
        /// </summary>
        public string LocalName
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeValue.
        /// </summary>
        public string NodeValue
        {
            get;
            set;
        }

        /// <summary>
        /// Child count for `Container` nodes.
        /// </summary>
        public int? ChildNodeCount
        {
            get;
            set;
        }

        /// <summary>
        /// Child nodes of this node when requested with children.
        /// </summary>
        public System.Collections.Generic.IList<Node> Children
        {
            get;
            set;
        }

        /// <summary>
        /// Attributes of the `Element` node in the form of flat array `[name1, value1, name2, value2]`.
        /// </summary>
        public string Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Document URL that `Document` or `FrameOwner` node points to.
        /// </summary>
        public string DocumentURL
        {
            get;
            set;
        }

        /// <summary>
        /// Base URL that `Document` or `FrameOwner` node uses for URL completion.
        /// </summary>
        public string BaseURL
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType`'s publicId.
        /// </summary>
        public string PublicId
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType`'s systemId.
        /// </summary>
        public string SystemId
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType`'s internalSubset.
        /// </summary>
        public string InternalSubset
        {
            get;
            set;
        }

        /// <summary>
        /// `Document`'s XML version in case of XML documents.
        /// </summary>
        public string XmlVersion
        {
            get;
            set;
        }

        /// <summary>
        /// `Attr`'s name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// `Attr`'s value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Pseudo element type for this node.
        /// </summary>
        public string PseudoType
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow root type.
        /// </summary>
        public string ShadowRootType
        {
            get;
            set;
        }

        /// <summary>
        /// Frame ID for frame owner elements.
        /// </summary>
        public string FrameId
        {
            get;
            set;
        }

        /// <summary>
        /// Content document for frame owner elements.
        /// </summary>
        public Node ContentDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow root list for given element host.
        /// </summary>
        public System.Collections.Generic.IList<Node> ShadowRoots
        {
            get;
            set;
        }

        /// <summary>
        /// Content document fragment for template elements.
        /// </summary>
        public Node TemplateContent
        {
            get;
            set;
        }

        /// <summary>
        /// Pseudo elements associated with this node.
        /// </summary>
        public System.Collections.Generic.IList<Node> PseudoElements
        {
            get;
            set;
        }

        /// <summary>
        /// Import document for the HTMLImport links.
        /// </summary>
        public Node ImportedDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Distributed nodes for given insertion point.
        /// </summary>
        public System.Collections.Generic.IList<BackendNode> DistributedNodes
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the node is SVG.
        /// </summary>
        public bool? IsSVG
        {
            get;
            set;
        }
    }
}