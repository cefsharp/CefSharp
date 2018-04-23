// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Structs
{
	public struct CursorInfo
	{
		public IntPtr Buffer { get; set; }
		public Point Hotspot { get; private set; }
		public float ImageScaleFactor { get; private set; }
		public Size Size { get; private set; }

		public CursorInfo(IntPtr buffer, Point hotspot, float imageScaleFactor, Size size)
		{
			Buffer = buffer;
			Hotspot = hotspot;
			ImageScaleFactor = imageScaleFactor;
			Size = size;
		}
	}
}
