// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;
using CefSharp.Structs;
using Point = System.Drawing.Point;
using Range = CefSharp.Structs.Range;
using Size = System.Drawing.Size;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// Default implementation of <see cref="IRenderHandler"/>, this class handles Offscreen Rendering (OSR).
    /// Upstream documentation at http://magpcss.org/ceforum/apidocs3/projects/(default)/CefRenderHandler.html
    /// </summary>
    public class DefaultRenderHandler : IRenderHandler
    {
        private ChromiumWebBrowser browser;

        private Size popupSize;
        private Point popupPosition;

        /// <summary>
        /// Need a lock because the caller may be asking for the bitmap
        /// while Chromium async rendering has returned on another thread.
        /// </summary>
        public readonly object BitmapLock = new object();

        /// <summary>
        /// Gets or sets a value indicating whether the popup is open.
        /// </summary>
        /// <value>
        /// <c>true</c> if popup is opened; otherwise, <c>false</c>.
        /// </value>
        public bool PopupOpen { get; protected set; }

        /// <summary>
        /// Contains the last bitmap buffer. Direct access
        /// to the underlying buffer - there is no locking when trying
        /// to access directly, use <see cref="BitmapBuffer.BitmapLock" /> where appropriate.
        /// </summary>
        /// <value>The bitmap.</value>
        public BitmapBuffer BitmapBuffer { get; private set; }

        /// <summary>
        /// The popup Bitmap.
        /// </summary>
        public BitmapBuffer PopupBuffer { get; private set; }

        /// <summary>
        /// Gets the size of the popup.
        /// </summary>
        public Size PopupSize
        {
            get { return popupSize; }
        }

        /// <summary>
        /// Gets the popup position.
        /// </summary>
        public Point PopupPosition
        {
            get { return popupPosition; }
        }

        /// <summary>
        /// Create a new instance of DefaultRenderHadler
        /// </summary>
        /// <param name="browser">reference to the ChromiumWebBrowser</param>
        public DefaultRenderHandler(ChromiumWebBrowser browser)
        {
            this.browser = browser;

            popupPosition = new Point();
            popupSize = new Size();

            BitmapBuffer = new BitmapBuffer(BitmapLock);
            PopupBuffer = new BitmapBuffer(BitmapLock);
        }

        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        public void Dispose()
        {
            browser = null;
            BitmapBuffer = null;
            PopupBuffer = null;
        }

        /// <summary>
        /// Called to allow the client to return a ScreenInfo object with appropriate values.
        /// If null is returned then the rectangle from GetViewRect will be used.
        /// If the rectangle is still empty or invalid popups may not be drawn correctly. 
        /// </summary>
        /// <returns>Return null if no screenInfo structure is provided.</returns>	
        public virtual ScreenInfo? GetScreenInfo()
        {
            var screenInfo = new ScreenInfo { DeviceScaleFactor = 1.0F };

            return screenInfo;
        }

        /// <summary>
        /// Called to retrieve the view rectangle which is relative to screen coordinates.
        /// This method must always provide a non-empty rectangle.
        /// </summary>
        /// <returns>Return a ViewRect strict containing the rectangle.</returns>
        public virtual Rect GetViewRect()
        {
            //TODO: See if this can be refactored and remove browser reference
            var size = browser.Size;

            var viewRect = new Rect(0, 0, size.Width, size.Height);

            return viewRect;
        }

        /// <summary>
        /// Called to retrieve the translation from view coordinates to actual screen coordinates. 
        /// </summary>
        /// <param name="viewX">x</param>
        /// <param name="viewY">y</param>
        /// <param name="screenX">screen x</param>
        /// <param name="screenY">screen y</param>
        /// <returns>Return true if the screen coordinates were provided.</returns>
        public virtual bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
        {
            screenX = viewX;
            screenY = viewY;

            return false;
        }

        /// <summary>
        /// Called when an element has been rendered to the shared texture handle.
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to true
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="sharedHandle">is the handle for a D3D11 Texture2D that can be accessed via ID3D11Device using the OpenSharedResource method.</param>
        public virtual void OnAcceleratedPaint(PaintElementType type, Rect dirtyRect, IntPtr sharedHandle)
        {
            //NOT USED
        }

        /// <summary>
        /// Called when an element should be painted. Pixel values passed to this method are scaled relative to view coordinates based on the
        /// value of <see cref="ScreenInfo.DeviceScaleFactor"/> returned from <see cref="GetScreenInfo"/>.
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to false.
        /// Called on the CEF UI Thread
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public virtual void OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            var isPopup = type == PaintElementType.Popup;

            var bitmapBuffer = isPopup ? PopupBuffer : BitmapBuffer;

            bitmapBuffer.UpdateBuffer(width, height, buffer, dirtyRect);
        }

        /// <summary>
        /// Called when the browser's cursor has changed.
        /// </summary>
        /// <param name="cursor">If type is Custom then customCursorInfo will be populated with the custom cursor information</param>
        /// <param name="type">cursor type</param>
        /// <param name="customCursorInfo">custom cursor Information</param>
        public virtual void OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {

        }

        /// <summary>
        /// Called when the user starts dragging content in the web view. Contextual information about the dragged content is
        /// supplied by dragData. OS APIs that run a system message loop may be used within the StartDragging call.
        /// Don't call any of the IBrowserHost.DragSource*Ended* methods after returning false.
        /// Return true to handle the drag operation. Call <see cref="IBrowserHost.DragSourceEndedAt"/> and <see cref="IBrowserHost.DragSourceSystemDragEnded"/> either synchronously or asynchronously to inform
        /// the web view that the drag operation has ended. 
        /// </summary>
        /// <param name="dragData">drag data</param>
        /// <param name="mask">operation mask</param>
        /// <param name="x">combined x and y provide the drag start location in screen coordinates</param>
        /// <param name="y">combined x and y provide the drag start location in screen coordinates</param>
        /// <returns>Return false to abort the drag operation.</returns>
        public virtual bool StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
        {
            return false;
        }

        /// <summary>
        /// Called when the web view wants to update the mouse cursor during a drag &amp; drop operation.
        /// </summary>
        /// <param name="operation">describes the allowed operation (none, move, copy, link). </param>
        public virtual void UpdateDragCursor(DragOperationsMask operation)
        {

        }

        /// <summary>
        /// Called when the browser wants to show or hide the popup widget.  
        /// </summary>
        /// <param name="show">The popup should be shown if show is true and hidden if show is false.</param>
        public virtual void OnPopupShow(bool show)
        {
            PopupOpen = show;
        }

        /// <summary>
        /// Called when the browser wants to move or resize the popup widget. 
        /// </summary>
        /// <param name="rect">contains the new location and size in view coordinates. </param>
        public virtual void OnPopupSize(Rect rect)
        {
            popupPosition.X = rect.X;
            popupPosition.Y = rect.Y;
            popupSize.Width = rect.Width;
            popupSize.Height = rect.Height;
        }

        /// <summary>
        /// Called when the IME composition range has changed.
        /// </summary>
        /// <param name="selectedRange">is the range of characters that have been selected</param>
        /// <param name="characterBounds">is the bounds of each character in view coordinates.</param>
        public virtual void OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds)
        {

        }

        /// <summary>
        /// Called when an on-screen keyboard should be shown or hidden for the specified browser. 
        /// </summary>
        /// <param name="browser">the browser</param>
        /// <param name="inputMode">specifies what kind of keyboard should be opened. If <see cref="TextInputMode.None"/>, any existing keyboard for this browser should be hidden.</param>
        public virtual void OnVirtualKeyboardRequested(IBrowser browser, TextInputMode inputMode)
        {

        }
    }
}
