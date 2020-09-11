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
        /// body
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
        /// originalSize
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
        /// encodedSize
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