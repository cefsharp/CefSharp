// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetNodeStackTracesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetNodeStackTracesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.StackTrace creation
        {
            get;
            set;
        }

        /// <summary>
        /// creation
        /// </summary>
        public CefSharp.DevTools.Runtime.StackTrace Creation
        {
            get
            {
                return creation;
            }
        }
    }
}