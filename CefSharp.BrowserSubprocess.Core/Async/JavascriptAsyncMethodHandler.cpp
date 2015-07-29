// Copyright � 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptAsyncMethodHandler.h"
#include "../CefSharp.Core/Internals/Messaging/Messages.h"
#include "../CefSharp.Core/Internals/Serialization/Primitives.h"
#include "Serialization/V8Serialization.h"
#include "CefBrowserWrapper.h"

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

                SetInt64(_objectId, argList, 0);
                SetInt64(callbackId, argList, 1);
                argList->SetString(2, name);
                argList->SetList(3, params);

                browser->SendProcessMessage(CefProcessId::PID_BROWSER, request);
                return true;
            }
        }
    }
}
