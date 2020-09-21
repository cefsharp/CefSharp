// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange header.
    /// https://wicg.github.io/webpackage/draft-yasskin-httpbis-origin-signed-exchanges-impl.html#cbor-representation
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SignedExchangeHeader : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Signed exchange request URL.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestUrl"), IsRequired = (true))]
        public string RequestUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange response code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseCode"), IsRequired = (true))]
        public int ResponseCode
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange response headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("responseHeaders"), IsRequired = (true))]
        public CefSharp.DevTools.Network.Headers ResponseHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange response signature.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signatures"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Network.SignedExchangeSignature> Signatures
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange header integrity hash in the form of "sha256-<base64-hash-value>".
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headerIntegrity"), IsRequired = (true))]
        public string HeaderIntegrity
        {
            get;
            set;
        }
    }
}