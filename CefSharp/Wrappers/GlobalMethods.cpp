// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

// This class exposes "global" CEF methods.

#include "Stdafx.h"
#include "CefAppWrapper.h"
#include "include/capi/cef_app_capi.h"

using namespace System;

namespace CefSharp
{
	namespace Wrappers
	{
		public ref class GlobalMethods
		{
		public:
			static int CefExecuteProcess(IntPtr hInstance, CefAppWrapper^ cefApp)
			{
				CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());
				return ::CefExecuteProcess(cefMainArgs, *(CefRefPtr<CefApp>*)cefApp->GetUnmanaged());
			}
		};
	}
}