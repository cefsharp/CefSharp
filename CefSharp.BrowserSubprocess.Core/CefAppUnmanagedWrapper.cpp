// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "RegisterBoundObjectHandler.h"
#include "JavascriptRootObjectWrapper.h"
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
        if (!Object::ReferenceEquals(_handler, nullptr))
        {
            Browser browserWrapper(browser);
            Frame frameWrapper(frame);
            V8Context contextWrapper(context);

            _handler->OnContextCreated(%browserWrapper, %frameWrapper, %contextWrapper);
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

        //TODO: Look at adding some sort of javascript mapping layer to reduce the code duplication
        auto global = context->GetGlobal();
        auto browserWrapper = FindBrowserWrapper(browser->GetIdentifier());

        auto cefSharpObj = CefV8Value::CreateObject(NULL, NULL);
        global->SetValue("CefSharp", cefSharpObj, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);

        //We'll support both CefSharp and cefSharp, for those who prefer the JS style
        auto cefSharpObjCamelCase = CefV8Value::CreateObject(NULL, NULL);
        global->SetValue("cefSharp", cefSharpObjCamelCase, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);

        //TODO: JSB: Split functions into their own classes
        //Browser wrapper is only used for BindObjectAsync
        auto bindObjAsyncFunction = CefV8Value::CreateFunction(kBindObjectAsync, new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, browserWrapper));
        auto unBindObjFunction = CefV8Value::CreateFunction(kDeleteBoundObject, new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, nullptr));
        auto removeObjectFromCacheFunction = CefV8Value::CreateFunction(kRemoveObjectFromCache, new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, nullptr));
        auto isObjectCachedFunction = CefV8Value::CreateFunction(kIsObjectCached, new RegisterBoundObjectHandler(_registerBoundObjectRegistry, _javascriptObjects, nullptr));

        cefSharpObj->SetValue(kBindObjectAsync, bindObjAsyncFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
        cefSharpObj->SetValue(kDeleteBoundObject, unBindObjFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
        cefSharpObj->SetValue(kRemoveObjectFromCache, removeObjectFromCacheFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
        cefSharpObj->SetValue(kIsObjectCached, isObjectCachedFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

        cefSharpObjCamelCase->SetValue(kBindObjectAsyncCamelCase, bindObjAsyncFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
        cefSharpObjCamelCase->SetValue(kDeleteBoundObjectCamelCase, unBindObjFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
        cefSharpObjCamelCase->SetValue(kRemoveObjectFromCacheCamelCase, removeObjectFromCacheFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
        cefSharpObjCamelCase->SetValue(kIsObjectCachedCamelCase, isObjectCachedFunction, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);

        //Send a message to the browser processing signaling that OnContextCreated has been called
        //only param is the FrameId. Previous sent only for main frame, now sent for all frames
        //Message sent after legacy objects have been bound and the CefSharp bind async helper methods
        //have been created
        auto contextCreatedMessage = CefProcessMessage::Create(kOnContextCreatedRequest);

        SetInt64(contextCreatedMessage->GetArgumentList(), 0, frame->GetIdentifier());

        browser->SendProcessMessage(CefProcessId::PID_BROWSER, contextCreatedMessage);
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

        SetInt64(contextReleasedMessage->GetArgumentList(), 0, frame->GetIdentifier());

        browser->SendProcessMessage(CefProcessId::PID_BROWSER, contextReleasedMessage);

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

    void CefAppUnmanagedWrapper::OnUncaughtException(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Exception> exception, CefRefPtr<CefV8StackTrace> stackTrace)
    {
        auto uncaughtExceptionMessage = CefProcessMessage::Create(kOnUncaughtException);
        auto list = uncaughtExceptionMessage->GetArgumentList();

        // Needed in the browser process to get the frame.
        SetInt64(list, 0, frame->GetIdentifier());
        list->SetString(1, exception->GetMessage());

        auto frames = CefListValue::Create();
        for (auto i = 0; i < stackTrace->GetFrameCount(); i++)
        {
            auto frame = CefListValue::Create();
            auto frameArg = stackTrace->GetFrame(i);

            frame->SetString(0, frameArg->GetFunctionName());
            frame->SetInt(1, frameArg->GetLineNumber());
            frame->SetInt(2, frameArg->GetColumn());
            frame->SetString(3, frameArg->GetScriptNameOrSourceURL());

            frames->SetList(i, frame);
        }

        list->SetList(2, frames);

        browser->SendProcessMessage(CefProcessId::PID_BROWSER, uncaughtExceptionMessage);
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

                                auto boundObjects = CefListValue::Create();

                                for (auto i = 0; i < javascriptObjects->Count; i++)
                                {
                                    auto dict = CefDictionaryValue::Create();
                                    auto name = javascriptObjects[i]->JavascriptName;
                                    dict->SetString("Name", StringUtils::ToNative(name));
                                    dict->SetBool("IsCached", false);
                                    dict->SetBool("AlreadyBound", false);

                                    boundObjects->SetDictionary(i, dict);
                                }

                                args->SetList(0, boundObjects);

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
                    auto ext = gcnew V8Extension(StringUtils::ToClr(extension->GetString(0)), StringUtils::ToClr(extension->GetString(1)));

                    _extensions->Add(ext);
                }
            }
        }
    }

    void CefAppUnmanagedWrapper::OnWebKitInitialized()
    {
        for each(V8Extension^ extension in _extensions->AsReadOnly())
        {
            //only support extensions without handlers now
            CefRegisterExtension(StringUtils::ToNative(extension->Name), StringUtils::ToNative(extension->JavascriptCode), NULL);
        }
    }

    void CefAppUnmanagedWrapper::OnRegisterCustomSchemes(CefRawPtr<CefSchemeRegistrar> registrar)
    {
        for each (CefCustomScheme^ scheme in _schemes->AsReadOnly())
        {
            int options = cef_scheme_options_t::CEF_SCHEME_OPTION_NONE;

            if (scheme->IsStandard)
            {
                options |= cef_scheme_options_t::CEF_SCHEME_OPTION_STANDARD;
            }

            if (scheme->IsLocal)
            {
                options |= cef_scheme_options_t::CEF_SCHEME_OPTION_LOCAL;
            }

            if (scheme->IsDisplayIsolated)
            {
                options |= cef_scheme_options_t::CEF_SCHEME_OPTION_DISPLAY_ISOLATED;
            }

            if (scheme->IsSecure)
            {
                options |= cef_scheme_options_t::CEF_SCHEME_OPTION_SECURE;
            }

            if (scheme->IsCorsEnabled)
            {
                options |= cef_scheme_options_t::CEF_SCHEME_OPTION_CORS_ENABLED;
            }

            if (scheme->IsCSPBypassing)
            {
                options |= cef_scheme_options_t::CEF_SCHEME_OPTION_CSP_BYPASSING;
            }

            registrar->AddCustomScheme(StringUtils::ToNative(scheme->SchemeName), options);
        }
    }
}
