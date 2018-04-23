// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;
using CefSharp.Structs;

namespace CefSharp.Internals
{
    public interface IRenderWebBrowser : IWebBrowserInternal
    {
        /// <summary>
        /// Called to allow the client to return a ScreenInfo object with appropriate values.
        /// If null is returned then the rectangle from GetViewRect will be used.
        /// If the rectangle is still empty or invalid popups may not be drawn correctly. 
        /// </summary>
        /// <returns>Return null if no screenInfo structure is provided.</returns>	
        ScreenInfo? GetScreenInfo();

        /// <summary>
        /// Called to retrieve the view rectangle which is relative to screen coordinates. 
        /// </summary>
        /// <returns>Return a ViewRect strict containing the rectangle or null. If the rectangle is
        /// still empty or invalid popups may not be drawn correctly. </returns>
        ViewRect? GetViewRect();

        /// <summary>
        /// Called to retrieve the translation from view coordinates to actual screen coordinates. 
        /// </summary>
        /// <param name="viewX">x</param>
        /// <param name="viewY">y</param>
        /// <param name="screenX">screen x</param>
        /// <param name="screenY">screen y</param>
        /// <returns>Return true if the screen coordinates were provided.</returns>
        bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY);

        /// <summary>
        /// Called when an element should be painted. Pixel values passed to this method are scaled relative to view coordinates based on the
        /// value of <see cref="ScreenInfo.ScaleFactor"/> returned from <see cref="GetScreenInfo"/>.
        /// Called on the CEF UI Thread
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        void OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height);

        /// <summary>
        /// Called when the browser's cursor has changed. . 
        /// </summary>
        /// <param name="cursor">If type is Custom then customCursorInfo will be populated with the custom cursor information</param>
        /// <param name="type">cursor type</param>
        /// <param name="customCursorInfo">custom cursor Information</param>
        void OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo);

        /// <summary>
        /// Called when the user starts dragging content in the web view. Contextual information about the dragged content is
        /// supplied by dragData. (|x|, |y|) is the drag start location in screen coordinates. OS APIs that run a system message
        /// loop may be used within the StartDragging call. Return false to abort the drag operation. Don't call any of
        /// CefBrowserHost::DragSource*Ended* methods after returning false. Return true to handle the drag operation.
        /// Call IBrowserHost::DragSourceEndedAt and DragSourceSystemDragEnded either synchronously or asynchronously to inform
        /// the web view that the drag operation has ended. 
        /// </summary>
        /// <param name="dragData">drag data</param>
        /// <param name="mask">operation mask</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>Return false to abort the drag operation.</returns>
        bool StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y);

        /// <summary>
        /// Called when the web view wants to update the mouse cursor during a drag & drop operation.
        /// </summary>
        /// <param name="operation">describes the allowed operation (none, move, copy, link). </param>
        void UpdateDragCursor(DragOperationsMask operation);

        /// <summary>
        /// Called when the browser wants to show or hide the popup widget.  
        /// </summary>
        /// <param name="show">The popup should be shown if show is true and hidden if show is false.</param>
        void OnPopupShow(bool show);

        /// <summary>
        /// Called when the browser wants to move or resize the popup widget. 
        /// </summary>
        /// <param name="rect">contains the new location and size in view coordinates. </param>
        void OnPopupSize(Rect rect);

        /// <summary>
        /// Called when the IME composition range has changed.
        /// </summary>
        /// <param name="selectedRange">is the range of characters that have been selected</param>
        /// <param name="characterBounds">is the bounds of each character in view coordinates.</param>
        void OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds);
    };
}
