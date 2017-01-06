// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Wpf.Example
{
	public static class Program
	{
		[STAThread]
		public static int Main(string[] args)
		{
			//Until https://bitbucket.org/chromiumembedded/cef/issues/1995/ is resolved it's nessicary to
			//deal with the spawning of the crashpad process here as it's not possible to configure which exe it uses
			//When running from within VS and using the vshost process you'll see an error in the log and this won't get called.
			var crashpadHandlerExitCode = Cef.ExecuteProcess();

			//crashpadHandlerExitCode will be -1 for normal process execution, only when running as a subprocess will it be greater
			if (crashpadHandlerExitCode >= 0)
			{
				return crashpadHandlerExitCode;
			}

			var application = new App();
			application.InitializeComponent();
			return application.Run();
		}
	}
}
