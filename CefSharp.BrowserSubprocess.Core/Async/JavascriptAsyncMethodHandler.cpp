// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptAsyncMethodHandler.h"
#include "../CefSharp.Core.Runtime/Internals/Messaging/Messages.h"
#include "../CefSharp.Core.Runtime/Internals/Serialization/Primitives.h"
#include "Serialization/V8Serialization.h"
#include "CefAppUnmanagedWrapper.h"

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;
using namespace CefSharp::BrowserSubprocess::Serialization;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        namespace Async
        {
            bool JavascriptAsyncMethodHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
            {
                auto context = CefV8Context::GetCurrentContext();
                auto frame = context->GetFrame();

                CefRefPtr<CefV8Value> promise = CefV8Value::CreatePromise();
                retval = promise;

                auto callback = gcnew JavascriptAsyncMethodCallback(context, promise);
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
