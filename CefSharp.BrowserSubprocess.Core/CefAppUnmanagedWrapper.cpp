// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "JavascriptRootObjectWrapper.h"
#include "Serialization\V8Serialization.h"
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

        if (wrapper->JavascriptRootObject != nullptr)
        {
            auto window = context->GetGlobal();

            wrapper->JavascriptRootObjectWrapper = gcnew JavascriptRootObjectWrapper(wrapper->JavascriptRootObject, wrapper->BrowserProcess);

            wrapper->JavascriptRootObjectWrapper->V8Value = window;
            wrapper->JavascriptRootObjectWrapper->Bind();
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

        auto browserId = argList->GetInt(0);
        if (browser->GetIdentifier() != browserId)
        {
            throw gcnew InvalidOperationException(String::Format("Request BrowserId : {0} does not match browser Id : {1}", browserId, browser->GetIdentifier()));
        }
        auto browserWrapper = FindBrowserWrapper(browserId, true);

        if (name == kEvaluateJavascriptRequest || name == kJavascriptCallbackRequest)
        {
            CefRefPtr<CefV8Value> result;
            CefRefPtr<CefV8Exception> exception;
            CefRefPtr<CefProcessMessage> response;
            bool success;
            int64 callbackId;

            if (name == kEvaluateJavascriptRequest)
            {
                auto frameId = GetInt64(argList, 1);
                callbackId = GetInt64(argList, 2);
                auto script = argList->GetString(3);

                auto frame = browser->GetFrame(frameId);
                if (frame.get())
                {
                    auto context = frame->GetV8Context();
                    V8ContextScope scope(context);

                    success = context->Eval(script, result, exception);
                    response = CefProcessMessage::Create(kEvaluateJavascriptResponse);
                    //we need to do this here to be able to store the v8context
                    if (success)
                    {
                        auto argList = response->GetArgumentList();
                        SerializeV8Object(result, argList, 2, browserWrapper->CallbackRegistry);
                    }
                }
                else
                {
                    //TODO handle error
                }
            }
            else
            {
                auto jsCallbackId = GetInt64(argList, 1);
                callbackId = GetInt64(argList, 2);
                auto parameterList = argList->GetList(3);
                CefV8ValueList params;
                for (CefV8ValueList::size_type i = 0; i < parameterList->GetSize(); i++)
                {
                    params.push_back(DeserializeV8Object(parameterList, static_cast<int>(i)));
                }

                auto callbackRegistry = browserWrapper->CallbackRegistry;
                auto callbackWrapper = callbackRegistry->FindWrapper(jsCallbackId);
                auto context = callbackWrapper->GetContext();
                auto value = callbackWrapper->GetValue();

                {
                    V8ContextScope scope(context);

                    result = value->ExecuteFunction(nullptr, params);
                    success = result.get() != nullptr;
                    response = CefProcessMessage::Create(kJavascriptCallbackResponse);
                    //we need to do this here to be able to store the v8context
                    if (success)
                    {
                        auto argList = response->GetArgumentList();
                        SerializeV8Object(result, argList, 2, browserWrapper->CallbackRegistry);
                    }
                    else
                    {
                        exception = value->GetException();
                    }
                }
            }

            if (response.get())
            {
                auto argList = response->GetArgumentList();
                argList->SetBool(0, success);
                SetInt64(callbackId, argList, 1);
                if (!success)
                {
                    argList->SetString(2, exception->GetMessage());
                }
                browser->SendProcessMessage(sourceProcessId, response);
            }

            handled = true;
        }
        else if (name == kJavascriptCallbackDestroyRequest)
        {
            auto jsCallbackId = GetInt64(argList, 1);
            browserWrapper->CallbackRegistry->Deregister(jsCallbackId);

            handled = true;
        }

        return handled;
    };
}