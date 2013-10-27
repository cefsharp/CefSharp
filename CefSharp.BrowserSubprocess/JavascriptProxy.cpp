// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Common.h"
#include "JavascriptProxy.h"
#include "SubprocessCefApp.h"

namespace CefSharp
{
	namespace BrowserSubprocess
	{
		JavascriptProxy::JavascriptProxy()
		{
			_subprocessCefApp = SubprocessCefApp::GetInstance();
		}

		Object^ JavascriptProxy::EvaluateScript(int browserId, int frameId, String^ script, double timeout)
		{
			return nullptr;

			//auto browser = _subprocessCefApp->GetBrowserById(browserId);
			//auto frame = browser->GetFrame(frameId);
			//auto v8Context = frame->GetV8Context();

			//if (v8Context.get() && v8Context->Enter())
			//{
			//	CefRefPtr<CefV8Value> result;
			//	CefRefPtr<CefV8Exception> exception;

			//	v8Context->Eval("10 + 20", result, exception);
			//}

			//return "gurka";
		}
	}
}