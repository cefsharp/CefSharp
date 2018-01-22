
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
        gcroot<Dictionary<String^, JavascriptObject^>^> _javascriptObjects;

    public:
        RegisterBoundObjectHandler(RegisterBoundObjectRegistry^ callbackRegistery, Dictionary<String^, JavascriptObject^>^ javascriptObjects)
        {
            _callbackRegistry = callbackRegistery;
            _javascriptObjects = javascriptObjects;
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

                        //TODO: Better name for this function
                        //TODO: Make this a const
                        if (name == "DeleteBoundObject")
                        {
                            if (arguments.size() == 0 || arguments.size() > 1)
                            {
                                //TODO: Improve error message
                                exception = "Must specify the name of a bound object to unbind, one object at a time.";

                                return true;
                            }

                            auto objectName = arguments[0]->GetStringValue();

                            auto global = context->GetGlobal();

                            auto success = global->DeleteValue(objectName);

                            retval = CefV8Value::CreateBool(success);
                        }
                        else if (name == "BindObjectAsync")
                        {
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

                            if (arguments.size() > 0)
                            {
                                for (auto i = 0; i < arguments.size(); i++)
                                {
                                    auto objectName = arguments[i]->GetStringValue();
                                    auto managedObjectName = StringUtils::ToClr(objectName);

                                    //TODO: JSB Implement Caching of JavascriptObjects
                                    //if (_javascriptObjects->ContainsKey(managedObjectName))
                                    //{
                                    //    //We have a cached version of the required object so we won't request it again
                                    //	  //We will still need to sent through a list of cached object names so they can be bound from cache
                                    //    //The problem with caching will be that we can only call Promise.Resolve once, so we'll need to gather
                                    //    //all our bound data before we return
                                    //}
                                    //else
                                    //{

                                    //}

                                    if (!global->HasValue(objectName))
                                    {
                                        boundObjectRequired = true;
                                        params->SetString(i, objectName);
                                    }								
                                }
                            }
                            else
                            {
                                //No objects names were specified so we default to makeing the request
                                boundObjectRequired = true;
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
                                CefV8ValueList returnArgs;
                                returnArgs.push_back(CefV8Value::CreateBool(false));
                                //If all the requested objects are bound then we simply return false
                                resolve->ExecuteFunctionWithContext(context, nullptr, returnArgs);
                            }
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

