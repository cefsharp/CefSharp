// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetBoxModelResponse
    /// </summary>
    public class GetBoxModelResponse
    {
        /// <summary>
        /// Box model for the node.
        /// </summary>
        public BoxModel model
        {
            get;
            set;
        }
    }
}