// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using namespace System;

namespace CefSharp
{
	namespace Internals
	{
		namespace JavascriptBinding
		{
			public ref class JavascriptProxySupport
			{
			public:
				literal String^ BaseAddress = "net.pipe://localhost";

				static String^ GetServiceName(int parentProcessId, int browserId)
				{
					auto elements = gcnew array<String^>(3);
					elements[0] = "JavaScriptProxy";
					elements[1] = parentProcessId.ToString();
					elements[2] = browserId.ToString();

					return String::Join("_", elements);
				}
			};
		}
	}
}
