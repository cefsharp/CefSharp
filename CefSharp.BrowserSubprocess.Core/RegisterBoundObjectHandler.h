
// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "RegisterBoundObjectRegistry.h"
#include "..\CefSharp.Core\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core\Internals\Serialization\Primitives.h"

using namespace System;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    private class RegisterBoundObjectHandler : public CefV8Handler
    {
    private:
        gcroot<RegisterBoundObjectRegistry^> _callbackRegistry;
        gcroot<Dictionary<String^, JavascriptObject^>^> _javascriptObjects;
        gcroot<CefBrowserWrapper^> _browserWrapper;

    public:
        RegisterBoundObjectHandler(RegisterBoundObjectRegistry^ callbackRegistery, Dictionary<String^, JavascriptObject^>^ javascriptObjects, CefBrowserWrapper^ browserWrapper)
        {
            _callbackRegistry = callbackRegistery;
            _javascriptObjects = javascriptObjects;
            _browserWrapper = browserWrapper;
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

                        if (name == "IsObjectCached")
                        {
                            if (arguments.size() == 0 || arguments.size() > 1)
                            {
                                //TODO: Improve error message
                                exception = "Must specify the name of a single bound object to check the cache for";

                                return true;
                            }

                            auto objectName = arguments[0]->GetStringValue();
                            auto managedObjectName = StringUtils::ToClr(objectName);

                            //Check to see if the object name is within the cache
                            retval = CefV8Value::CreateBool(_javascriptObjects->ContainsKey(managedObjectName));
                        }
                        else if (name == "RemoveObjectFromCache")
                        {
                            if (arguments.size() == 0 || arguments.size() > 1)
                            {
                                //TODO: Improve error message
                                exception = "Must specify the name of a single bound object to remove from cache";

                                return true;
                            }

                            auto objectName = arguments[0]->GetStringValue();
                            auto managedObjectName = StringUtils::ToClr(objectName);

                            if (_javascriptObjects->ContainsKey(managedObjectName))
                            {
                                //Remove object from cache
                                retval = CefV8Value::CreateBool(_javascriptObjects->Remove(managedObjectName));
                            }
                            else
                            {
                                retval = CefV8Value::CreateBool(false);
                            }							
                        }
                        //TODO: Better name for this function
                        //TODO: Make this a const
                        else if (name == "DeleteBoundObject")
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

                            auto request = CefProcessMessage::Create(kJavascriptRootObjectRequest);
                            auto argList = request->GetArgumentList();
                            auto params = CefListValue::Create();

                            auto boundObjectRequired = false;
                            auto cachedObjects = gcnew List<JavascriptObject^>();

                            if (arguments.size() > 0)
                            {
                                for (auto i = 0; i < arguments.size(); i++)
                                {
                                    auto objectName = arguments[i]->GetStringValue();

                                    //Check if the object has already been bound
                                    if (!global->HasValue(objectName))
                                    {
                                        //If no matching object found then we'll add the object name to the list
                                        boundObjectRequired = true;
                                        params->SetString(i, objectName);

                                        auto managedObjectName = StringUtils::ToClr(objectName);

                                        JavascriptObject^ obj;
                                        if (_javascriptObjects->TryGetValue(managedObjectName, obj))
                                        {
                                            cachedObjects->Add(obj);
                                        }
                                    }								
                                }
                            }
                            else
                            {
                                //No objects names were specified so we default to makeing the request
                                boundObjectRequired = true;
                            }

                            if (boundObjectRequired)
                            {
                                //If the number of cached objects matches the number of args
                                //then we'll immediately bind the cached objects
                                if (cachedObjects->Count == (int)arguments.size())
                                {
                                    auto frame = context->GetFrame();
                                    if (frame.get())
                                    {
                                        if (Object::ReferenceEquals(_browserWrapper, nullptr))
                                        {
                                            callback->Fail("Browser wrapper is null and unable to bind objects");
                                        }
                                        else
                                        {
                                            //TODO: JSB This code is almost exactly duplicated in CefAppUnmangedWrapper
                                            //Need to extract into a common method
                                            auto rootObjectWrappers = _browserWrapper->JavascriptRootObjectWrappers;

                                            JavascriptRootObjectWrapper^ rootObject;
                                            if (!rootObjectWrappers->TryGetValue(frame->GetIdentifier(), rootObject))
                                            {
                                                rootObject = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier(), _browserWrapper->BrowserProcess);
                                                rootObjectWrappers->TryAdd(frame->GetIdentifier(), rootObject);
                                            }
                                        
                                            rootObject->Bind(cachedObjects, context->GetGlobal());

                                            //Response object has no Accessor or Interceptor
                                            auto response = CefV8Value::CreateObject(NULL, NULL);

                                            response->SetValue("Count", CefV8Value::CreateInt(cachedObjects->Count), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                            response->SetValue("Success", CefV8Value::CreateBool(true), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                            response->SetValue("Message", CefV8Value::CreateString("OK"), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                            callback->Success(response);

                                            //Send message notifying Browser Process of which objects were bound
                                            //We do this after the objects have been created in the V8Context to gurantee
                                            //they are accessible.
                                            auto msg = CefProcessMessage::Create(kJavascriptObjectsBoundInJavascript);
                                            auto args = msg->GetArgumentList();

                                            auto names = CefListValue::Create();

                                            for (auto i = 0; i < cachedObjects->Count; i++)
                                            {
                                                auto name = cachedObjects[i]->JavascriptName;
                                                names->SetString(i, StringUtils::ToNative(name));
                                            }

                                            args->SetList(0, names);

                                            browser->SendProcessMessage(CefProcessId::PID_BROWSER, msg);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    //Obtain a callbackId then send off the Request
                                    auto callbackId = _callbackRegistry->SaveMethodCallback(callback);

                                    argList->SetInt(0, browser->GetIdentifier());
                                    SetInt64(argList, 1, context->GetFrame()->GetIdentifier());
                                    SetInt64(argList, 2, callbackId);
                                    argList->SetList(3, params);

                                    browser->SendProcessMessage(CefProcessId::PID_BROWSER, request);
                                }
                            }
                            else
                            {
                                //Response object has no Accessor or Interceptor
                                auto response = CefV8Value::CreateObject(NULL, NULL);


                                //Objects already bound so we immediately resolve the Promise
                                response->SetValue("Success", CefV8Value::CreateBool(false), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                response->SetValue("Count", CefV8Value::CreateInt(0), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                response->SetValue("Message", CefV8Value::CreateString("Object(s) already bound"), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);

                                CefV8ValueList returnArgs;
                                returnArgs.push_back(response);
                                //If all the requested objects are bound then we immediately execute resolve
                                //with Success true and Count of 0
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

