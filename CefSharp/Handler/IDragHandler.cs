// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using CefSharp.Enums;

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

        /// <summary>
        /// Called whenever draggable regions for the browser window change.
        /// These can be specified using the '-webkit-app-region: drag/no-drag' CSS-property.
        /// If draggable regions are never defined in a document this method will also never be called.
        /// If the last draggable region is removed from a document this method will be called with an empty IList. 
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="regions">List of <see cref="DraggableRegion"/> objects or null if last region was removed.</param>
        void OnDraggableRegionsChanged(IWebBrowser browserControl, IBrowser browser, IList<DraggableRegion> regions);
    }
}
