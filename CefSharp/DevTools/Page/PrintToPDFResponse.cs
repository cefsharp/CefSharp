// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// PrintToPDFResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PrintToPDFResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string data
        {
            get;
            set;
        }

        /// <summary>
        /// data
        /// </summary>
        public byte[] Data
        {
            get
            {
                return Convert(data);
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string stream
        {
            get;
            set;
        }

        /// <summary>
        /// stream
        /// </summary>
        public string Stream
        {
            get
            {
                return stream;
            }
        }
    }
}