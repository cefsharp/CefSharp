// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
	public class ResourceHandler
	{
		public string MimeType { get; set; }
		public Stream Stream { get; set; }

		public static ResourceHandler FromFileName(string fileName)
		{
			return new ResourceHandler { Stream = File.OpenRead(fileName) };
		}
	}
}
