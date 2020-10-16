// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// A Node in the DOM tree.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DOMNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
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
        /// `Node`'s nodeValue.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeValue"), IsRequired = (true))]
        public string NodeValue
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for textarea elements, contains the text value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("textValue"), IsRequired = (false))]
        public string TextValue
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for input elements, contains the input's associated text value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("inputValue"), IsRequired = (false))]
        public string InputValue
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for radio and checkbox input elements, indicates if the element has been checked
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("inputChecked"), IsRequired = (false))]
        public bool? InputChecked
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for option elements, indicates if the element has been selected
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("optionSelected"), IsRequired = (false))]
        public bool? OptionSelected
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s id, corresponds to DOM.Node.backendNodeId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (true))]
        public int BackendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// The indexes of the node's child nodes in the `domNodes` array returned by `getSnapshot`, if
        /// any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("childNodeIndexes"), IsRequired = (false))]
        public int[] ChildNodeIndexes
        {
            get;
            set;
        }

        /// <summary>
        /// Attributes of an `Element` node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("attributes"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOMSnapshot.NameValue> Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Indexes of pseudo elements associated with this node in the `domNodes` array returned by
        /// `getSnapshot`, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pseudoElementIndexes"), IsRequired = (false))]
        public int[] PseudoElementIndexes
        {
            get;
            set;
        }

        /// <summary>
        /// The index of the node's related layout tree node in the `layoutTreeNodes` array returned by
        /// `getSnapshot`, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("layoutNodeIndex"), IsRequired = (false))]
        public int? LayoutNodeIndex
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
        /// Only set for documents, contains the document's content language.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentLanguage"), IsRequired = (false))]
        public string ContentLanguage
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for documents, contains the document's character set encoding.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("documentEncoding"), IsRequired = (false))]
        public string DocumentEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType` node's publicId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("publicId"), IsRequired = (false))]
        public string PublicId
        {
            get;
            set;
        }

        /// <summary>
        /// `DocumentType` node's systemId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("systemId"), IsRequired = (false))]
        public string SystemId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame ID for frame owner elements and also for the document node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frameId"), IsRequired = (false))]
        public string FrameId
        {
            get;
            set;
        }

        /// <summary>
        /// The index of a frame owner element's content document in the `domNodes` array returned by
        /// `getSnapshot`, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentDocumentIndex"), IsRequired = (false))]
        public int? ContentDocumentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Type of a pseudo element node.
        /// </summary>
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
        /// Type of a pseudo element node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pseudoType"), IsRequired = (false))]
        internal string pseudoType
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow root type.
        /// </summary>
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
        /// Whether this DOM node responds to mouse clicks. This includes nodes that have had click
        /// event listeners attached via JavaScript as well as anchor tags that naturally navigate when
        /// clicked.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isClickable"), IsRequired = (false))]
        public bool? IsClickable
        {
            get;
            set;
        }

        /// <summary>
        /// Details of the node's event listeners, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("eventListeners"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.DOMDebugger.EventListener> EventListeners
        {
            get;
            set;
        }

        /// <summary>
        /// The selected url for nodes with a srcset attribute.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("currentSourceURL"), IsRequired = (false))]
        public string CurrentSourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// The url of the script (if any) that generates this node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("originURL"), IsRequired = (false))]
        public string OriginURL
        {
            get;
            set;
        }

        /// <summary>
        /// Scroll offsets, set when this node is a Document.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollOffsetX"), IsRequired = (false))]
        public long? ScrollOffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// ScrollOffsetY
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollOffsetY"), IsRequired = (false))]
        public long? ScrollOffsetY
        {
            get;
            set;
        }
    }
}