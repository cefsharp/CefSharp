// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;
using CefSharp.Structs;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace CefSharp.OffScreen
{
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

        public DefaultRenderHandler(ChromiumWebBrowser browser)
        {
            this.browser = browser;

            popupPosition = new Point();
            popupSize = new Size();

            BitmapBuffer = new BitmapBuffer(BitmapLock);
            PopupBuffer = new BitmapBuffer(BitmapLock);
        }

        public void Dispose()
        {
            browser = null;
        }

        public virtual ScreenInfo? GetScreenInfo()
        {
            var screenInfo = new ScreenInfo { DeviceScaleFactor = 1.0F };

            return screenInfo;
        }

        public virtual Rect? GetViewRect()
        {
            //TODO: See if this can be refactored and remove browser reference
            var size = browser.Size;

            var viewRect = new Rect(0, 0, size.Width, size.Height);

            return viewRect;
        }

        public virtual bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
        {
            screenX = viewX;
            screenY = viewY;

            return false;
        }

        public virtual void OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            var isPopup = type == PaintElementType.Popup;

            var bitmapBuffer = isPopup ? PopupBuffer : BitmapBuffer;

            bitmapBuffer.UpdateBuffer(width, height, buffer, dirtyRect);
        }

        public virtual void OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {

        }

        public virtual bool StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
        {
            return false;
        }

        public virtual void UpdateDragCursor(DragOperationsMask operation)
        {

        }

        public virtual void OnPopupShow(bool show)
        {
            PopupOpen = show;
        }

        public virtual void OnPopupSize(Rect rect)
        {
            popupPosition.X = rect.X;
            popupPosition.Y = rect.Y;
            popupSize.Width = rect.Width;
            popupSize.Height = rect.Height;
        }

        public virtual void OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds)
        {

        }
    }
}
