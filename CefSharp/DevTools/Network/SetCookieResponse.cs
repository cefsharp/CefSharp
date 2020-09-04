// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// SetCookieResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetCookieResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool success
        {
            get;
            set;
        }

        /// <summary>
        /// True if successfully set cookie.
        /// </summary>
        public bool Success
        {
            get
            {
                return success;
            }
        }
    }
}