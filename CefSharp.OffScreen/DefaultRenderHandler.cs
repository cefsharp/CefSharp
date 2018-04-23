// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Structs;

namespace CefSharp.OffScreen
{
	public class DefaultRenderHandler : IRenderHandler
	{
		private ChromiumWebBrowser browser;

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
		/// Need a lock because the caller may be asking for the bitmap
		/// while Chromium async rendering has returned on another thread.
		/// </summary>
		public readonly object BitmapLock = new object();

		public DefaultRenderHandler(ChromiumWebBrowser browser)
		{
			this.browser = browser;

			BitmapBuffer = new BitmapBuffer(BitmapLock);
			PopupBuffer = new BitmapBuffer(BitmapLock);

		}

		public void Dispose()
		{
			browser = null;
		}

		public virtual void OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height)
		{
			var bitmapBuffer = isPopup ? PopupBuffer : BitmapBuffer;

			bitmapBuffer.UpdateBuffer(width, height, buffer, dirtyRect);
		}
	}
}
