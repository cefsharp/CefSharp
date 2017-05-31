// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public interface IRenderWebBrowser : IWebBrowserInternal
    {
        ScreenInfo GetScreenInfo();
        ViewRect GetViewRect();
        /// <summary>
        /// Called to retrieve the translation from view coordinates to actual screen coordinates. 
        /// </summary>
        /// <param name="viewX">x</param>
        /// <param name="viewY">y</param>
        /// <param name="screenX">screen x</param>
        /// <param name="screenY">screen y</param>
        /// <returns>Return true if the screen coordinates were provided.</returns>
        bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY);

        BitmapInfo CreateBitmapInfo(bool isPopup);
        
        /// <summary>
        /// Called when an element should be painted.
        /// Pixel values passed to this method are scaled relative to view coordinates based on the value of
        /// ScreenInfo.DeviceScaleFactor returned from GetScreenInfo. bitmapInfo.IsPopup indicates whether the element is the view
        /// or the popup widget. BitmapInfo.DirtyRect contains the set of rectangles in pixel coordinates that need to be
        /// repainted. The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin.
        /// The underlying buffer is copied into the back buffer and is accessible via BackBufferHandle
        /// </summary>
        /// <param name="bitmapInfo">information about the bitmap to be rendered</param>
        void OnPaint(BitmapInfo bitmapInfo);

        void SetCursor(IntPtr cursor, CursorType type);

        bool StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y);
        void UpdateDragCursor(DragOperationsMask operation);

        void SetPopupIsOpen(bool show);
        void SetPopupSizeAndPosition(int width, int height, int x, int y);

        /// <summary>
        /// Called when the IME composition range has changed.
        /// </summary>
        /// <param name="selectedRange">is the range of characters that have been selected</param>
        /// <param name="characterBounds">is the bounds of each character in view coordinates.</param>
        void OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds);
    };
}
