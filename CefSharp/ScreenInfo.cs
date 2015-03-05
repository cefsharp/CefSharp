// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
	public struct ScreenInfo
	{
        public int X { get; set; }
        public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

        public int AvailableX { get; set; }
        public int AvailableY { get; set; }
        public int AvailableWidth { get; set; }
        public int AvailableHeight { get; set; }

		public float ScaleFactor { get; set; }
	}
}
