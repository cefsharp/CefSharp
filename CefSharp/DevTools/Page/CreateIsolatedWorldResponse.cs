// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// CreateIsolatedWorldResponse
    /// </summary>
    public class CreateIsolatedWorldResponse
    {
        /// <summary>
        /// Execution context of the isolated world.
        /// </summary>
        public int executionContextId
        {
            get;
            set;
        }
    }
}