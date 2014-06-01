// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "include/cef_browser.h"
#include "include/cef_runnable.h"
#include "CefTaskScheduler.h"

using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    private class CefBrowserUnmanagedWrapper
    {
        CefRefPtr<CefBrowser> _cefBrowser;

    public:
        CefBrowserUnmanagedWrapper(CefRefPtr<CefBrowser> cefBrowser)
        {
            _cefBrowser = cefBrowser;
        }

        ~CefBrowserUnmanagedWrapper()
        {
            _cefBrowser = nullptr;
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
                //EvaluateScriptExceptionMessage = "Failed to evaluate script";
            }
        }

        IMPLEMENT_REFCOUNTING(CefBrowserUnmanagedWrapper);
    };

    // "Master class" for wrapping everything that the CefSubprocess needs.
    ref class CefBrowserWrapper : CefBrowserBase
    {
        MCefRefPtr<CefBrowser> _cefBrowser;
        MCefRefPtr<CefBrowserUnmanagedWrapper> _unmanagedWrapper;

    public:

        CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser) :
            _cefBrowser(cefBrowser)
        {
            RenderThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_RENDERER));
            BrowserId = cefBrowser->GetIdentifier();
            _unmanagedWrapper = new CefBrowserUnmanagedWrapper(cefBrowser);
        }

        virtual void DoDispose(bool disposing) override
        {
            _cefBrowser = nullptr;
            _unmanagedWrapper = nullptr;
            CefBrowserBase::DoDispose(disposing);
        }

        virtual Object^ DoEvaluateScript(System::Int64 frameId, String^ script) override
        {
            return _unmanagedWrapper->EvaluateScriptCallback(frameId, StringUtils::ToNative(script));
        }
    };
}
