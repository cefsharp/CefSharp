// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to dragging.
    /// The methods of this class will be called on the UI thread. 
    /// </summary>
    public interface IDragHandler
    {
        /// <summary>
        /// Called when an external drag event enters the browser window.
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="dragData">contains the drag event data</param>
        /// <param name="mask">represents the type of drag operation</param>
        /// <returns>Return false for default drag handling behavior or true to cancel the drag event. </returns>
        bool OnDragEnter(IWebBrowser browserControl, IBrowser browser, IDragData dragData, DragOperationsMask mask);
    }
}
