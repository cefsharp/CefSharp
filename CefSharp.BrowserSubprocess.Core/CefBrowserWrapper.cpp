// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"

namespace CefSharp
{
    CefBrowserWrapper::CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser)
    {
        _cefBrowser = cefBrowser;
        BrowserId = cefBrowser->GetIdentifier();
        IsPopup = cefBrowser->IsPopup();
        _callbackRegistry = gcnew JavascriptCallbackRegistry(BrowserId);
    }

    JavascriptRootObjectWrapper^ CefBrowserWrapper::JavascriptRootObjectWrapper::get()
    {
        return _javascriptRootObjectWrapper;
    }

    void CefBrowserWrapper::JavascriptRootObjectWrapper::set(CefSharp::JavascriptRootObjectWrapper^ value)
    {
        _javascriptRootObjectWrapper = value;
        if (_javascriptRootObjectWrapper != nullptr)
        {
            _javascriptRootObjectWrapper->CallbackRegistry = _callbackRegistry;
        }
    }

    JavascriptResponse^ CefBrowserWrapper::EvaluateScriptInContext(CefRefPtr<CefV8Context> context, CefString script)
    {
        CefRefPtr<CefV8Value> result;
        CefRefPtr<CefV8Exception> exception;
        JavascriptResponse^ response = gcnew JavascriptResponse();

        response->Success = context->Eval(script, result, exception);
        if (response->Success)
        {
            if (result->IsFunction())
            {
                response->Result = _callbackRegistry->Register(context, result);
            }
            else 
            {
                response->Result = TypeUtils::ConvertFromCef(result);
            }
        }
        else if (exception.get())
        {
            response->Message = StringUtils::ToClr(exception->GetMessage());
        }

        return response;
    }

    void CefBrowserWrapper::DoDispose(bool disposing)
    {
        _cefBrowser = nullptr;
        if (disposing)
        {
            delete _callbackRegistry;
            _callbackRegistry = nullptr;
        }
        DisposableResource::DoDispose(disposing);
    }

    JavascriptResponse^ CefBrowserWrapper::DoEvaluateScript(System::Int64 frameId, String^ script)
    {
        auto frame = _cefBrowser->GetFrame(frameId);
        CefRefPtr<CefV8Context> context = frame->GetV8Context();

        if (context.get() && context->Enter())
        {
            try
            {
                return EvaluateScriptInContext(context, StringUtils::ToNative(script));
            }
            finally
            {
                context->Exit();
            }
        }

        return nullptr;
    }

    JavascriptResponse^ CefBrowserWrapper::DoCallback(System::Int64 callbackId, array<Object^>^ parameters)
    {
        return _callbackRegistry->Execute(callbackId, parameters);
    }

    void CefBrowserWrapper::DestroyJavascriptCallback(Int64 id)
    {
        _callbackRegistry->Deregister(id);
    }

    CefBrowserWrapper::~CefBrowserWrapper()
    {
        _cefBrowser = nullptr;
        delete _callbackRegistry;
        _callbackRegistry = nullptr;
    }
}