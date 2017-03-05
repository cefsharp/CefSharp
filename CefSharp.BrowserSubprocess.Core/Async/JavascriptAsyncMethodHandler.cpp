// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptAsyncMethodHandler.h"
#include "../CefSharp.Core/Internals/Messaging/Messages.h"
#include "../CefSharp.Core/Internals/Serialization/Primitives.h"
#include "Serialization/V8Serialization.h"

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            bool JavascriptAsyncMethodHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
            {
                auto context = CefV8Context::GetCurrentContext();
                auto browser = context->GetBrowser();
                //this will create a promise and give us the reject/resolve functions {p: Promise, res: resolve(), rej: reject()}
                auto promiseData = _promiseCreator->ExecuteFunctionWithContext(context, nullptr, CefV8ValueList());
                retval = promiseData->GetValue("p");

                auto resolve = promiseData->GetValue("res");
                auto reject = promiseData->GetValue("rej");
                auto callback = gcnew JavascriptAsyncMethodCallback(context, resolve, reject);
                auto callbackId = _methodCallbackSave->Invoke(callback);

                auto request = CefProcessMessage::Create(kJavascriptAsyncMethodCallRequest);
                auto argList = request->GetArgumentList();
                auto params = CefListValue::Create();
                for (auto i = 0; i < arguments.size(); i++)
                {
                    SerializeV8Object(arguments[i], params, i, _callbackRegistry);
                }

                SetInt64(argList, 0, context->GetFrame()->GetIdentifier());
                SetInt64(argList, 1, _objectId);
                SetInt64(argList, 2, callbackId);
                argList->SetString(3, name);
                argList->SetList(4, params);

                browser->SendProcessMessage(CefProcessId::PID_BROWSER, request);

                return true;
            }
        }
    }
}
