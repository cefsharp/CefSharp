// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "CefBrowserWrapper.h"
#include "RegisterBoundObjectRegistry.h"

using namespace System::Collections::Generic;
using namespace CefSharp::RenderProcess;

namespace CefSharp
{
    // This class is the native subprocess level CEF object wrapper.
    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<IRenderProcessHandler^> _handler;
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserDestroyed;
        gcroot<ConcurrentDictionary<int, CefBrowserWrapper^>^> _browserWrappers;
        gcroot<List<V8Extension^>^> _extensions;
        gcroot<List<CefCustomScheme^>^> _schemes;
        bool _focusedNodeChangedEnabled;
        bool _legacyBindingEnabled;

        // The serialized registered object data waiting to be used.
        gcroot<Dictionary<String^, JavascriptObject^>^> _javascriptObjects;

        gcroot<RegisterBoundObjectRegistry^> _registerBoundObjectRegistry;

    public:
        static const CefString kPromiseCreatorScript;

        CefAppUnmanagedWrapper(IRenderProcessHandler^ handler, List<CefCustomScheme^>^ schemes, bool enableFocusedNodeChanged, Action<CefBrowserWrapper^>^ onBrowserCreated, Action<CefBrowserWrapper^>^ onBrowserDestroyed)
        {
            _handler = handler;
            _onBrowserCreated = onBrowserCreated;
            _onBrowserDestroyed = onBrowserDestroyed;
            _browserWrappers = gcnew ConcurrentDictionary<int, CefBrowserWrapper^>();
            _extensions = gcnew List<V8Extension^>();
            _schemes = schemes;
            _focusedNodeChangedEnabled = enableFocusedNodeChanged;
            _javascriptObjects = gcnew Dictionary<String^, JavascriptObject^>();
            _registerBoundObjectRegistry = gcnew RegisterBoundObjectRegistry();
            _legacyBindingEnabled = false;
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

        CefBrowserWrapper^ FindBrowserWrapper(int browserId);
        JavascriptRootObjectWrapper^ GetJsRootObjectWrapper(int browserId, int64 frameId);

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE;
        virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE;
        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL void OnContextReleased(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE;
        virtual DECL bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId sourceProcessId, CefRefPtr<CefProcessMessage> message) OVERRIDE;
        virtual DECL void OnRenderThreadCreated(CefRefPtr<CefListValue> extraInfo) OVERRIDE;
        virtual DECL void OnWebKitInitialized() OVERRIDE;
        virtual DECL void OnRegisterCustomSchemes(CefRawPtr<CefSchemeRegistrar> registrar) OVERRIDE;
        virtual DECL void OnFocusedNodeChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefDOMNode> node) OVERRIDE;
        virtual DECL void OnUncaughtException(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Exception> exception, CefRefPtr<CefV8StackTrace> stackTrace) OVERRIDE;

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
