// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
	internal class DefaultFocusHandler : IFocusHandler
	{
		private readonly ChromiumWebBrowser browser;

		public DefaultFocusHandler(ChromiumWebBrowser browser)
		{
			this.browser = browser;
		}

		public void OnGotFocus()
		{
		}

		public bool OnSetFocus(CefFocusSource source)
		{
			return false;
		}

		public void OnTakeFocus(bool next)
		{
			browser.BeginInvoke(new MethodInvoker(() => browser.SelectNextControl(browser, next, true, true, true)));
		}
	}
}
