// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Controls;

namespace CefSharp.Wpf
{
	/// <summary>
	/// Factory class used to generate a BitmapInfo object for OSR rendering (WPF and OffScreen projects)
	/// Implement this interface if you wish to render the underlying Bitmap to a custom type
	/// e.g. a GDI Bitmap in the WPF Control
	/// </summary>
	public interface IBitmapFactory : IDisposable
	{
		void CreateOrUpdateBitmap(bool isPopup, IntPtr buffer, Rect dirtyRect, int width, int height, Image image);
	}
}