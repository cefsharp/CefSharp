// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// MoveToResponse
    /// </summary>
    public class MoveToResponse
    {
        /// <summary>
        /// New id of the moved node.
        /// </summary>
        public int nodeId
        {
            get;
            set;
        }
    }
}