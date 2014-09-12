// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "JavascriptRootObjectWrapper.h"
#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
	public ref class CefAppWrapper abstract : public DisposableResource
	{
	private:
		MCefRefPtr<CefAppUnmanagedWrapper> cefApp;

	internal:
		List<CefBrowserWrapper^>^ browserWrappers;
		
	public:        
		static CefAppWrapper^ Instance;

		CefAppWrapper();
		int Run();

		void Bind(JavascriptRootObject^ rootObject);

		virtual void OnBrowserCreated(CefBrowserWrapper^ cefBrowserWrapper) abstract;
		virtual IBrowserProcess^ CreateBrowserProxy() abstract;
	};
}
