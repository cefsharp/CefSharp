// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// Table containing nodes.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class NodeTreeSnapshot : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Parent node index.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parentIndex"), IsRequired = (false))]
        public int[] ParentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeType.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeType"), IsRequired = (false))]
        public int[] NodeType
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeName.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeName"), IsRequired = (false))]
        public int[] NodeName
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s nodeValue.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nodeValue"), IsRequired = (false))]
        public int[] NodeValue
        {
            get;
            set;
        }

        /// <summary>
        /// `Node`'s id, corresponds to DOM.Node.backendNodeId.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (false))]
        public int[] BackendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Attributes of an `Element` node. Flatten name, value pairs.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("attributes"), IsRequired = (false))]
        public int[] Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for textarea elements, contains the text value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("textValue"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareStringData TextValue
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for input elements, contains the input's associated text value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("inputValue"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareStringData InputValue
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for radio and checkbox input elements, indicates if the element has been checked
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("inputChecked"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareBooleanData InputChecked
        {
            get;
            set;
        }

        /// <summary>
        /// Only set for option elements, indicates if the element has been selected
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("optionSelected"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareBooleanData OptionSelected
        {
            get;
            set;
        }

        /// <summary>
        /// The index of the document in the list of the snapshot documents.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentDocumentIndex"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareIntegerData ContentDocumentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Type of a pseudo element node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pseudoType"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareStringData PseudoType
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
        public CefSharp.DevTools.DOMSnapshot.RareBooleanData IsClickable
        {
            get;
            set;
        }

        /// <summary>
        /// The selected url for nodes with a srcset attribute.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("currentSourceURL"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareStringData CurrentSourceURL
        {
            get;
            set;
        }

        /// <summary>
        /// The url of the script (if any) that generates this node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("originURL"), IsRequired = (false))]
        public CefSharp.DevTools.DOMSnapshot.RareStringData OriginURL
        {
            get;
            set;
        }
    }
}