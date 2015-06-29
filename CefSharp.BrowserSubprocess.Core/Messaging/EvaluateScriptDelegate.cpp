// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "../CefSharp.Core/Internals/Messaging/Messages.h"
#include "../CefSharp.Core/Internals/Serialization/Primitives.h"

#include "Serialization/V8Serialization.h"
#include "../CefAppUnmanagedWrapper.h"
#include "EvaluateScriptDelegate.h"

using namespace CefSharp::Internals::Serialization;
    
namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            EvaluateScriptDelegate::EvaluateScriptDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper)
                :_appUnmanagedWrapper(appUnmanagedWrapper)
            {

            }

            bool EvaluateScriptDelegate::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message)
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

                    auto browserWrapper = _appUnmanagedWrapper->FindBrowserWrapper(browserId, true);
                    auto browser = browserWrapper->GetWrapped();
                    auto frame = browser->GetFrame(frameId);
                    if (browser.get() && frame.get())
                    {
                        auto context = frame->GetV8Context();

                        if (context.get() && context->Enter())
                        {
                            try
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
                                    SerializeV8Object(result, argList, 2, browserWrapper->CallbackRegistry);
                                }
                                else
                                {
                                    argList->SetString(2, exception->GetMessage());
                                }

                                if (response.get())
                                {
                                    browser->SendProcessMessage(sourceProcessId, response);
                                }
                            }
                            finally
                            {
                                context->Exit();
                            }
                        }
                    }
                    else
                    {
                        //TODO handle error
                    }

                    handled = true;
                }

                return handled;
            }
        }
    }
}