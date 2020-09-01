// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Information about the Frame hierarchy.
    /// </summary>
    public class FrameTree
    {
        /// <summary>
        /// Frame information for this tree item.
        /// </summary>
        public Frame Frame
        {
            get;
            set;
        }

        /// <summary>
        /// Child frames.
        /// </summary>
        public System.Collections.Generic.IList<FrameTree> ChildFrames
        {
            get;
            set;
        }
    }
}