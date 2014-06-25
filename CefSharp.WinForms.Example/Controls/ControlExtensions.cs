// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Controls
{
	public static class ControlExtensions
	{
		/// <summary>
		/// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
		/// </summary>
		/// <param name="control">the control for which the update is required</param>
		/// <param name="action">action to be performed on the control</param>
		public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
		{
			if (control.InvokeRequired)
			{
				control.BeginInvoke(action);
			}
			else
			{
				action.Invoke();
			}
		}
	}
}
