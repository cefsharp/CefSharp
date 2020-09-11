// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetAttributesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetAttributesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] attributes
        {
            get;
            set;
        }

        /// <summary>
        /// attributes
        /// </summary>
        public string[] Attributes
        {
            get
            {
                return attributes;
            }
        }
    }
}