// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

using namespace System;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private class JavascriptPromiseResolverCatch : public CefV8Handler
        {
            int64_t _callbackId;
            bool _isJsCallback;

        public:
            JavascriptPromiseResolverCatch(int64_t callbackId, bool isJsCallback) : _callbackId(callbackId), _isJsCallback(isJsCallback)
            {

            }

            virtual bool Execute(const CefString& name,
                CefRefPtr<CefV8Value> object,
                const CefV8ValueList& arguments,
                CefRefPtr<CefV8Value>& retval,
                CefString& exception)
            {
                auto context = CefV8Context::GetCurrentContext();

                auto reason = arguments[0];
                CefString reasonString;

                if (reason->IsString())
                {
                    reasonString = reason->GetStringValue();
                }
                else
                {
                    //Convert value to String
                    auto strFunc = context->GetGlobal()->GetValue("String");
                    CefV8ValueList args;
                    args.push_back(reason);
                    auto strVal = strFunc->ExecuteFunction(nullptr, args);                    

                    reasonString = strVal->GetStringValue();
                }

                auto response = CefProcessMessage::Create(_isJsCallback ? kJavascriptCallbackResponse : kEvaluateJavascriptResponse);
                auto responseArgList = response->GetArgumentList();

                //Success
                responseArgList->SetBool(0, false);
                //Callback Id
                SetInt64(responseArgList, 1, _callbackId);
                responseArgList->SetString(2, reasonString);

                auto frame = context->GetFrame();

                frame->SendProcessMessage(CefProcessId::PID_BROWSER, response);

                return true;
            }

            IMPLEMENT_REFCOUNTINGM(JavascriptPromiseResolverCatch);
        };
    }
}
