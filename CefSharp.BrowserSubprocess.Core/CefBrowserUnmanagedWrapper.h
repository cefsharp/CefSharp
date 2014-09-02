// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"
#include "include/cef_runnable.h"
#include "include/cef_v8.h"

#include "TypeUtils.h"
#include "Stdafx.h"

using namespace CefSharp::Internals;
using namespace System;
using namespace System::ServiceModel;
using namespace System::Threading;

namespace CefSharp
{
    private class CefBrowserUnmanagedWrapper
    {
        CefRefPtr<CefBrowser> _cefBrowser;

    public:
        gcroot<AutoResetEvent^> WaitHandle;
        gcroot<Object^> EvaluateScriptResult;
        gcroot<String^> EvaluateScriptExceptionMessage;

        CefBrowserUnmanagedWrapper(CefRefPtr<CefBrowser> cefBrowser)
        {
            _cefBrowser = cefBrowser;
            WaitHandle = gcnew AutoResetEvent(false);
        }

        ~CefBrowserUnmanagedWrapper()
        {
            _cefBrowser = nullptr;
            EvaluateScriptResult = nullptr;
            EvaluateScriptExceptionMessage = nullptr;
            
            AutoResetEvent^ waithandle = WaitHandle;
            WaitHandle = nullptr;
            delete waithandle;
        }

        JavascriptResponse^ EvaluateScriptCallback(int64 frameId, CefString script)
        {
            auto frame = _cefBrowser->GetFrame(frameId);
            CefRefPtr<CefV8Context> context = frame->GetV8Context();

            if (context.get() && context->Enter())
            {
                try
                {
                    return EvaluateScriptInContext(context, script);
                }
                finally
                {
                    context->Exit();
                }
            }

            return nullptr;
        }

        JavascriptResponse^ EvaluateScriptInContext(CefRefPtr<CefV8Context> context, CefString script)
        {
            CefRefPtr<CefV8Value> result;
            CefRefPtr<CefV8Exception> exception;
            JavascriptResponse^ response = gcnew JavascriptResponse();

            response->Success = context->Eval(script, result, exception);
            if (response->Success)
            {
                response->Result = TypeUtils::ConvertFromCef(result);
            }
            else if (exception.get())
            {
                response->Message = StringUtils::ToClr(exception->GetMessage());
            }

            return response;
        }

        IMPLEMENT_REFCOUNTING(CefBrowserUnmanagedWrapper);
    };
}
