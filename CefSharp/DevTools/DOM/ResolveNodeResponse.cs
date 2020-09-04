// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// ResolveNodeResponse
    /// </summary>
    public class ResolveNodeResponse
    {
        /// <summary>
        /// JavaScript object wrapper for given node.
        /// </summary>
        public Runtime.RemoteObject @object
        {
            get;
            set;
        }
    }
}