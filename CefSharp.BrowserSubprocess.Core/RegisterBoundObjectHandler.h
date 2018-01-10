
// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "RegisterBoundObjectRegistry.h"
#include "..\CefSharp.Core\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core\Internals\Serialization\Primitives.h"

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    private class RegisterBoundObjectHandler : public CefV8Handler
    {
    private:
        gcroot<RegisterBoundObjectRegistry^> _callbackRegistry;

    public:
        RegisterBoundObjectHandler(RegisterBoundObjectRegistry^ callbackRegistery)
        {
            _callbackRegistry = callbackRegistery;
        }

        bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception) OVERRIDE
        {
            auto context = CefV8Context::GetCurrentContext();
            if (context.get())
            {
                auto browser = context->GetBrowser();

                if (context.get() && context->Enter())
                {
                    try
                    {
                        auto global = context->GetGlobal();
                        auto promiseCreator = global->GetValue(CefAppUnmanagedWrapper::kPromiseCreatorFunction);

                        //this will create a promise and give us the reject/resolve functions {p: Promise, res: resolve(), rej: reject()}
                        auto promiseData = promiseCreator->ExecuteFunctionWithContext(context, nullptr, CefV8ValueList());

                        //return the promose
                        retval = promiseData->GetValue("p");

                        //References to the promise resolve and reject methods
                        auto resolve = promiseData->GetValue("res");
                        auto reject = promiseData->GetValue("rej");

                        auto callback = gcnew JavascriptAsyncMethodCallback(context, resolve, reject);

                        auto callbackId = _callbackRegistry->SaveMethodCallback(callback);

                        auto request = CefProcessMessage::Create(kJavascriptRootObjectRequest);
                        auto argList = request->GetArgumentList();
                        auto params = CefListValue::Create();

                        auto boundObjectRequired = false;

                        for (auto i = 0; i < arguments.size(); i++)
                        {
                            auto objectName = arguments[i]->GetStringValue();

                            //TODO: JSB Check if object already bound
                            //if (!global->GetValue(objectName).get())
                            {
                                boundObjectRequired = true;
                                params->SetString(i, objectName);
                            }								
                        }

                        //If objects already exist then we'll just do nothing
                        if (boundObjectRequired)
                        {
                            argList->SetInt(0, browser->GetIdentifier());
                            SetInt64(argList, 1, context->GetFrame()->GetIdentifier());
                            SetInt64(argList, 2, callbackId);
                            argList->SetList(3, params);

                            browser->SendProcessMessage(CefProcessId::PID_BROWSER, request);
                        }
                        else
                        {
                            resolve->ExecuteFunctionWithContext(context, CefV8Value::CreateBool(true), CefV8ValueList());
                        }
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


        IMPLEMENT_REFCOUNTING(RegisterBoundObjectHandler)
    };
}

