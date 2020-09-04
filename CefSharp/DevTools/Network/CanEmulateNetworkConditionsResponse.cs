// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// CanEmulateNetworkConditionsResponse
    /// </summary>
    public class CanEmulateNetworkConditionsResponse
    {
        /// <summary>
        /// True if emulation of network conditions is supported.
        /// </summary>
        public bool result
        {
            get;
            set;
        }
    }
}