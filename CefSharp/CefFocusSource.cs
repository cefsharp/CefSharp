// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
	/// <summary>
	/// Focus Source
	/// </summary>
	public enum CefFocusSource
	{
		///
		// The source is explicit navigation via the API (LoadURL(), etc).
		///
		FocusSourceNavigation = 0,
		///
		// The source is a system-generated focus event.
		///
		FocusSourceSystem
	}
}
