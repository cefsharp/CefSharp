// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// CreateTargetResponse
    /// </summary>
    public class CreateTargetResponse
    {
        /// <summary>
        /// The id of the page opened.
        /// </summary>
        public string targetId
        {
            get;
            set;
        }
    }
}