// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Animation
{
    /// <summary>
    /// ResolveAnimationResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ResolveAnimationResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.RemoteObject remoteObject
        {
            get;
            set;
        }

        /// <summary>
        /// remoteObject
        /// </summary>
        public CefSharp.DevTools.Runtime.RemoteObject RemoteObject
        {
            get
            {
                return remoteObject;
            }
        }
    }
}