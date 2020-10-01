// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Memory
{
    /// <summary>
    /// GetDOMCountersResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetDOMCountersResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int documents
        {
            get;
            set;
        }

        /// <summary>
        /// documents
        /// </summary>
        public int Documents
        {
            get
            {
                return documents;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal int nodes
        {
            get;
            set;
        }

        /// <summary>
        /// nodes
        /// </summary>
        public int Nodes
        {
            get
            {
                return nodes;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal int jsEventListeners
        {
            get;
            set;
        }

        /// <summary>
        /// jsEventListeners
        /// </summary>
        public int JsEventListeners
        {
            get
            {
                return jsEventListeners;
            }
        }
    }
}