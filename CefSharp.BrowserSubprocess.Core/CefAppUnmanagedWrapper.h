// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "CefBrowserWrapper.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    // This class is the native subprocess level CEF object wrapper.
    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        static const CefString kPromiseCreatorScript;

        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserDestroyed;
        gcroot<ConcurrentDictionary<int, CefBrowserWrapper^>^> _browserWrappers;
        gcroot<List<CefExtension^>^> _extensions;
        gcroot<List<CefCustomScheme^>^> _schemes;
        bool _focusedNodeChangedEnabled;

        // The serialized registered object data waiting to be used (only contains methods and bound async).
        gcroot<JavascriptRootObject^> _javascriptAsyncRootObject;

        // The serialized registered object data waiting to be used.
        gcroot<JavascriptRootObject^> _javascriptRootObject;

    public:
        static const CefString kPromiseCreatorFunction;

        CefAppUnmanagedWrapper(List<CefCustomScheme^>^ schemes, bool enableFocusedNodeChanged, Action<CefBrowserWrapper^>^ onBrowserCreated, Action<CefBrowserWrapper^>^ onBrowserDestoryed)
        {
            _onBrowserCreated = onBrowserCreated;
            _onBrowserDestroyed = onBrowserDestoryed;
            _browserWrappers = gcnew ConcurrentDictionary<int, CefBrowserWrapper^>();
            _extensions = gcnew List<CefExtension^>();
            _schemes = schemes;
            _focusedNodeChangedEnabled = enableFocusedNodeChanged;
        }

        ~CefAppUnmanagedWrapper()
        {
            if (!Object::ReferenceEquals(_browserWrappers, nullptr))
            {
                for each(CefBrowserWrapper^ browser in Enumerable::OfType<CefBrowserWrapper^>(_browserWrappers))
                {
                    delete browser;
                }

                _browserWrappers = nullptr;
            }
            delete _onBrowserCreated;
            delete _onBrowserDestroyed;
            delete _extensions;
            delete _schemes;
        }

        CefBrowserWrapper^ FindBrowserWrapper(int browserId, bool mustExist);

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE;
        virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL void OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message) OVERRIDE;
        virtual DECL void OnRenderThreadCreated(CefRefPtr<CefListValue> extraInfo) OVERRIDE;
        virtual DECL void OnWebKitInitialized() OVERRIDE;
        virtual DECL void OnRegisterCustomSchemes(CefRefPtr<CefSchemeRegistrar> registrar) OVERRIDE;
        virtual DECL void OnFocusedNodeChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefDOMNode> node) OVERRIDE;

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
