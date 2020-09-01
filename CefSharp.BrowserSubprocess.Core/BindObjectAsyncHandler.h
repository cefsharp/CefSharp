// Copyright © 2019 The CefSharp Authors. All rights reserved.
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
    const CefString kBindObjectAsync = CefString("BindObjectAsync");
    const CefString kBindObjectAsyncCamelCase = CefString("bindObjectAsync");

    private class BindObjectAsyncHandler : public CefV8Handler
    {
    private:
        gcroot<RegisterBoundObjectRegistry^> _callbackRegistry;
        gcroot<Dictionary<String^, JavascriptObject^>^> _javascriptObjects;
        gcroot<CefBrowserWrapper^> _browserWrapper;

    public:
        BindObjectAsyncHandler(RegisterBoundObjectRegistry^ callbackRegistery, Dictionary<String^, JavascriptObject^>^ javascriptObjects, CefBrowserWrapper^ browserWrapper)
        {
            _callbackRegistry = callbackRegistery;
            _javascriptObjects = javascriptObjects;
            _browserWrapper = browserWrapper;
        }

        ~BindObjectAsyncHandler()
        {
            _callbackRegistry = nullptr;
            _javascriptObjects = nullptr;
            _browserWrapper = nullptr;
        }

        bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception) OVERRIDE
        {
            auto context = CefV8Context::GetCurrentContext();

            if (context.get() && context->Enter())
            {
                try
                {
                    auto params = CefListValue::Create();
                    //We need to store a seperate index into our params as
                    //there are instances we skip over already cached objects
                    //and end up with empty strings in the list.
                    //e.g. first object is already bound/cached, we previously
                    //second object isn't we end up with a list of "", "secondObject"
                    int paramsIndex = 0;

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
                            //Upper and camelcase options are supported
                            notifyIfAlreadyBound = GetV8BoolValue(arguments[0], "NotifyIfAlreadyBound", "notifyIfAlreadyBound");
                            ignoreCache = GetV8BoolValue(arguments[0], "IgnoreCache", "ignoreCache");

                            //If we have a config object then we remove that from the count
                            objectCount = objectCount - 1;
                        }

                        auto global = context->GetGlobal();

                        //Loop through all arguments and ignore anything that's not a string
                        for (size_t i = 0; i < arguments.size(); i++)
                        {
                            //Validate arg as being a string
                            if (arguments[i]->IsString())
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
                                    params->SetString(paramsIndex++, objectName);

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

                    auto frame = context->GetFrame();

                    if (frame.get() && frame->IsValid())
                    {
                        if (boundObjectRequired || ignoreCache)
                        {
                            //If the number of cached objects matches the number of args
                            //(we have a cached copy of all requested objects)
                            //then we'll immediately bind the cached objects
                            if (cachedObjects->Count == objectCount && ignoreCache == false)
                            {
                                if (Object::ReferenceEquals(_browserWrapper, nullptr))
                                {
                                    exception = "BindObjectAsyncHandler::Execute - Browser wrapper null, unable to bind objects";

                                    return true;
                                }

                                auto browser = context->GetBrowser();

                                auto rootObjectWrappers = _browserWrapper->JavascriptRootObjectWrappers;

                                JavascriptRootObjectWrapper^ rootObject;
                                if (!rootObjectWrappers->TryGetValue(frame->GetIdentifier(), rootObject))
                                {
#ifdef NETCOREAPP
                                    rootObject = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier());
#else
                                    rootObject = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier(), _browserWrapper->BrowserProcess);
#endif
                                    rootObjectWrappers->TryAdd(frame->GetIdentifier(), rootObject);
                                }

                                //Cached objects only contains a list of objects not already bound
                                rootObject->Bind(cachedObjects, context->GetGlobal());

                                //Objects already bound or ignore cache
                                CefRefPtr<CefV8Value> promiseResolve;
                                CefRefPtr<CefV8Exception> promiseException;

                                auto promiseResolveScript = StringUtils::ToNative("Promise.resolve({Success:true, Count:" + cachedObjects->Count + ", Message:'OK'});");

                                if (context->Eval(promiseResolveScript, CefString(), 0, promiseResolve, promiseException))
                                {
                                    retval = promiseResolve;
                                }
                                else
                                {
                                    exception = promiseException->GetMessage();

                                    return true;
                                }

                                NotifyObjectBound(frame, objectNamesWithBoundStatus);
                            }
                            else
                            {
                                CefRefPtr<CefV8Value> promiseData;
                                CefRefPtr<CefV8Exception> promiseException;
                                //this will create a promise and give us the reject/resolve functions {p: Promise, res: resolve(), rej: reject()}
                                if (!context->Eval(CefAppUnmanagedWrapper::kPromiseCreatorScript, CefString(), 0, promiseData, promiseException))
                                {
                                    exception = promiseException->GetMessage();

                                    return true;
                                }

                                //when refreshing the browser this is sometimes null, in this case return true and log message
                                //https://github.com/cefsharp/CefSharp/pull/2446
                                if (promiseData == NULL)
                                {
                                    LOG(WARNING) << "BindObjectAsyncHandler::Execute promiseData returned NULL";

                                    return true;
                                }

                                //return the promose
                                retval = promiseData->GetValue("p");

                                //References to the promise resolve and reject methods
                                auto resolve = promiseData->GetValue("res");
                                auto reject = promiseData->GetValue("rej");

                                auto callback = gcnew JavascriptAsyncMethodCallback(context, resolve, reject);

                                auto request = CefProcessMessage::Create(kJavascriptRootObjectRequest);
                                auto argList = request->GetArgumentList();

                                //Obtain a callbackId then send off the Request for objects
                                auto callbackId = _callbackRegistry->SaveMethodCallback(callback);

                                SetInt64(argList, 0, callbackId);
                                argList->SetList(1, params);

                                frame->SendProcessMessage(CefProcessId::PID_BROWSER, request);
                            }
                        }
                        else
                        {
                            //Objects already bound or ignore cache
                            CefRefPtr<CefV8Value> promiseResolve;
                            CefRefPtr<CefV8Exception> promiseException;

                            auto promiseResolveScript = CefString("Promise.resolve({Success:false, Count:0, Message:'Object(s) already bound'});");

                            if (context->Eval(promiseResolveScript, CefString(), 0, promiseResolve, promiseException))
                            {
                                retval = promiseResolve;

                                if (notifyIfAlreadyBound)
                                {
                                    NotifyObjectBound(frame, objectNamesWithBoundStatus);
                                }
                            }
                            else
                            {
                                exception = promiseException->GetMessage();
                            }
                        }
                    }
                    else
                    {
                        exception = "BindObjectAsyncHandler::Execute - Frame is invalid.";
                    }
                }
                finally
                {
                    context->Exit();
                }
            }
            else
            {
                exception = "BindObjectAsyncHandler::Execute - Unable to Get or Enter Context";
            }


            return true;
        }

    private:
        void NotifyObjectBound(const CefRefPtr<CefFrame> frame, List<Tuple<String^, bool, bool>^>^ objectNamesWithBoundStatus)
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

            frame->SendProcessMessage(CefProcessId::PID_BROWSER, msg);
        }

        bool GetV8BoolValue(const CefRefPtr<CefV8Value> val, const CefString key, const CefString camelCaseKey)
        {
            if (val->HasValue(key))
            {
                auto obj = val->GetValue(key);
                if (obj->IsBool())
                {
                    return obj->GetBoolValue();
                }
            }

            if (val->HasValue(camelCaseKey))
            {
                auto obj = val->GetValue(camelCaseKey);
                if (obj->IsBool())
                {
                    return obj->GetBoolValue();
                }
            }

            return false;
        }


        IMPLEMENT_REFCOUNTING(BindObjectAsyncHandler);
    };
}

