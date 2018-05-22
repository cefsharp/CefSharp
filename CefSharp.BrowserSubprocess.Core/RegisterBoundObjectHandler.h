
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
    const CefString kIsObjectCached = CefString("IsObjectCached");
    const CefString kIsObjectCachedCamelCase = CefString("isObjectCached");
    const CefString kRemoveObjectFromCache = CefString("RemoveObjectFromCache");
    const CefString kRemoveObjectFromCacheCamelCase = CefString("removeObjectFromCache");
    const CefString kDeleteBoundObject = CefString("DeleteBoundObject");
    const CefString kDeleteBoundObjectCamelCase = CefString("deleteBoundObject");
    const CefString kBindObjectAsync = CefString("BindObjectAsync");
    const CefString kBindObjectAsyncCamelCase = CefString("bindObjectAsync");

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

                        if (name == kIsObjectCached || name == kIsObjectCachedCamelCase)
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
                        else if (name == kRemoveObjectFromCache || name == kRemoveObjectFromCacheCamelCase)
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
                        else if (name == kDeleteBoundObject || name == kDeleteBoundObject)
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
                        else if (name == kBindObjectAsync || name == kBindObjectAsyncCamelCase)
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
                            auto notifyIfAlreadyBound = false;
                            auto ignoreCache = false;
                            auto cachedObjects = gcnew List<JavascriptObject^>();
                            //TODO: Create object to represent this information
                            auto objectNamesWithBoundStatus = gcnew List<Tuple<String^, bool, bool>^>();
                            auto objectCount = 0;

                            if (arguments.size() > 0)
                            {
                                objectCount = (int)arguments.size();

                                //If first argument is an object, we'll see if it contains config values
                                if (arguments[0]->IsObject())
                                {
                                    //TODO: Look at adding some sort of javascript mapping layer to reduce the code duplication
                                    if (arguments[0]->HasValue("NotifyIfAlreadyBound"))
                                    {
                                        auto notify = arguments[0]->GetValue("NotifyIfAlreadyBound");
                                        if (notify->IsBool())
                                        {
                                            notifyIfAlreadyBound = notify->GetBoolValue();
                                        }
                                    }

                                    if (arguments[0]->HasValue("IgnoreCache"))
                                    {
                                        auto ignore = arguments[0]->GetValue("IgnoreCache");
                                        if (ignore->IsBool())
                                        {
                                            ignoreCache = ignore->GetBoolValue();
                                        }
                                    }

                                    if (arguments[0]->HasValue("notifyIfAlreadyBound"))
                                    {
                                        auto notify = arguments[0]->GetValue("notifyIfAlreadyBound");
                                        if (notify->IsBool())
                                        {
                                            notifyIfAlreadyBound = notify->GetBoolValue();
                                        }
                                    }

                                    if (arguments[0]->HasValue("ignoreCache"))
                                    {
                                        auto ignore = arguments[0]->GetValue("ignoreCache");
                                        if (ignore->IsBool())
                                        {
                                            ignoreCache = ignore->GetBoolValue();
                                        }
                                    }

                                    //If we have a config object then we remove that from the count
                                    objectCount = objectCount - 1;
                                }

                                //Loop through all arguments and ignore anything that's not a string
                                for (auto i = 0; i < arguments.size(); i++)
                                {
                                    //Validate arg as being a string
                                    if(arguments[i]->IsString())
                                    {
                                        auto objectName = arguments[i]->GetStringValue();
                                        auto managedObjectName = StringUtils::ToClr(objectName);
                                        auto alreadyBound = global->HasValue(objectName);
                                        auto cached = false;

                                        //Check if the object has already been bound
                                        if (alreadyBound)
                                        {
                                            cached = _javascriptObjects->ContainsKey(managedObjectName);
                                        }
                                        else
                                        {
                                            //If no matching object found then we'll add the object name to the list
                                            boundObjectRequired = true;
                                            params->SetString(i, objectName);

                                            JavascriptObject^ obj;
                                            if (_javascriptObjects->TryGetValue(managedObjectName, obj))
                                            {
                                                cachedObjects->Add(obj);

                                                cached = true;
                                            }
                                        }

                                        objectNamesWithBoundStatus->Add(Tuple::Create(managedObjectName, alreadyBound, cached));
                                    }
                                }
                            }
                            else
                            {
                                //No objects names were specified so we default to makeing the request
                                boundObjectRequired = true;
                            }

                            if (boundObjectRequired || ignoreCache)
                            {
                                //If the number of cached objects matches the number of args
                                //(we have a cached copy of all requested objects)
                                //then we'll immediately bind the cached objects
                                if (cachedObjects->Count == objectCount && ignoreCache == false)
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
                                            auto rootObjectWrappers = _browserWrapper->JavascriptRootObjectWrappers;

                                            JavascriptRootObjectWrapper^ rootObject;
                                            if (!rootObjectWrappers->TryGetValue(frame->GetIdentifier(), rootObject))
                                            {
                                                rootObject = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier(), _browserWrapper->BrowserProcess);
                                                rootObjectWrappers->TryAdd(frame->GetIdentifier(), rootObject);
                                            }
                                        
                                            //Cached objects only contains a list of objects not already bound
                                            rootObject->Bind(cachedObjects, context->GetGlobal());

                                            //Response object has no Accessor or Interceptor
                                            auto response = CefV8Value::CreateObject(NULL, NULL);

                                            response->SetValue("Count", CefV8Value::CreateInt(cachedObjects->Count), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                            response->SetValue("Success", CefV8Value::CreateBool(true), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                            response->SetValue("Message", CefV8Value::CreateString("OK"), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                            callback->Success(response);

                                            NotifyObjectBound(browser, objectNamesWithBoundStatus);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    //Obtain a callbackId then send off the Request for objects
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
                                //Objects already bound or ignore cache

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

                                if (notifyIfAlreadyBound)
                                {
                                    NotifyObjectBound(browser, objectNamesWithBoundStatus);
                                }
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

    private:
        void NotifyObjectBound(const CefRefPtr<CefBrowser> browser, List<Tuple<String^, bool, bool>^>^ objectNamesWithBoundStatus)
        {
            //Send message notifying Browser Process of which objects were bound
            //We do this after the objects have been created in the V8Context to gurantee
            //they are accessible.
            auto msg = CefProcessMessage::Create(kJavascriptObjectsBoundInJavascript);
            auto args = msg->GetArgumentList();

            auto boundObjects = CefListValue::Create();
            auto index = 0;

            for each(auto obj in objectNamesWithBoundStatus)
            {
                auto dict = CefDictionaryValue::Create();

                auto name = obj->Item1;
                auto alreadyBound = obj->Item2;
                auto isCached = obj->Item3;
                dict->SetString("Name", StringUtils::ToNative(name));
                dict->SetBool("IsCached", isCached);
                dict->SetBool("AlreadyBound", alreadyBound);

                boundObjects->SetDictionary(index++, dict);
            }

            args->SetList(0, boundObjects);

            browser->SendProcessMessage(CefProcessId::PID_BROWSER, msg);
        }


        IMPLEMENT_REFCOUNTING(RegisterBoundObjectHandler)
    };
}

