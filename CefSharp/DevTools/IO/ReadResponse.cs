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
        /// base64Encoded
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
        /// data
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
        /// eof
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