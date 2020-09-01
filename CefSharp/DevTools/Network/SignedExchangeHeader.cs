// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange header.
    public class SignedExchangeHeader
    {
        /// <summary>
        /// Signed exchange request URL.
        /// </summary>
        public string RequestUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange response code.
        /// </summary>
        public int ResponseCode
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange response headers.
        /// </summary>
        public Headers ResponseHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange response signature.
        /// </summary>
        public System.Collections.Generic.IList<SignedExchangeSignature> Signatures
        {
            get;
            set;
        }

        /// <summary>
        /// Signed exchange header integrity hash in the form of "sha256-<base64-hash-value>".
        /// </summary>
        public string HeaderIntegrity
        {
            get;
            set;
        }
    }
}