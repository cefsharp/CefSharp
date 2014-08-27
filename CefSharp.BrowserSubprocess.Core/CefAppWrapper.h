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

using namespace System::Collections::Generic;

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    public ref class CefAppWrapper abstract : public ManagedCefApp
    {
    private:
        MCefRefPtr<CefAppUnmanagedWrapper> cefApp;

    internal:
        List<CefBrowserWrapper^>^ browserWrappers;
        
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
            auto wrapper = gcnew CefBrowserWrapper(browser);
            CefAppWrapper::Instance->browserWrappers->Add(wrapper);
            _onBrowserCreated->Invoke(wrapper);
        }

        virtual DECL void OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context) OVERRIDE
        {
            auto window = context->GetGlobal();

            JavascriptObjectWrapper^ jswindow = _windowObject;


            if (jswindow != nullptr)
            {
                jswindow->Value = window;
                jswindow->Bind();
            }
        };
        
        void Bind(JavascriptObject^ windowObject)
        {
            _windowObject = gcnew JavascriptObjectWrapper();
            _windowObject->Clone(windowObject);
        };

        virtual DECL void CefAppUnmanagedWrapper::OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE
        {
            auto browserWrappers = CefAppWrapper::Instance->browserWrappers;
            auto browserId = browser->GetIdentifier();
            CefBrowserWrapper^ wrapper = nullptr;
            for (int i = 0; i < browserWrappers->Count; i++)
            {
                if (browserWrappers[i]->BrowserId == browserId)
                {
                    wrapper = browserWrappers[i];
                    browserWrappers->RemoveAt(i);
                    break;
                }
            }
            if (wrapper != nullptr)
            {
                delete wrapper;
            }
        };

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
