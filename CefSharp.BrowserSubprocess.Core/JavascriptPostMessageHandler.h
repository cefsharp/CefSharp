// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "..\CefSharp.Core.Runtime\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core.Runtime\Internals\Serialization\Primitives.h"
#include "Serialization\V8Serialization.h"

using namespace System;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    const CefString kPostMessage = CefString("PostMessage");
    const CefString kPostMessageCamelCase = CefString("postMessage");

    private class JavascriptPostMessageHandler : public CefV8Handler
    {
    private:
        gcroot<JavascriptCallbackRegistry^> _javascriptCallbackRegistry;

    public:
        JavascriptPostMessageHandler(JavascriptCallbackRegistry^ javascriptCallbackRegistry)
        {
            _javascriptCallbackRegistry = javascriptCallbackRegistry;
        }

        bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception) OVERRIDE
        {
            if (arguments.size() == 0 || arguments.size() > 1)
            {
                //TODO: Improve error message
                exception = "Only a single param is accepted";

                return true;
            }

            auto context = CefV8Context::GetCurrentContext();
            if (context.get())
            {
                auto frame = context->GetFrame();

                if (frame.get() && frame->IsValid() && context->Enter())
                {
                    try
                    {
                        auto global = context->GetGlobal();

                        auto request = CefProcessMessage::Create(kJavascriptMessageReceived);
                        auto argList = request->GetArgumentList();

                        auto params = CefListValue::Create();
                        SerializeV8Object(arguments[0], params, 0, _javascriptCallbackRegistry);

                        //We're only interested in the first param
                        if (params->GetSize() > 0)
                        {
                            argList->SetValue(0, params->GetValue(0));
                        }

                        frame->SendProcessMessage(CefProcessId::PID_BROWSER, request);

                        retval = CefV8Value::CreateNull();
                    }
                    finally
                    {
                        context->Exit();
                    }
                }
                else
                {
                    exception = "Unable to Enter Context";
                }
            }
            else
            {
                exception = "Unable to get current context";
            }


            return true;
        }

        IMPLEMENT_REFCOUNTING(JavascriptPostMessageHandler);
    };
}


