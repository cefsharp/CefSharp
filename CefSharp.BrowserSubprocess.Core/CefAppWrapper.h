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

    public ref class CefAppWrapper
    {
    private:
        CefRefPtr<CefAppUnmanagedWrapper>* cefApp;
        ISubprocessCallback^ _callback;

    internal:
        Action<CefBrowserWrapper^>^ OnBrowserCreated;

        virtual property ISubprocessCallback^ Callback 
        {
            ISubprocessCallback^ get() { return _callback; }
            void set(ISubprocessCallback^ value) { _callback = value; }
        }

    public:        
        static CefAppWrapper^ Instance;

        CefAppWrapper(Action<CefBrowserWrapper^>^ onBrowserCreated);
        int Run();
    };

    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;
        gcroot<JavascriptObject^> _boundObject;

    public:
        CefAppUnmanagedWrapper(Action<CefBrowserWrapper^>^ onBrowserCreated)
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
            // TODO: Dummy code for now which just sets up a global window.foo object with a bar() method. :)
            
            auto bar = gcnew JavascriptMethodWrapper();
            bar->Description->JavascriptName = "bar";
            bar->Description->ManagedName = "Bar";

            auto foo = gcnew JavascriptPropertyWrapper();
            foo->Description->JavascriptName = "foo";
            foo->Description->ManagedName = "Foo";
            foo->Value->Members->Add(bar);

            auto windowObject = gcnew JavascriptObjectWrapper();
            windowObject->Members->Add(foo);
            windowObject->Value = context->GetGlobal();

            windowObject->Bind();
        }

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
