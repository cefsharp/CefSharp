// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "RegisterBoundObjectHandler.h"
#include "JavascriptRootObjectWrapper.h"
#include "Serialization\V8Serialization.h"
#include "Serialization\JsObjectsSerialization.h"
#include "Async\JavascriptAsyncMethodCallback.h"
#include "..\CefSharp.Core\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core\Internals\Serialization\Primitives.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    const CefString CefAppUnmanagedWrapper::kPromiseCreatorFunction = "cefsharp_CreatePromise";
    const CefString CefAppUnmanagedWrapper::kPromiseCreatorScript = ""
        "function cefsharp_CreatePromise() {"
        "   var result = {};"
        "   var promise = new Promise(function(resolve, reject) {"
        "       result.res = resolve; result.rej = reject;"
        "   });"
        "   result.p = promise;"
        "   return result;"
        "}";

    CefRefPtr<CefRenderProcessHandler> CefAppUnmanagedWrapper::GetRenderProcessHandler()
    {
        return this;
    };

    // CefRenderProcessHandler
    void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser)
    {
        auto wrapper = gcnew CefBrowserWrapper(browser);
        _onBrowserCreated->Invoke(wrapper);

        //Multiple CefBrowserWrappers created when opening popups
        _browserWrappers->TryAdd(browser->GetIdentifier(), wrapper);
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
        //Send a message to the browser processing signaling that OnContextCreated has been called
        //only param is the FrameId. Currently an IPC message is only sent for the main frame - will see
        //how viable this solution is and if it's worth expanding to sub/child frames.
        if (frame->IsMain())
        {
            auto contextCreatedMessage = CefProcessMessage::Create(kOnContextCreatedRequest);

            SetInt64(contextCreatedMessage->GetArgumentList(), 0, frame->GetIdentifier());

            browser->SendProcessMessage(CefProcessId::PID_BROWSER, contextCreatedMessage);
        }

        if (_legacyBindingEnabled)
        {
            if (_javascriptObjects->Count > 0)
            {
                auto rootObject = GetJsRootObjectWrapper(browser->GetIdentifier(), frame->GetIdentifier());
                if (rootObject != nullptr)
                {
                    rootObject->Bind(_javascriptObjects->Values, context->GetGlobal());
                }
            }
        }

        auto global = context->GetGlobal();

        auto cefSharpObj = CefV8Value::CreateObject(NULL, NULL);
        global->SetValue("CefSharp", cefSharpObj, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);

        auto browserWrapper = FindBrowserWrapper(browser->GetIdentifier());

        //TODO: JSB: Split functions into their own classes
        //Browser wrapper is only used for BindObjectAsync
        auto bindObjAsyncFunction = CefV8Value::CreateFunction("BindObjectAsync", new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, browserWrapper));
        cefSharpObj->SetValue("BindObjectAsync", bindObjAsyncFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

        auto unBindObFunction = CefV8Value::CreateFunction("DeleteBoundObject", new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, nullptr));
        cefSharpObj->SetValue("DeleteBoundObject", unBindObFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

        auto removeObjectFromCacheFunction = CefV8Value::CreateFunction("RemoveObjectFromCache", new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, nullptr));
        cefSharpObj->SetValue("RemoveObjectFromCache", removeObjectFromCacheFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

        auto isObjectCachedFunction = CefV8Value::CreateFunction("IsObjectCached", new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, nullptr));
        cefSharpObj->SetValue("IsObjectCached", isObjectCachedFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
    };

    void CefAppUnmanagedWrapper::OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        //Send a message to the browser processing signaling that OnContextReleased has been called
        //only param is the FrameId. Currently an IPC message is only sent for the main frame - will see
        //how viable this solution is and if it's worth expanding to sub/child frames.
        if (frame->IsMain())
        {
            auto contextReleasedMessage = CefProcessMessage::Create(kOnContextReleasedRequest);

            SetInt64(contextReleasedMessage->GetArgumentList(), 0, frame->GetIdentifier());

            browser->SendProcessMessage(CefProcessId::PID_BROWSER, contextReleasedMessage);
        }

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

        // Needed in the browser process to get the frame.
        SetInt64(list, 0, frame->GetIdentifier());

        // The node will be empty if an element loses focus but another one
        // doesn't gain focus. Only transfer information if the node is an
        // element.
        if (node != nullptr && node->IsElement())
        {
            // True when a node exists, false if it doesn't.
            list->SetBool(1, true);

            // Store the tag name.
            list->SetString(2, node->GetElementTagName());

            // Transfer the attributes in a Dictionary.
            auto attributes = CefDictionaryValue::Create();
            CefDOMNode::AttributeMap attributeMap;
            node->GetElementAttributes(attributeMap);
            for (auto iter : attributeMap)
            {
                attributes->SetString(iter.first, iter.second);
            }

            list->SetDictionary(3, attributes);
        }
        else
        {
            list->SetBool(1, false);
        }

        browser->SendProcessMessage(CefProcessId::PID_BROWSER, focusedNodeChangedMessage);
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
            rootObject = gcnew JavascriptRootObjectWrapper(browserId, browserWrapper->BrowserProcess);
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

    bool CefAppUnmanagedWrapper::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message)
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

            auto callbackId = GetInt64(argList, 1);
            auto response = CefProcessMessage::Create(responseName);
            auto responseArgList = response->GetArgumentList();
            auto errorMessage = String::Format("Request BrowserId : {0} not found it's likely the browser is already closed", browser->GetIdentifier());

            //success: false
            responseArgList->SetBool(0, false);
            SetInt64(responseArgList, 1, callbackId);
            responseArgList->SetString(2, StringUtils::ToNative(errorMessage));
            browser->SendProcessMessage(sourceProcessId, response);

            return true;
        }
    
        //these messages are roughly handled the same way
        if (name == kEvaluateJavascriptRequest || name == kJavascriptCallbackRequest)
        {
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

            //both messages have the frameId stored at 0 and callbackId stored at index 1
            auto frameId = GetInt64(argList, 0);
            int64 callbackId = GetInt64(argList, 1);

            if (name == kEvaluateJavascriptRequest)
            {

                JavascriptRootObjectWrapper^ rootObjectWrapper;
                browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frameId, rootObjectWrapper);
                
                //NOTE: In the rare case when when OnContextCreated hasn't been called we need to manually create the rootObjectWrapper
                //It appears that OnContextCreated is only called for pages that have javascript on them, which makes sense
                //as without javascript there is no need for a context.
                if (rootObjectWrapper == nullptr)
                {
                    rootObjectWrapper = gcnew JavascriptRootObjectWrapper(browser->GetIdentifier(), browserWrapper->BrowserProcess);

                    browserWrapper->JavascriptRootObjectWrappers->TryAdd(frameId, rootObjectWrapper);
                }

                auto callbackRegistry = rootObjectWrapper->CallbackRegistry;

                auto script = argList->GetString(2);
                auto scriptUrl = argList->GetString(3);
                auto startLine = argList->GetInt(4);

                auto frame = browser->GetFrame(frameId);
                if (frame.get())
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
                                auto responseArgList = response->GetArgumentList();
                                SerializeV8Object(result, responseArgList, 2, callbackRegistry);
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
                    auto jsCallbackId = GetInt64(argList, 2);

                    auto callbackWrapper = callbackRegistry->FindWrapper(jsCallbackId);
                    if (callbackWrapper == nullptr)
                    {
                        errorMessage = "Unable to find callbackWrapper";
                    }
                    else
                    {
                        auto context = callbackWrapper->GetContext();
                        auto value = callbackWrapper->GetValue();
                
                        if (context.get() && context->Enter())
                        {
                            try
                            {
                                auto parameterList = argList->GetList(3);
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

            auto responseArgList = response->GetArgumentList();
            responseArgList->SetBool(0, success);
            SetInt64(responseArgList, 1, callbackId);
            if (!success)
            {
                responseArgList->SetString(2, errorMessage);
            }
            browser->SendProcessMessage(sourceProcessId, response);

            handled = true;
        }
        else if (name == kJavascriptCallbackDestroyRequest)
        {
            auto jsCallbackId = GetInt64(argList, 0);
            auto frameId = GetInt64(argList, 1);
            JavascriptRootObjectWrapper^ rootObjectWrapper;
            browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frameId, rootObjectWrapper);
            if (rootObjectWrapper != nullptr && rootObjectWrapper->CallbackRegistry != nullptr)
            {
                rootObjectWrapper->CallbackRegistry->Deregister(jsCallbackId);
            }

            handled = true;
        }
        else if (name == kJavascriptRootObjectResponse)
        {
            auto useLegacyBehaviour = argList->GetBool(0);

            //For the old legacy behaviour we add the objects
            //to the cache
            if (useLegacyBehaviour)
            {
                _legacyBindingEnabled = true;

                auto javascriptObjects = DeserializeJsObjects(argList, 4);

                for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(javascriptObjects))
                {
                    _javascriptObjects->Add(obj->JavascriptName, obj);
                }
            }
            else
            {
                auto browserId = argList->GetInt(1);
                auto frameId = GetInt64(argList, 2);
                auto callbackId = GetInt64(argList, 3);
                auto javascriptObjects = DeserializeJsObjects(argList, 4);

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

                auto frame = browser->GetFrame(frameId);
                if (frame.get())
                {
                    auto rootObject = GetJsRootObjectWrapper(browser->GetIdentifier(), frameId);

                    if (rootObject == nullptr)
                    {
                        return false;
                    }

                    auto context = frame->GetV8Context();

                    if (context.get() && context->Enter())
                    {
                        try
                        {
                            rootObject->Bind(javascriptObjects, context->GetGlobal());

                            JavascriptAsyncMethodCallback^ callback;
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

                                auto names = CefListValue::Create();

                                for (auto i = 0; i < javascriptObjects->Count; i++)
                                {
                                    auto name = javascriptObjects[i]->JavascriptName;
                                    names->SetString(i, StringUtils::ToNative(name));
                                }

                                args->SetList(0, names);

                                browser->SendProcessMessage(CefProcessId::PID_BROWSER, msg);
                            }
                        }
                        finally
                        {
                            context->Exit();
                        }
                    }
                }
            }

            handled = true;
        }
        else if (name == kJavascriptAsyncMethodCallResponse)
        {
            auto frameId = GetInt64(argList, 0);
            auto callbackId = GetInt64(argList, 1);

            JavascriptRootObjectWrapper^ rootObjectWrapper;
            browserWrapper->JavascriptRootObjectWrappers->TryGetValue(frameId, rootObjectWrapper);

            if (rootObjectWrapper != nullptr)
            {
                JavascriptAsyncMethodCallback^ callback;
                if (rootObjectWrapper->TryGetAndRemoveMethodCallback(callbackId, callback))
                {

                    try 
                    {
                        auto frame = browser->GetFrame(frameId);
                        if (frame.get())
                        {
                            auto context = frame->GetV8Context();

                            if (context.get() && context->Enter())
                            {
                                try
                                {
                                    auto success = argList->GetBool(2);
                                    if (success)
                                    {
                                        callback->Success(DeserializeV8Object(argList, 3));
                                    }
                                    else
                                    {
                                        callback->Fail(argList->GetString(3));
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
                    }
                    finally
                    {
                        //dispose
                        delete callback;
                    }
                }
            }
            handled = true;
        }

        return handled;
    };

    void CefAppUnmanagedWrapper::OnRenderThreadCreated(CefRefPtr<CefListValue> extraInfo)
    {
        //Check to see if we have a list
        if (extraInfo.get())
        {
            auto extensionList = extraInfo->GetList(0);
            if (extensionList.get())
            {
                for (size_t i = 0; i < extensionList->GetSize(); i++)
                {
                    auto extension = extensionList->GetList(i);
                    auto ext = gcnew CefExtension(StringUtils::ToClr(extension->GetString(0)), StringUtils::ToClr(extension->GetString(1)));

                    _extensions->Add(ext);
                }
            }
        }
    }

    void CefAppUnmanagedWrapper::OnWebKitInitialized()
    {
        //we need to do this because the builtin Promise object is not accesible
        CefRegisterExtension("cefsharp/promisecreator", kPromiseCreatorScript, NULL);

        for each(CefExtension^ extension in _extensions->AsReadOnly())
        {
            //only support extensions without handlers now
            CefRegisterExtension(StringUtils::ToNative(extension->Name), StringUtils::ToNative(extension->JavascriptCode), NULL);
        }
    }

    void CefAppUnmanagedWrapper::OnRegisterCustomSchemes(CefRawPtr<CefSchemeRegistrar> registrar)
    {
        for each (CefCustomScheme^ scheme in _schemes->AsReadOnly())
        {
            registrar->AddCustomScheme(StringUtils::ToNative(scheme->SchemeName), scheme->IsStandard, scheme->IsLocal, scheme->IsDisplayIsolated, scheme->IsSecure, scheme->IsCorsEnabled, scheme->IsCSPBypassing);
        }
    }
}