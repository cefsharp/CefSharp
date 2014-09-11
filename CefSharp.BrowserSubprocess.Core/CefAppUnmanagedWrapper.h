// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once


#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "JavascriptRootObjectWrapper.h"
#include "CefBrowserWrapper.h"
#include "CefAppWrapper.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
	private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
	{
	private:
		gcroot<Action<CefBrowserBase^>^> _onBrowserCreated;
		gcroot<JavascriptRootObjectWrapper^> _windowObject;
	public:
		
		CefAppUnmanagedWrapper(Action<CefBrowserBase^>^ onBrowserCreated)
		{
			_onBrowserCreated = onBrowserCreated;
		}

		virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE;
		virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
		virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
		void Bind(JavascriptRootObject^ rootObject);
		virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE;

		IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
	};
}
