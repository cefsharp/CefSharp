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

        Object^ EvaluateScriptCallback(int64 frameId, CefString script)
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

        Object^ EvaluateScriptInContext(CefRefPtr<CefV8Context> context, CefString script)
        {
            CefRefPtr<CefV8Value> result;
            CefRefPtr<CefV8Exception> exception;

            bool success = context->Eval(script, result, exception);
            if (success)
            {
                return TypeUtils::ConvertFromCef(result);
            }
            else if (exception.get())
            {
                throw gcnew ScriptException(StringUtils::ToClr(exception->GetMessage()));
            }
            else
            {
                return nullptr;
            }
        }

        IMPLEMENT_REFCOUNTING(CefBrowserUnmanagedWrapper);
    };
}
