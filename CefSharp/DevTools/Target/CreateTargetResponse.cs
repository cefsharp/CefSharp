// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// CreateTargetResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CreateTargetResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string targetId
        {
            get;
            set;
        }

        /// <summary>
        /// targetId
        /// </summary>
        public string TargetId
        {
            get
            {
                return targetId;
            }
        }
    }
}