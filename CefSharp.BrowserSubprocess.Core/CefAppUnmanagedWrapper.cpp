// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "JavascriptRootObjectWrapper.h"
#include "Serialization\V8Serialization.h"
#include "Serialization\ObjectsSerialization.h"
#include "..\CefSharp.Core\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core\Internals\Serialization\Primitives.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
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
        _browserWrappers->Add(browser->GetIdentifier(), wrapper);
    }

    void CefAppUnmanagedWrapper::OnBrowserDestroyed(CefRefPtr<CefBrowser> browser)
    {
        auto wrapper = FindBrowserWrapper(browser->GetIdentifier(), false);

        if (wrapper != nullptr)
        {
            _browserWrappers->Remove(wrapper->BrowserId);
            _onBrowserDestroyed->Invoke(wrapper);
            delete wrapper;
        }
    };

    void CefAppUnmanagedWrapper::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        auto wrapper = FindBrowserWrapper(browser->GetIdentifier(), true);
        auto window = context->GetGlobal();

        if (wrapper->JavascriptRootObject != nullptr)
        {
            wrapper->JavascriptRootObjectWrapper = gcnew JavascriptRootObjectWrapper(wrapper->JavascriptRootObject, wrapper->BrowserProcess);

            wrapper->JavascriptRootObjectWrapper->V8Value = window;
            wrapper->JavascriptRootObjectWrapper->Bind();
        }

        if (wrapper->JavascriptAsyncRootObject != nullptr)
        {
            wrapper->JavascriptAsyncRootObjectWrapper = gcnew JavascriptAsyncRootObjectWrapper(wrapper->JavascriptAsyncRootObject);

            wrapper->JavascriptAsyncRootObjectWrapper->V8Value = window;
            wrapper->JavascriptAsyncRootObjectWrapper->Bind();
        }
    };

    void CefAppUnmanagedWrapper::OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    { 
        auto wrapper = FindBrowserWrapper(browser->GetIdentifier(), true);

        if (wrapper->JavascriptRootObjectWrapper != nullptr)
        {
            delete wrapper->JavascriptRootObjectWrapper;
            wrapper->JavascriptRootObjectWrapper = nullptr;
        }
    };

    CefBrowserWrapper^ CefAppUnmanagedWrapper::FindBrowserWrapper(int browserId, bool mustExist)
    {
        CefBrowserWrapper^ wrapper = nullptr;

        _browserWrappers->TryGetValue(browserId, wrapper);

        if (mustExist && wrapper == nullptr)
        {
            throw gcnew InvalidOperationException(String::Format("Failed to identify BrowserWrapper in OnContextCreated. : {0}", browserId));
        }

        return wrapper;
    }

    bool CefAppUnmanagedWrapper::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message)
    {
        auto handled = false;
        auto name = message->GetName();
        auto argList = message->GetArgumentList();

        auto browserWrapper = FindBrowserWrapper(browser->GetIdentifier(), false);

        //Error handling for missing/closed browser
        if (browserWrapper == nullptr)
        {
            if (name == kJavascriptCallbackDestroyRequest || name == kJavascriptRootObjectRequest || name == kJavascriptMethodCallResponse)
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
                // when they added a new message and havn't yet implemented the render process functionality.
                throw gcnew Exception("Unsupported message type");
            }

            auto callbackId = GetInt64(argList, 1);
            auto response = CefProcessMessage::Create(responseName);
            auto responseArgList = response->GetArgumentList();
            auto errorMessage = String::Format("Request BrowserId : {0} not found it's likely the browser is already closed", browser->GetIdentifier());

            //success: false
            responseArgList->SetBool(0, false);
            SetInt64(callbackId, responseArgList, 1);
            responseArgList->SetString(2, StringUtils::ToNative(errorMessage));
            browser->SendProcessMessage(sourceProcessId, response);

            return true;
        }
    
        //these messages are roughly handled the same way
        if (name == kEvaluateJavascriptRequest || name == kJavascriptCallbackRequest)
        {
            bool success;
            CefRefPtr<CefV8Value> result;
            CefString errorMessage;
            CefRefPtr<CefProcessMessage> response;
            //both messages have the callbackid stored at index 1
            int64 callbackId = GetInt64(argList, 1);

            if (name == kEvaluateJavascriptRequest)
            {
                auto frameId = GetInt64(argList, 0);
                auto script = argList->GetString(2);

                response = CefProcessMessage::Create(kEvaluateJavascriptResponse);

                auto frame = browser->GetFrame(frameId);
                if (frame.get())
                {
                    auto context = frame->GetV8Context();
                    
                    if (context.get() && context->Enter())
                    {
                        try
                        {
                            CefRefPtr<CefV8Exception> exception;
                            success = context->Eval(script, result, exception);
                            
                            //we need to do this here to be able to store the v8context
                            if (success)
                            {
                                auto responseArgList = response->GetArgumentList();
                                SerializeV8Object(result, responseArgList, 2, browserWrapper->CallbackRegistry);
                            }
                            else
                            {
                                errorMessage = exception->GetMessage();
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
                    errorMessage = "Unable to Get Frame matching Id";
                }
            }
            else
            {
                auto jsCallbackId = GetInt64(argList, 0);
                auto parameterList = argList->GetList(2);
                CefV8ValueList params;
                for (CefV8ValueList::size_type i = 0; i < parameterList->GetSize(); i++)
                {
                    params.push_back(DeserializeV8Object(parameterList, static_cast<int>(i)));
                }

                response = CefProcessMessage::Create(kJavascriptCallbackResponse);

                auto callbackRegistry = browserWrapper->CallbackRegistry;
                auto callbackWrapper = callbackRegistry->FindWrapper(jsCallbackId);
                auto context = callbackWrapper->GetContext();
                auto value = callbackWrapper->GetValue();
                
                if (context.get() && context->Enter())
                {
                    try
                    {
                        result = value->ExecuteFunction(nullptr, params);
                        success = result.get() != nullptr;
                        
                        //we need to do this here to be able to store the v8context
                        if (success)
                        {
                            auto responseArgList = response->GetArgumentList();
                            SerializeV8Object(result, responseArgList, 2, browserWrapper->CallbackRegistry);
                        }
                        else
                        {
                            auto exception = value->GetException();
                            if(exception.get())
                            {
                                errorMessage = exception->GetMessage();
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
                    errorMessage = "Unable to Enter Context";			
                }                
            }

            if (response.get())
            {
                auto responseArgList = response->GetArgumentList();
                responseArgList->SetBool(0, success);
                SetInt64(callbackId, responseArgList, 1);
                if (!success)
                {
                    responseArgList->SetString(2, errorMessage);
                }
                browser->SendProcessMessage(sourceProcessId, response);
            }

            handled = true;
        }
        else if (name == kJavascriptCallbackDestroyRequest)
        {
            auto jsCallbackId = GetInt64(argList, 0);
            browserWrapper->CallbackRegistry->Deregister(jsCallbackId);

            handled = true;
        }
        else if (name == kJavascriptRootObjectRequest)
        {
            browserWrapper->JavascriptAsyncRootObject = DeserializeJsObject(argList, 0);
            handled = true;
        }
        else if (name == kJavascriptMethodCallResponse)
        {
            auto callbackId = GetInt64(argList, 0);
            JavascriptAsyncMethodCallback^ callback;
            if (browserWrapper->TryGetAndRemoveMethodCallback(callbackId, callback))
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
                //dispose
                delete callback;
            }
            handled = true;
        }

        return handled;
    };

    DECL void CefAppUnmanagedWrapper::OnWebKitInitialized()
    {
        //we need to do this because the builtin Promise object is not accesible
        CefRegisterExtension("promisecreator", "function cefsharp_CreatePromise() {var object = {};var promise = new Promise(function(resolve, reject) {object.resolve = resolve;object.reject = reject;});return{ p: promise, res : object.resolve,  rej: object.reject};}", nullptr);
    }
}