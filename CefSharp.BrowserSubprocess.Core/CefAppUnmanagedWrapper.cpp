// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "RegisterBoundObjectHandler.h"
#include "BindObjectAsyncHandler.h"
#include "JavascriptPostMessageHandler.h"
#include "JavascriptRootObjectWrapper.h"
#include "JavascriptPromiseHandler.h"
#include "Async\JavascriptAsyncMethodCallback.h"
#include "Serialization\V8Serialization.h"
#include "Serialization\JsObjectsSerialization.h"
#include "Wrapper\V8Context.h"
#include "Wrapper\Frame.h"
#include "Wrapper\Browser.h"
#include "..\CefSharp.Core\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core\Internals\Serialization\Primitives.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;
using namespace CefSharp::BrowserSubprocess;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    const CefString CefAppUnmanagedWrapper::kPromiseCreatorScript = ""
        "(function()"
        "{"
        "   var result = {};"
        "   var promise = new Promise(function(resolve, reject) {"
        "       result.res = resolve; result.rej = reject;"
        "   });"
        "   result.p = promise;"
        "   return result;"
        "})();";

    const CefString kRenderProcessId = CefString("RenderProcessId");
    const CefString kRenderProcessIdCamelCase = CefString("renderProcessId");

    CefRefPtr<CefRenderProcessHandler> CefAppUnmanagedWrapper::GetRenderProcessHandler()
    {
        return this;
    };

    // CefRenderProcessHandler
    void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDictionaryValue> extraInfo)
    {
        auto wrapper = gcnew CefBrowserWrapper(browser);
        _onBrowserCreated->Invoke(wrapper);

        //Multiple CefBrowserWrappers created when opening popups
        _browserWrappers->TryAdd(browser->GetIdentifier(), wrapper);

        //For the main browser only we check LegacyBindingEnabled and
        //load the objects. Popups don't send this information and checking
        //will override the _legacyBindingEnabled field
        if (!browser->IsPopup())
        {
            _legacyBindingEnabled = extraInfo->GetBool("LegacyBindingEnabled");

            if (_legacyBindingEnabled)
            {
                auto objects = extraInfo->GetList("LegacyBindingObjects");
                if (objects.get() && objects->IsValid())
                {
                    auto javascriptObjects = DeserializeJsObjects(objects, 0);

                    for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(javascriptObjects))
                    {
                        //Using LegacyBinding with multiple ChromiumWebBrowser instances that share the same
                        //render process and using LegacyBinding will cause problems for the limited caching implementation
                        //that exists at the moment, for now we'll remove an object if already exists, same behaviour
                        //as the new binding method. 
                        //TODO: This should be removed when https://github.com/cefsharp/CefSharp/issues/2306
                        //Is complete as objects will be stored at the browser level
                        if (_javascriptObjects->ContainsKey(obj->JavascriptName))
                        {
                            _javascriptObjects->Remove(obj->JavascriptName);
                        }
                        _javascriptObjects->Add(obj->JavascriptName, obj);
                    }
                }
            }
        }

        if (extraInfo->HasKey("JsBindingPropertyName") || extraInfo->HasKey("JsBindingPropertyNameCamelCase"))
        {
            //TODO: Create constant for these and legacy binding strings above
            _jsBindingPropertyName = extraInfo->GetString("JsBindingPropertyName");
            _jsBindingPropertyNameCamelCase = extraInfo->GetString("JsBindingPropertyNameCamelCase");
        }
        else
        {
            _jsBindingPropertyName = "CefSharp";
            _jsBindingPropertyNameCamelCase = "cefSharp";
        }
    }

    void CefAppUnmanagedWrapper::OnBrowserDestroyed(CefRefPtr<CefBrowser> browser)
    {
        CefBrowserWrapper^ wrapper;
        if (_browserWrappers->TryRemove(browser->GetIdentifier(), wrapper))
        {
            _onBrowserDestroyed->Invoke(wrapper);
            delete wrapper;
        }
    };

    void CefAppUnmanagedWrapper::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        if (!Object::ReferenceEquals(_handler, nullptr))
        {
            Browser browserWrapper(browser);
            Frame frameWrapper(frame);
            V8Context contextWrapper(context);

            _handler->OnContextCreated(%browserWrapper, %frameWrapper, %contextWrapper);
        }

        auto rootObject = GetJsRootObjectWrapper(browser->GetIdentifier(), frame->GetIdentifier());

        if (_legacyBindingEnabled)
        {
            if (_javascriptObjects->Count > 0 && rootObject != nullptr)
            {
                rootObject->Bind(_javascriptObjects->Values, context->GetGlobal());
            }
        }

        //TODO: Look at adding some sort of javascript mapping layer to reduce the code duplication
        auto global = context->GetGlobal();
        auto browserWrapper = FindBrowserWrapper(browser->GetIdentifier());
        auto processId = System::Diagnostics::Process::GetCurrentProcess()->Id;

        //TODO: JSB: Split functions into their own classes
        //Browser wrapper is only used for BindObjectAsync
        auto bindObjAsyncFunction = CefV8Value::CreateFunction(kBindObjectAsync, new BindObjectAsyncHandler(_registerBoundObjectRegistry, _javascriptObjects, browserWrapper));
        auto unBindObjFunction = CefV8Value::CreateFunction(kDeleteBoundObject, new RegisterBoundObjectHandler(_javascriptObjects));
        auto removeObjectFromCacheFunction = CefV8Value::CreateFunction(kRemoveObjectFromCache, new RegisterBoundObjectHandler(_javascriptObjects));
        auto isObjectCachedFunction = CefV8Value::CreateFunction(kIsObjectCached, new RegisterBoundObjectHandler(_javascriptObjects));
        auto postMessageFunction = CefV8Value::CreateFunction(kPostMessage, new JavascriptPostMessageHandler(rootObject == nullptr ? nullptr : rootObject->CallbackRegistry));
        auto promiseHandlerFunction = CefV8Value::CreateFunction(kSendEvalScriptResponse, new JavascriptPromiseHandler());

        //By default We'll support both CefSharp and cefSharp, for those who prefer the JS style
        auto createCefSharpObj = !_jsBindingPropertyName.empty();
        auto createCefSharpObjCamelCase = !_jsBindingPropertyNameCamelCase.empty();

        if (createCefSharpObj)
        {
            auto cefSharpObj = CefV8Value::CreateObject(NULL, NULL);
            cefSharpObj->SetValue(kBindObjectAsync, bindObjAsyncFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObj->SetValue(kDeleteBoundObject, unBindObjFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObj->SetValue(kRemoveObjectFromCache, removeObjectFromCacheFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObj->SetValue(kIsObjectCached, isObjectCachedFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObj->SetValue(kPostMessage, postMessageFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObj->SetValue(kSendEvalScriptResponse, promiseHandlerFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObj->SetValue(kRenderProcessId, CefV8Value::CreateInt(processId), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

            global->SetValue(_jsBindingPropertyName, cefSharpObj, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
        }

        if (createCefSharpObjCamelCase)
        {
            auto cefSharpObjCamelCase = CefV8Value::CreateObject(NULL, NULL);
            cefSharpObjCamelCase->SetValue(kBindObjectAsyncCamelCase, bindObjAsyncFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObjCamelCase->SetValue(kDeleteBoundObjectCamelCase, unBindObjFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObjCamelCase->SetValue(kRemoveObjectFromCacheCamelCase, removeObjectFromCacheFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObjCamelCase->SetValue(kIsObjectCachedCamelCase, isObjectCachedFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObjCamelCase->SetValue(kPostMessageCamelCase, postMessageFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObjCamelCase->SetValue(kSendEvalScriptResponseCamelCase, promiseHandlerFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
            cefSharpObjCamelCase->SetValue(kRenderProcessIdCamelCase, CefV8Value::CreateInt(processId), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

            global->SetValue(_jsBindingPropertyNameCamelCase, cefSharpObjCamelCase, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
        }

        //Send a message to the browser processing signaling that OnContextCreated has been called
        //only param is the FrameId. Previous sent only for main frame, now sent for all frames
        //Message sent after legacy objects have been bound and the CefSharp bind async helper methods
        //have been created
        auto contextCreatedMessage = CefProcessMessage::Create(kOnContextCreatedRequest);

        frame->SendProcessMessage(CefProcessId::PID_BROWSER, contextCreatedMessage);
    };

    void CefAppUnmanagedWrapper::OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        if (!Object::ReferenceEquals(_handler, nullptr))
        {
            Browser browserWrapper(browser);
            Frame frameWrapper(frame);
            V8Context contextWrapper(context);

            _handler->OnContextReleased(%browserWrapper, %frameWrapper, %contextWrapper);
        }

        auto contextReleasedMessage = CefProcessMessage::Create(kOnContextReleasedRequest);

        frame->SendProcessMessage(CefProcessId::PID_BROWSER, contextReleasedMessage);

        auto browserWrapper = FindBrowserWrapper(browser->GetIdentifier());

        //If we no longer have a browser wrapper reference then there's nothing we can do
        if (browserWrapper == nullptr)
        {
            return;
        }

        auto rootObjectWrappers = browserWrapper->JavascriptRootObjectWrappers;

        JavascriptRootObjectWrapper^ wrapper;
        if (rootObjectWrappers->TryRemove(frame->GetIdentifier(), wrapper))
        {
            delete wrapper;
        }
    };

    void CefAppUnmanagedWrapper::OnFocusedNodeChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefDOMNode> node)
    {
        if (!_focusedNodeChangedEnabled)
        {
            return;
        }

        auto focusedNodeChangedMessage = CefProcessMessage::Create(kOnFocusedNodeChanged);
        auto list = focusedNodeChangedMessage->GetArgumentList();

        // The node will be empty if an element loses focus but another one
        // doesn't gain focus. Only transfer information if the node is an
        // element.
        if (node != nullptr && node->IsElement())
        {
            // True when a node exists, false if it doesn't.
            list->SetBool(0, true);

            // Store the tag name.
            list->SetString(1, node->GetElementTagName());

            // Transfer the attributes in a Dictionary.
            auto attributes = CefDictionaryValue::Create();
            CefDOMNode::AttributeMap attributeMap;
            node->GetElementAttributes(attributeMap);
            for (auto iter : attributeMap)
            {
                attributes->SetString(iter.first, iter.second);
            }

            list->SetDictionary(2, attributes);
        }
        else
        {
            list->SetBool(0, false);
        }

        frame->SendProcessMessage(CefProcessId::PID_BROWSER, focusedNodeChangedMessage);
    }

    void CefAppUnmanagedWrapper::OnUncaughtException(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Exception> exception, CefRefPtr<CefV8StackTrace> stackTrace)
    {
        auto uncaughtExceptionMessage = CefProcessMessage::Create(kOnUncaughtException);
        auto list = uncaughtExceptionMessage->GetArgumentList();

        list->SetString(0, exception->GetMessage());

        auto frames = CefListValue::Create();
        for (auto i = 0; i < stackTrace->GetFrameCount(); i++)
        {
            auto stackTraceFrame = CefListValue::Create();
            auto frameArg = stackTrace->GetFrame(i);

            stackTraceFrame->SetString(0, frameArg->GetFunctionName());
            stackTraceFrame->SetInt(1, frameArg->GetLineNumber());
            stackTraceFrame->SetInt(2, frameArg->GetColumn());
            stackTraceFrame->SetString(3, frameArg->GetScriptNameOrSourceURL());

            frames->SetList(i, stackTraceFrame);
        }

        list->SetList(1, frames);

        frame->SendProcessMessage(CefProcessId::PID_BROWSER, uncaughtExceptionMessage);
    }

    JavascriptRootObjectWrapper^ CefAppUnmanagedWrapper::GetJsRootObjectWrapper(int browserId, int64 frameId)
    {
        auto browserWrapper = FindBrowserWrapper(browserId);

        if (browserWrapper == nullptr)
        {
            return nullptr;
        }

        auto rootObjectWrappers = browserWrapper->JavascriptRootObjectWrappers;

        JavascriptRootObjectWrapper^ rootObject;
        if (!rootObjectWrappers->TryGetValue(frameId, rootObject))
        {
#ifdef NETCOREAPP
            rootObject = gcnew JavascriptRootObjectWrapper(browserId);
#else
            rootObject = gcnew JavascriptRootObjectWrapper(browserId, browserWrapper->BrowserProcess);
#endif
            rootObjectWrappers->TryAdd(frameId, rootObject);
        }

        return rootObject;
    }

    CefBrowserWrapper^ CefAppUnmanagedWrapper::FindBrowserWrapper(int browserId)
    {
        CefBrowserWrapper^ wrapper = nullptr;

        _browserWrappers->TryGetValue(browserId, wrapper);

        if (wrapper == nullptr)
        {
            //TODO: Find the syntax for delcaring the native string directly
            LOG(ERROR) << StringUtils::ToNative("Failed to identify BrowserWrapper in OnContextCreated BrowserId:" + browserId).ToString();
        }

        return wrapper;
    }

    bool CefAppUnmanagedWrapper::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message)
    {
        auto handled = false;
        auto name = message->GetName();
        auto argList = message->GetArgumentList();

        auto browserWrapper = FindBrowserWrapper(browser->GetIdentifier());
        //Error handling for missing/closed browser
        if (browserWrapper == nullptr)
        {
            if (name == kJavascriptCallbackDestroyRequest ||
                name == kJavascriptRootObjectResponse ||
                name == kJavascriptAsyncMethodCallResponse)
            {
                //If we can't find the browser wrapper then we'll just
                //ignore this as it's likely already been disposed of
                return true;
            }

            CefString responseName;
            if (name == kEvaluateJavascriptRequest)
            {
                responseName = kEvaluateJavascriptResponse;
            }
            else if (name == kJavascriptCallbackRequest)
            {
                responseName = kJavascriptCallbackResponse;
            }
            else
            {
                //TODO: Should be throw an exception here? It's likely that only a CefSharp developer would see this
                // when they added a new message and haven't yet implemented the render process functionality.
                throw gcnew Exception("Unsupported message type");
            }

            auto callbackId = GetInt64(argList, 0);
            auto response = CefProcessMessage::Create(responseName);
            auto responseArgList = response->GetArgumentList();
            auto errorMessage = String::Format("Request BrowserId : {0} not found it's likely the browser is already closed", browser->GetIdentifier());

            //success: false
            responseArgList->SetBool(0, false);
            SetInt64(responseArgList, 1, callbackId);
            responseArgList->SetString(2, StringUtils::ToNative(errorMessage));
            frame->SendProcessMessage(sourceProcessId, response);

            return true;
        }

        //these messages are roughly handled the same way
        if (name == kEvaluateJavascriptRequest || name == kJavascriptCallbackRequest)
        {
            bool sendResponse = true;
            bool success = false;
            CefRefPtr<CefV8Value> result;
            CefString errorMessage;
            CefRefPtr<CefProcessMessage> response;

            if (name == kEvaluateJavascriptRequest)
            {
                response = CefProcessMessage::Create(kEvaluateJavascriptResponse);
            }
            else
            {
                response = CefProcessMessage::Create(kJavascriptCallbackResponse);
            }

            //both messages have callbackId stored at index 0
            auto frameId = frame->GetIdentifier();
            int64 callbackId = GetInt64(argList, 0);

            if (name == kEvaluateJavascriptRequest)
            {
                JavascriptRootObjectWrapper^ rootObjectWrapper;
                browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frameId, rootObjectWrapper);

                //NOTE: In the rare case when when OnContextCreated hasn't been called we need to manually create the rootObjectWrapper
                //It appears that OnContextCreated is only called for pages that have javascript on them, which makes sense
                //as without javascript there is no need for a context.
                if (rootObjectWrapper == nullptr)
                {
#ifdef NETCOREAPP
                    rootObjectWrapper = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier());
#else
                    rootObjectWrapper = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier(), browserWrapper->BrowserProcess);
#endif

                    browserWrapper->JavascriptRootObjectWrappers->TryAdd(frameId, rootObjectWrapper);
                }

                auto callbackRegistry = rootObjectWrapper->CallbackRegistry;

                auto script = argList->GetString(1);
                auto scriptUrl = argList->GetString(2);
                auto startLine = argList->GetInt(3);

                if (frame.get() && frame->IsValid())
                {
                    auto context = frame->GetV8Context();

                    if (context.get() && context->Enter())
                    {
                        try
                        {
                            CefRefPtr<CefV8Exception> exception;
                            success = context->Eval(script, scriptUrl, startLine, result, exception);

                            //we need to do this here to be able to store the v8context
                            if (success)
                            {
                                //If the response is a string of CefSharpDefEvalScriptRes then
                                //we don't send the response, we'll let that happen when the promise has completed.
                                if (result->IsString() && result->GetStringValue() == "CefSharpDefEvalScriptRes")
                                {
                                    sendResponse = false;
                                }
                                else
                                {
                                    auto responseArgList = response->GetArgumentList();
                                    SerializeV8Object(result, responseArgList, 2, callbackRegistry);
                                }
                            }
                            else
                            {
                                errorMessage = StringUtils::CreateExceptionString(exception);
                            }
                        }
                        finally
                        {
                            context->Exit();
                        }
                    }
                    else
                    {
                        errorMessage = "Unable to Enter Context";
                    }
                }
                else
                {
                    errorMessage = StringUtils::ToNative("Frame " + frameId + " is no longer available, most likely the Frame has been Disposed or Removed.");
                }
            }
            else
            {
                JavascriptRootObjectWrapper^ rootObjectWrapper;
                browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frameId, rootObjectWrapper);
                auto callbackRegistry = rootObjectWrapper == nullptr ? nullptr : rootObjectWrapper->CallbackRegistry;
                if (callbackRegistry == nullptr)
                {
                    errorMessage = StringUtils::ToNative("The callback registry for Frame " + frameId + " is no longer available, most likely the Frame has been Disposed.");
                }
                else
                {
                    auto jsCallbackId = GetInt64(argList, 1);

                    auto callbackWrapper = callbackRegistry->FindWrapper(jsCallbackId);
                    if (callbackWrapper == nullptr)
                    {
                        errorMessage = StringUtils::ToNative("Unable to find JavascriptCallback with Id " + jsCallbackId + " for Frame " + frameId);
                    }
                    else
                    {
                        auto context = callbackWrapper->GetContext();
                        auto value = callbackWrapper->GetValue();

                        if (context.get() && context->Enter())
                        {
                            try
                            {
                                auto parameterList = argList->GetList(2);
                                CefV8ValueList params;

                                //Needs to be called within the context as for Dictionary (mapped to struct)
                                //a V8Object will be created
                                for (CefV8ValueList::size_type i = 0; i < parameterList->GetSize(); i++)
                                {
                                    params.push_back(DeserializeV8Object(parameterList, static_cast<int>(i)));
                                }

                                result = value->ExecuteFunction(nullptr, params);
                                success = result.get() != nullptr;

                                //we need to do this here to be able to store the v8context
                                if (success)
                                {
                                    auto responseArgList = response->GetArgumentList();
                                    SerializeV8Object(result, responseArgList, 2, callbackRegistry);
                                }
                                else
                                {
                                    auto exception = value->GetException();
                                    errorMessage = StringUtils::CreateExceptionString(exception);
                                }
                            }
                            finally
                            {
                                context->Exit();
                            }
                        }
                        else
                        {
                            errorMessage = "Unable to Enter Context";
                        }
                    }
                }
            }

            if (sendResponse)
            {
                auto responseArgList = response->GetArgumentList();
                responseArgList->SetBool(0, success);
                SetInt64(responseArgList, 1, callbackId);
                if (!success)
                {
                    responseArgList->SetString(2, errorMessage);
                }
                frame->SendProcessMessage(sourceProcessId, response);
            }

            handled = true;
        }
        else if (name == kJavascriptCallbackDestroyRequest)
        {
            if (frame.get() && frame->IsValid())
            {
                auto jsCallbackId = GetInt64(argList, 0);
                JavascriptRootObjectWrapper^ rootObjectWrapper;
                browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frame->GetIdentifier(), rootObjectWrapper);
                if (rootObjectWrapper != nullptr && rootObjectWrapper->CallbackRegistry != nullptr)
                {
                    rootObjectWrapper->CallbackRegistry->Deregister(jsCallbackId);
                }
            }

            handled = true;
        }
        else if (name == kJavascriptRootObjectResponse)
        {
            if (browser.get() && frame.get() && frame->IsValid())
            {
                auto callbackId = GetInt64(argList, 0);
                auto javascriptObjects = DeserializeJsObjects(argList, 1);

                //Caching of JavascriptObjects
                //TODO: JSB Should caching be configurable? On a per object basis?
                for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(javascriptObjects))
                {
                    if (_javascriptObjects->ContainsKey(obj->JavascriptName))
                    {
                        _javascriptObjects->Remove(obj->JavascriptName);
                    }
                    _javascriptObjects->Add(obj->JavascriptName, obj);
                }

                auto rootObject = GetJsRootObjectWrapper(browser->GetIdentifier(), frame->GetIdentifier());

                if (rootObject == nullptr)
                {
                    return false;
                }

                auto context = frame->GetV8Context();

                if (context.get() && context->Enter())
                {
                    JavascriptAsyncMethodCallback^ callback;

                    try
                    {
                        rootObject->Bind(javascriptObjects, context->GetGlobal());

                        if (_registerBoundObjectRegistry->TryGetAndRemoveMethodCallback(callbackId, callback))
                        {
                            //Response object has no Accessor or Interceptor
                            auto response = CefV8Value::CreateObject(NULL, NULL);

                            response->SetValue("Count", CefV8Value::CreateInt(javascriptObjects->Count), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);

                            if (javascriptObjects->Count > 0)
                            {
                                //TODO: JSB Should we include a list of successfully bound object names?
                                response->SetValue("Success", CefV8Value::CreateBool(true), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                response->SetValue("Message", CefV8Value::CreateString("OK"), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                callback->Success(response);
                            }
                            else
                            {
                                response->SetValue("Success", CefV8Value::CreateBool(false), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                response->SetValue("Message", CefV8Value::CreateString("Zero objects bounds"), CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                                callback->Success(response);
                            }

                            //Send message notifying Browser Process of which objects were bound
                            //We do this after the objects have been created in the V8Context to gurantee
                            //they are accessible.
                            auto msg = CefProcessMessage::Create(kJavascriptObjectsBoundInJavascript);
                            auto args = msg->GetArgumentList();

                            auto boundObjects = CefListValue::Create();

                            for (auto i = 0; i < javascriptObjects->Count; i++)
                            {
                                auto dict = CefDictionaryValue::Create();
                                auto objectName = javascriptObjects[i]->JavascriptName;
                                dict->SetString("Name", StringUtils::ToNative(objectName));
                                dict->SetBool("IsCached", false);
                                dict->SetBool("AlreadyBound", false);

                                boundObjects->SetDictionary(i, dict);
                            }

                            args->SetList(0, boundObjects);

                            frame->SendProcessMessage(CefProcessId::PID_BROWSER, msg);
                        }
                    }
                    finally
                    {
                        context->Exit();

                        delete callback;
                    }
                }
            }
            else
            {
                LOG(INFO) << "CefAppUnmanagedWrapper Frame Invalid";
            }

            handled = true;
        }
        else if (name == kJavascriptAsyncMethodCallResponse)
        {
            if (frame.get() && frame->IsValid())
            {
                auto frameId = frame->GetIdentifier();
                auto callbackId = GetInt64(argList, 0);

                JavascriptRootObjectWrapper^ rootObjectWrapper;
                browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frameId, rootObjectWrapper);

                if (rootObjectWrapper != nullptr)
                {
                    JavascriptAsyncMethodCallback^ callback;
                    if (rootObjectWrapper->TryGetAndRemoveMethodCallback(callbackId, callback))
                    {
                        try
                        {
                            auto context = frame->GetV8Context();

                            if (context.get() && context->Enter())
                            {
                                try
                                {
                                    auto success = argList->GetBool(1);
                                    if (success)
                                    {
                                        callback->Success(DeserializeV8Object(argList, 2));
                                    }
                                    else
                                    {
                                        callback->Fail(argList->GetString(2));
                                    }
                                }
                                finally
                                {
                                    context->Exit();
                                }
                            }
                            else
                            {
                                callback->Fail("Unable to Enter Context");
                            }
                        }
                        finally
                        {
                            //dispose
                            delete callback;
                        }
                    }
                }
            }
            handled = true;
        }

        return handled;
    };

    void CefAppUnmanagedWrapper::OnWebKitInitialized()
    {
        if (!Object::ReferenceEquals(_handler, nullptr))
        {
            _handler->OnWebKitInitialized();
        }
    }
}
