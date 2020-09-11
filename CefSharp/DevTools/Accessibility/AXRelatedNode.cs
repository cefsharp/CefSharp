// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// AXRelatedNode
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AXRelatedNode : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The BackendNodeId of the related DOM node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendDOMNodeId"), IsRequired = (true))]
        public int BackendDOMNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// The IDRef value provided, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("idref"), IsRequired = (false))]
        public string Idref
        {
            get;
            set;
        }

        /// <summary>
        /// The text alternative of this node in the current context.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (false))]
        public string Text
        {
            get;
            set;
        }
    }
}