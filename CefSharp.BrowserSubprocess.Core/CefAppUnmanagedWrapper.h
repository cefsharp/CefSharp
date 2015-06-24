// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <map>

#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "CefBrowserWrapper.h"
#include "Internals/Messaging/ProcessMessageDelegate.h"
#include "Messaging/JsRootObjectDelegate.h"
#include "Messaging/EvaluateScriptDelegate.h"
#include "Messaging/JavascriptCallbackDelegate.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    // This class is the native subprocess level CEF object wrapper.
    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        friend Internals::Messaging::JsRootObjectDelegate;
        friend Internals::Messaging::EvaluateScriptDelegate;
        friend Internals::Messaging::JavascriptCallbackDelegate;

        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserDestroyed;
        gcroot<Dictionary<int, CefBrowserWrapper^>^> _browserWrappers;

        std::map<int, CefRefPtr<CefBrowser>> _browsers;
        Internals::Messaging::ProcessMessageDelegateSet _processMessageDelegates;

        CefBrowserWrapper^ FindBrowserWrapper(CefRefPtr<CefBrowser> browser, bool mustExist);
        CefRefPtr<CefBrowser> FindBrowser(int browserId);
    public:
        
        CefAppUnmanagedWrapper(Action<CefBrowserWrapper^>^ onBrowserCreated, Action<CefBrowserWrapper^>^ onBrowserDestoryed)
        {
            _onBrowserCreated = onBrowserCreated;
            _onBrowserDestroyed = onBrowserDestoryed;
            _browserWrappers = gcnew Dictionary<int, CefBrowserWrapper^>();

            AddProcessMessageDelegate(new Internals::Messaging::JsRootObjectDelegate(this));
            AddProcessMessageDelegate(new Internals::Messaging::EvaluateScriptDelegate(this));
            AddProcessMessageDelegate(new Internals::Messaging::JavascriptCallbackDelegate(this));
        }

        ~CefAppUnmanagedWrapper()
        {
            delete _browserWrappers;
            delete _onBrowserCreated;
            delete _onBrowserDestroyed;
        }

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE;
        virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL void OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL bool OnProcessMessageReceived(CefRefPtr< CefBrowser > browser, CefProcessId source_process, CefRefPtr< CefProcessMessage > message) OVERRIDE;

        void AddProcessMessageDelegate(CefRefPtr<Internals::Messaging::ProcessMessageDelegate> processMessageDelegate);

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
