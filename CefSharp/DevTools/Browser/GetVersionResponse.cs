// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// GetVersionResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetVersionResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string protocolVersion
        {
            get;
            set;
        }

        /// <summary>
        /// protocolVersion
        /// </summary>
        public string ProtocolVersion
        {
            get
            {
                return protocolVersion;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string product
        {
            get;
            set;
        }

        /// <summary>
        /// product
        /// </summary>
        public string Product
        {
            get
            {
                return product;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string revision
        {
            get;
            set;
        }

        /// <summary>
        /// revision
        /// </summary>
        public string Revision
        {
            get
            {
                return revision;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string userAgent
        {
            get;
            set;
        }

        /// <summary>
        /// userAgent
        /// </summary>
        public string UserAgent
        {
            get
            {
                return userAgent;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string jsVersion
        {
            get;
            set;
        }

        /// <summary>
        /// jsVersion
        /// </summary>
        public string JsVersion
        {
            get
            {
                return jsVersion;
            }
        }
    }
}