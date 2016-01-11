// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms.Example.Handlers
{
	public class WinFormsRequestHandler : RequestHandler
	{
		private Action<string, int?> openNewTab;

		public WinFormsRequestHandler(Action<string, int?> openNewTab)
		{
			this.openNewTab = openNewTab;
		}

		protected override bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
		{
			if(openNewTab == null)
			{
				return false;
			}

			var control = (Control)browserControl;

			control.InvokeOnUiThreadIfRequired(delegate ()
			{
				openNewTab(targetUrl, null);
			});			

			return true;
		}
	}
}
