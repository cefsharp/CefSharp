// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include <assert.h>
#include <vcclr.h>
#include "include/cef_runnable.h"
#include "Common.h"
#include "JavascriptProxy.h"
#include "JavascriptProxyService.h"
#include "SubprocessCefApp.h"

JavascriptProxyService* _service;
CefRefPtr<CefBrowser>* _browser;

// TODO: Don't duplicate these when we already have them in StringUtils.cpp.
CefString ToNative(String^ str)
{
	if (str == nullptr)
	{
		return CefString();
	}

	pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
	CefString cefStr(pStr);
	return cefStr;
}

void EvaluateScriptHelper(int frameId, CefString script)
{
	return;

	// TODO: Code below crashes for yet unknown reasons...
	assert(_browser != nullptr);
	auto frame = _browser->get()->GetFrame(frameId);

	assert(frame != nullptr);

	auto v8Context = frame->GetV8Context();

	if (v8Context.get() && v8Context->Enter())
	{
		CefRefPtr<CefV8Value> result;
		CefRefPtr<CefV8Exception> exception;

		v8Context->Eval("alert(10 + 20);", result, exception);
	}
}

namespace CefSharp
{
	namespace BrowserSubprocess
	{
		JavascriptProxy::JavascriptProxy()
		{
			_subprocessCefApp = SubprocessCefApp::GetInstance();
			_browser = &SubprocessCefApp::GetCefBrowser();
		}

		Object^ JavascriptProxy::EvaluateScript(int frameId, String^ script, double timeout)
		{
			CefString scriptCefString = ToNative(script);
			CefPostTask(CefThreadId::TID_RENDERER, NewCefRunnableFunction(EvaluateScriptHelper, frameId, scriptCefString));
			return "gurka";
		}
	}
}