// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
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

    JavascriptResponse^ CefBrowserWrapper::DoCallback(System::Int64 callbackId, array<Object^>^ parameters)
    {
        return _callbackRegistry->Execute(callbackId, parameters);
    }

    void CefBrowserWrapper::DestroyJavascriptCallback(Int64 id)
    {
        _callbackRegistry->Deregister(id);
    }

    JavascriptCallbackRegistry^ CefBrowserWrapper::CallbackRegistry::get()
    {
        return _callbackRegistry;
    }

    CefBrowserWrapper::!CefBrowserWrapper()
    {
        _cefBrowser = nullptr;
    }

    CefBrowserWrapper::~CefBrowserWrapper()
    {
        this->!CefBrowserWrapper();
        if (_callbackRegistry != nullptr)
        {
            delete _callbackRegistry;
            _callbackRegistry = nullptr;
        }
    }
}