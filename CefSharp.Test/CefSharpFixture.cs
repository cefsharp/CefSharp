// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Test
{
	public class CefSharpFixture : IDisposable
	{
		public CefSharpFixture()
		{
			if (!Cef.IsInitialized)
			{
				var isDefault = AppDomain.CurrentDomain.IsDefaultAppDomain();
				if (!isDefault)
				{
					throw new Exception(@"Add <add key=""xunit.appDomain"" value=""denied""/> to your app.config to disable appdomains");
				}

				var settings = new CefSettings();

				//The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
				//HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
				settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\Cache");

				Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
			}
		}

		public void Dispose()
		{
			if (Cef.IsInitialized)
			{
				Cef.Shutdown();
			}
		}
	}
}
