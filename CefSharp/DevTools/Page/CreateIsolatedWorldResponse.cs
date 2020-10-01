// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// CreateIsolatedWorldResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CreateIsolatedWorldResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int executionContextId
        {
            get;
            set;
        }

        /// <summary>
        /// executionContextId
        /// </summary>
        public int ExecutionContextId
        {
            get
            {
                return executionContextId;
            }
        }
    }
}