// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefSubprocessWrapper.h"
#include "include/cef_app.h"

namespace CefSharp
{
	namespace Wrappers
	{
		class CefAppUnmanagedWrapper;

		public ref class CefAppWrapper
		{
		private:
			CefRefPtr<CefAppUnmanagedWrapper>* cefApp;
			bool is_disposed;

		public:
			CefAppWrapper();
			~CefAppWrapper();
			void* GetUnmanaged();
			virtual void OnBrowserCreated(CefSubprocessWrapper^ cefBrowserWrapper) {};

		private:
			!CefAppWrapper();
		};

		private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
		{
		private:
			gcroot<CefAppWrapper^> _cefAppWrapper;
			CefRefPtr<CefBrowser> _browser;

		public:
			CefAppUnmanagedWrapper(CefAppWrapper^ cefAppWrapper)
			{
				_cefAppWrapper = cefAppWrapper;
			}

			virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE
			{
				return this;
			}

			virtual DECL void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE
			{
				// TODO: Could destroy this CefBrowserWrapper in OnBrowserDestroyed(), but it doesn't seem to be reliably called...
				_cefAppWrapper->OnBrowserCreated(gcnew CefSubprocessWrapper(browser));
			}

			IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
		};
	}
}
