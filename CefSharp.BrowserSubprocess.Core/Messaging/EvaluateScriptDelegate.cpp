// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "../CefSharp.Core/Internals/Messaging/Messages.h"
#include "../CefSharp.Core/Internals/Serialization/Primitives.h"

#include "Serialization/V8Serialization.h"
#include "../CefAppUnmanagedWrapper.h"
#include "EvaluateScriptDelegate.h"
    
namespace CefSharp
{
    namespace Internals
    {
        using namespace Serialization;

        namespace Messaging
        {
            EvaluateScriptDelegate::EvaluateScriptDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper)
                :_appUnmanagedWrapper(appUnmanagedWrapper)
            {

            }

            bool EvaluateScriptDelegate::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
            {
                auto handled = false;
                auto name = message->GetName();
                if (name == kEvaluateJavascriptRequest)
                {
                    auto argList = message->GetArgumentList();
                    auto browserId = argList->GetInt(0);
                    auto frameId = argList->GetInt(1);
                    auto callbackId = GetInt64(argList, 2);
                    auto script = argList->GetString(3);

                    auto response = EvaluateScript(browserId, frameId, callbackId, script);
                    if (response.get())
                    {
                        browser->SendProcessMessage(source_process, response);
                    }

                    handled = true;
                }

                return handled;
            }

            CefRefPtr<CefProcessMessage> EvaluateScriptDelegate::EvaluateScript(int browserId, int frameId, int64 callbackId, CefString script)
            {
                CefRefPtr<CefProcessMessage> result;
                auto browserWrapper = _appUnmanagedWrapper->FindBrowserWrapper(browserId, true);
                auto browser = browserWrapper->GetWrapped();
                auto frame = browser->GetFrame(frameId);
                if (browser.get() && frame.get())
                {
                    result = EvaluateScriptInFrame(frame, callbackId, script, browserWrapper->CallbackRegistry);
                }
                else
                {
                    //TODO handle error
                }
                return result;
            }

            CefRefPtr<CefProcessMessage> EvaluateScriptDelegate::EvaluateScriptInFrame(CefRefPtr<CefFrame> frame, int64 callbackId, CefString script, JavascriptCallbackRegistry^ callbackRegistry)
            {
                CefRefPtr<CefProcessMessage> result;
                auto context = frame->GetV8Context();

                if (context.get() && context->Enter())
                {
                    try
                    {
                        result = EvaluateScriptInContext(context, callbackId, script, callbackRegistry);
                    }
                    finally
                    {
                        context->Exit();
                    }
                }

                return result;
            }

            CefRefPtr<CefProcessMessage> EvaluateScriptDelegate::EvaluateScriptInContext(CefRefPtr<CefV8Context> context, int64 callbackId, CefString script, JavascriptCallbackRegistry^ callbackRegistry)
            {
                CefRefPtr<CefV8Value> result;
                CefRefPtr<CefV8Exception> exception;
                auto success = context->Eval(script, result, exception);
                auto response = CefProcessMessage::Create(kEvaluateJavascriptResponse);
                auto argList = response->GetArgumentList();

                argList->SetBool(0, success);
                SetInt64(callbackId, argList, 1);
                if (success)
                {
                    SerializeV8Object(result, argList, 2, callbackRegistry);
                }
                else
                {
                    argList->SetString(2, exception->GetMessage());
                }

                return response;
            }

        }
    }
}