// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "include/cef_browser.h"
#include "include/cef_runnable.h"

using namespace System::Threading;

namespace CefSharp
{
	namespace Wrappers
	{
		private class CefBrowserUnmanagedWrapper
		{
			CefRefPtr<CefBrowser> _cefBrowser;

		public:
			CefRefPtr<CefFrame> Frame;
			gcroot<AutoResetEvent^> WaitHandle;

			CefBrowserUnmanagedWrapper(CefRefPtr<CefBrowser> cefBrowser)
			{
				_cefBrowser = cefBrowser;
				WaitHandle = gcnew AutoResetEvent(false);
			}

			void GetFrameCallback(int64 frameId)
			{
				Frame = _cefBrowser->GetFrame(frameId);
				WaitHandle->Set();
			}

			IMPLEMENT_REFCOUNTING(CefBrowserUnmanagedWrapper);
		};

		public ref class CefBrowserWrapper
		{
			CefRefPtr<CefBrowser>* _cefBrowser;
			int _browserId;
			CefRefPtr<CefBrowserUnmanagedWrapper>* _unmanagedWrapper;

		public:
			property int BrowserId
			{
				int get() { return _browserId; }
				private: void set(int value) { _browserId = value; }
			}

			CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser)
			{
				_cefBrowser = &cefBrowser;
				_browserId = cefBrowser->GetIdentifier();

				// TODO: Should be deallocated at some point to avoid leaking memory.
				_unmanagedWrapper = new CefRefPtr<CefBrowserUnmanagedWrapper>(new CefBrowserUnmanagedWrapper(cefBrowser));
			}

			System::IntPtr GetFrame(long frameId)
			{
				// TODO: Could we do something genericly useful here using C++ lambdas? To avoid having to make a lot of of these...
				// TODO: DON'T USE AUTORESETEVENT STUPIDITY! Even though the code below compiles & runs correctly, it deadlocks the
				// thread from which the request came, which is very, very stupid, especially since V8 and Chromium are built
				// with asynchrony in mind. Instead, we should re-think this API to utilize WCF callbacks instead:
				// http://idunno.org/archive/2008/05/29/wcf-callbacks-a-beginners-guide.aspx
				// That feels much more like 2013, and not 1994... :)
				CefPostTask(CefThreadId::TID_RENDERER, NewCefRunnableMethod(_unmanagedWrapper->get(), &CefBrowserUnmanagedWrapper::GetFrameCallback, frameId));
				_unmanagedWrapper->get()->WaitHandle->WaitOne();				

				// TODO: Wrap in a CefFrameWrapper, obviously...
				return (System::IntPtr)&_unmanagedWrapper->get()->Frame;
			}
		};
	}
}
