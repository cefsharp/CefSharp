// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

// This class exposes "global" CEF methods.

#include "Stdafx.h"
#include "include/cef_app.h"

using namespace System;

// TODO: Support the stuff that CefApp has. One important question is whether we go with the C++ API or make the (seemingly a bit
// strange) move to start using the C API instead. It may actually make it possible to write our code in a cleaner fashion. Worth
// thinking about.
public ref class WrappedCefApp
{
	CefRefPtr<CefApp>* cefRefPtr;

public:
	void* GetUnmanaged()
	{
		return cefRefPtr;
	}
};

public ref class GlobalMethods
{
public:
	static int CefExecuteProcess(IntPtr hInstance, WrappedCefApp^ cefApp)
	{
		CefMainArgs mainArgs((HINSTANCE)hInstance.ToPointer());
		return ::CefExecuteProcess(mainArgs, *(CefRefPtr<CefApp>*)cefApp->GetUnmanaged());
	}
};
