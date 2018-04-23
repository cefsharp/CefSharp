// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CefSharp.Structs;

namespace CefSharp.OffScreen
{
	public class BitmapBuffer
	{
		private const int BytesPerPixel = 4;
		private const PixelFormat Format = PixelFormat.Format32bppPArgb;

		private byte[] buffer;

		/// <summary>
		/// Number of bytes
		/// </summary>
		public int NumberOfBytes { get; private set; }
		/// <summary>
		/// Width
		/// </summary>
		public int Width { get; private set; }
		/// <summary>
		/// Height
		/// </summary>
		public int Height { get; private set; }
		/// <summary>
		/// Dirty Rect - unified region containing th
		/// </summary>
		public Rect DirtyRect { get; private set; }
		
		public object BitmapLock { get; private set; }

		public BitmapBuffer(object bitmapLock)
		{
			BitmapLock = bitmapLock;
		}

		public byte[] Buffer
		{
			get { return buffer; }
		}

		//TODO: May need to Pin the buffer in memory using GCHandle.Alloc(this.buffer, GCHandleType.Pinned);
		private void ResizeBuffer(int width, int height)
		{
			if (buffer == null)
			{
				//No of Pixels (width * height) * BytesPerPixel
				NumberOfBytes = width * height * BytesPerPixel;

				buffer = new byte[NumberOfBytes];

				Width = width;
				Height = height;
			}
			else if (width != Width || height != Height)
			{
				//No of Pixels (width * height) * BytesPerPixel
				NumberOfBytes = width * height * BytesPerPixel;

				Array.Resize(ref buffer, NumberOfBytes);
				Width = width;
				Height = height;
			}
		}

		public void UpdateBuffer(int width, int height, IntPtr buffer, Rect dirtyRect)
		{
			lock (BitmapLock)
			{
				DirtyRect = dirtyRect;
				ResizeBuffer(width, height);
				Marshal.Copy(buffer, this.buffer, 0, NumberOfBytes);
			}
		}

		public Bitmap CreateBitmap()
		{
			lock (BitmapLock)
			{
				if (Width == 0 || Height == 0 || buffer.Length == 0)
				{
					return null;
				}

				var stride = Width * BytesPerPixel;

				var pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);

				var bitmap = new Bitmap(Width, Height, stride, Format, pointer);

				return bitmap;
			}
		}
	}
}
