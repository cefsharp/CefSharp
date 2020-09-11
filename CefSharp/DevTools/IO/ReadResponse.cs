// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IO
{
    /// <summary>
    /// ReadResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ReadResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool? base64Encoded
        {
            get;
            set;
        }

        /// <summary>
        /// Set if the data is base64-encoded
        /// </summary>
        public bool? Base64Encoded
        {
            get
            {
                return base64Encoded;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string data
        {
            get;
            set;
        }

        /// <summary>
        /// Data that were read.
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool eof
        {
            get;
            set;
        }

        /// <summary>
        /// Set if the end-of-file condition occured while reading.
        /// </summary>
        public bool Eof
        {
            get
            {
                return eof;
            }
        }
    }
}