// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once


#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "CefBrowserWrapper.h"
#include "Messaging/EvaluateScriptDelegate.h"
#include "../CefSharp.Core/Internals/Messaging/ProcessMessageDelegate.h"

using namespace System::Collections::Generic;
using namespace CefSharp::Internals::Messaging;

namespace CefSharp
{
    // This class is the native subprocess level CEF object wrapper.
    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        ProcessMessageDelegateSet _processMessageDelegates;

        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserDestroyed;
        gcroot<Dictionary<int, CefBrowserWrapper^>^> _browserWrappers;
    public:
        
        CefAppUnmanagedWrapper(Action<CefBrowserWrapper^>^ onBrowserCreated, Action<CefBrowserWrapper^>^ onBrowserDestoryed)
        {
            _onBrowserCreated = onBrowserCreated;
            _onBrowserDestroyed = onBrowserDestoryed;
            _browserWrappers = gcnew Dictionary<int, CefBrowserWrapper^>();

            //Register evaluate script request handler
            _processMessageDelegates.insert(new EvaluateScriptDelegate(this));
        }

        ~CefAppUnmanagedWrapper()
        {
            delete _browserWrappers;
            delete _onBrowserCreated;
            delete _onBrowserDestroyed;
        }

        CefBrowserWrapper^ FindBrowserWrapper(int browserId, bool mustExist);

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE;
        virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL void OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message) OVERRIDE;

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
