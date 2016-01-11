// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example
{
	public class SubBoundObject
	{
		public string SimpleProperty { get; set; }

		public SubBoundObject()
		{
			SimpleProperty = "This is a very simple property.";
		}

		public string GetMyType()
		{
			return "My Type is " + GetType();
		}

		public string EchoSimpleProperty()
		{
			return SimpleProperty;
		}
	}
}
