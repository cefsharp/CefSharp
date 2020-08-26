// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptAsyncMethodHandler.h"
#include "../CefSharp.Core/Internals/Messaging/Messages.h"
#include "../CefSharp.Core/Internals/Serialization/Primitives.h"
#include "Serialization/V8Serialization.h"
#include "CefAppUnmanagedWrapper.h"

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
                auto frame = context->GetFrame();

                CefRefPtr<CefV8Value> promiseData;
                CefRefPtr<CefV8Exception> promiseException;
                //this will create a promise and give us the reject/resolve functions {p: Promise, res: resolve(), rej: reject()}
                if (!context->Eval(CefAppUnmanagedWrapper::kPromiseCreatorScript, CefString(), 0, promiseData, promiseException))
                {
                    LOG(WARNING) << "JavascriptAsyncMethodHandler::Execute promiseData returned exception: " + promiseException->GetMessage().ToString();

                    exception = promiseException->GetMessage();

                    return true;
                }

                //when refreshing the browser this is sometimes null, in this case return true and log message
                //https://github.com/cefsharp/CefSharp/pull/2446
                if (promiseData == NULL)
                {
                    LOG(WARNING) << "JavascriptAsyncMethodHandler::Execute promiseData returned NULL";

                    return true;
                }

                retval = promiseData->GetValue("p");

                auto resolve = promiseData->GetValue("res");
                auto reject = promiseData->GetValue("rej");
                auto callback = gcnew JavascriptAsyncMethodCallback(context, resolve, reject);
                auto callbackId = _methodCallbackSave->Invoke(callback);

                auto request = CefProcessMessage::Create(kJavascriptAsyncMethodCallRequest);
                auto argList = request->GetArgumentList();
                auto params = CefListValue::Create();
                for (size_t i = 0; i < arguments.size(); i++)
                {
                    SerializeV8Object(arguments[i], params, i, _callbackRegistry);
                }

                SetInt64(argList, 0, _objectId);
                SetInt64(argList, 1, callbackId);
                argList->SetString(2, name);
                argList->SetList(3, params);

                frame->SendProcessMessage(CefProcessId::PID_BROWSER, request);

                return true;
            }
        }
    }
}
