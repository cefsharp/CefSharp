// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// ResolveNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ResolveNodeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.RemoteObject @object
        {
            get;
            set;
        }

        /// <summary>
        /// object
        /// </summary>
        public CefSharp.DevTools.Runtime.RemoteObject Object
        {
            get
            {
                return @object;
            }
        }
    }
}