// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// GetEncodedResponseResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetEncodedResponseResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string body
        {
            get;
            set;
        }

        /// <summary>
        /// The encoded body as a base64 string. Omitted if sizeOnly is true.
        /// </summary>
        public byte[] Body
        {
            get
            {
                return Convert(body);
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal int originalSize
        {
            get;
            set;
        }

        /// <summary>
        /// Size before re-encoding.
        /// </summary>
        public int OriginalSize
        {
            get
            {
                return originalSize;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal int encodedSize
        {
            get;
            set;
        }

        /// <summary>
        /// Size after re-encoding.
        /// </summary>
        public int EncodedSize
        {
            get
            {
                return encodedSize;
            }
        }
    }
}