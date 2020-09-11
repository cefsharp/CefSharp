// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// CanEmulateResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CanEmulateResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool result
        {
            get;
            set;
        }

        /// <summary>
        /// result
        /// </summary>
        public bool Result
        {
            get
            {
                return result;
            }
        }
    }
}