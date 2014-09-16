// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefAppWrapper.h"
#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;

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
        auto browserId = browser->GetIdentifier();
        CefBrowserWrapper^ wrapper = nullptr;

        _browserWrappers->TryGetValue(browserId, wrapper);

        if (wrapper != nullptr)
        {
            _browserWrappers->Remove(browserId);
            _onBrowserDestroyed->Invoke(wrapper);
            delete wrapper;
        }
    };

    void CefAppUnmanagedWrapper::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        if (!Object::ReferenceEquals(_javascriptRootObject, nullptr))
        {
            auto window = context->GetGlobal();
            
            auto jsRootWrapper = gcnew JavascriptRootObjectWrapper(_javascriptRootObject, _createBrowserProxyDelegate);
        
            jsRootWrapper->V8Value = window;
            jsRootWrapper->Bind();
        }
    };

    void CefAppUnmanagedWrapper::OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
    {
        
    };

    void CefAppUnmanagedWrapper::Bind(JavascriptRootObject^ rootObject, Func<IBrowserProcess^>^ createBrowserProxyDelegate)
    {
        _javascriptRootObject = rootObject;
        _createBrowserProxyDelegate = createBrowserProxyDelegate;
    };
}