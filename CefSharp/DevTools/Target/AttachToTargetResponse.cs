// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// AttachToTargetResponse
    /// </summary>
    public class AttachToTargetResponse
    {
        /// <summary>
        /// Id assigned to the session.
        /// </summary>
        public string sessionId
        {
            get;
            set;
        }
    }
}