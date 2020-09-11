// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Backend node with a friendly name.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class BackendNode : CefSharp.DevTools.DevToolsDomainEntityBase
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
        /// BackendNodeId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (true))]
        public int BackendNodeId
        {
            get;
            set;
        }
    }
}