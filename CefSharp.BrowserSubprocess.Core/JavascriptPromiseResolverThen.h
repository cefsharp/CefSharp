// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "..\CefSharp.Core.Runtime\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core.Runtime\Internals\Serialization\Primitives.h"
#include "Serialization\V8Serialization.h"

using namespace System;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::BrowserSubprocess::Serialization;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private class JavascriptPromiseResolverThen : public CefV8Handler
        {
            int64_t _callbackId;
            bool _isJsCallback;

        public:
            JavascriptPromiseResolverThen(int64_t callbackId, bool isJsCallback) : _callbackId(callbackId), _isJsCallback(isJsCallback)
            {

            }

            virtual bool Execute(const CefString& name,
                CefRefPtr<CefV8Value> object,
                const CefV8ValueList& arguments,
                CefRefPtr<CefV8Value>& retval,
                CefString& exception)
            {
                auto response = CefProcessMessage::Create(_isJsCallback ? kJavascriptCallbackResponse : kEvaluateJavascriptResponse);

                auto responseArgList = response->GetArgumentList();

                //Success
                responseArgList->SetBool(0, true);
                //Callback Id
                SetInt64(responseArgList, 1, _callbackId);
                SerializeV8Object(arguments[0], responseArgList, 2, nullptr);

                auto context = CefV8Context::GetCurrentContext();

                auto frame = context->GetFrame();

                frame->SendProcessMessage(CefProcessId::PID_BROWSER, response);

                return true;
            }

            IMPLEMENT_REFCOUNTINGM(JavascriptPromiseResolverThen);
        };
    }
}
