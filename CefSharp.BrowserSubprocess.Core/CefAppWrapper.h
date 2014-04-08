// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptMethodHandler.h"
#include "JavascriptPropertyHandler.h"
#include "CefSubprocessWrapper.h"
#include "include/cef_app.h"

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    public ref class CefAppWrapper
    {
    private:
        CefRefPtr<CefAppUnmanagedWrapper>* cefApp;

    internal:
        Action<CefBrowserWrapper^>^ OnBrowserCreated;

    public:

        CefAppWrapper(Action<CefBrowserWrapper^>^ onBrowserCreated);
        int Run();
    };

    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;

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
            auto window = context->GetGlobal();
            auto javascriptPropertyHandler = new JavascriptPropertyHandler();
            auto boundObject = window->CreateObject(static_cast<CefRefPtr<CefV8Accessor>>(javascriptPropertyHandler));

            auto methodName = StringUtils::ToNative("bar");
            auto handler = static_cast<CefV8Handler*>(new JavascriptMethodHandler());
            boundObject->SetValue(methodName, CefV8Value::CreateFunction(methodName, handler), V8_PROPERTY_ATTRIBUTE_NONE);

            window->SetValue(StringUtils::ToNative("foo"), boundObject, V8_PROPERTY_ATTRIBUTE_NONE);            

            // TODO: Support the BindingHandler with CEF3.
            /*
            for each(KeyValuePair<String^, Object^>^ kvp in Cef::GetBoundObjects())
            {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
            }

            for each(KeyValuePair<String^, Object^>^ kvp in _browserControl->GetBoundObjects())
            {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
            }
            */
        }

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
