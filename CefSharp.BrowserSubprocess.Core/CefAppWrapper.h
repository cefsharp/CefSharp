// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptMethodHandler.h"
#include "JavascriptMethodWrapper.h"
#include "JavascriptObjectWrapper.h"
#include "JavascriptPropertyHandler.h"
#include "JavascriptPropertyWrapper.h"
#include "CefSubprocessWrapper.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    public ref class CefAppWrapper abstract : public ManagedCefApp
    {
    private:
        MCefRefPtr<CefAppUnmanagedWrapper> cefApp;
        
    public:        
        static CefAppWrapper^ Instance;

        CefAppWrapper();
        int Run();

        void Bind(JavascriptObject^ windowObject);
    };

    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<Action<CefBrowserBase^>^> _onBrowserCreated;
        gcroot<JavascriptObjectWrapper^> _windowObject;
    public:
        
        CefAppUnmanagedWrapper(Action<CefBrowserBase^>^ onBrowserCreated)
        {
            _onBrowserCreated = onBrowserCreated;
        }

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE
        {
            return this;
        };

        // CefRenderProcessHandler
        virtual DECL void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE
        {
            // TODO: Could destroy this CefBrowserWrapper in OnBrowserDestroyed(), but it doesn't seem to be reliably called...
            _onBrowserCreated->Invoke(gcnew CefBrowserWrapper(browser));
        }

        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE
        {
            auto window = context->GetGlobal();

            JavascriptObjectWrapper^ jswindow = _windowObject;

            if (jswindow != nullptr)
            {
                jswindow->V8Value = window;
                jswindow->Bind();
            }
        };
        
        void Bind(JavascriptObject^ windowObject)
        {
            _windowObject = gcnew JavascriptObjectWrapper();
            _windowObject->Clone(windowObject);
        };

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
