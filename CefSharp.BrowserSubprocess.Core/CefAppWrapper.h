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

        void RegisterJavascriptObjects(JavascriptObjectWrapper^ windowObject);
    };

    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<Action<CefBrowserWrapper^>^> _onBrowserCreated;
        gcroot<JavascriptObject^> _boundObject;
        CefRefPtr<CefV8Context> _context;
        CefRefPtr<CefV8Value> _window;
        gcroot<JavascriptObjectWrapper^> _windowObject;

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
            _context = context;
            _window = context->GetGlobal();

            //TODO; whats the right timing here
            //TODO: _windowObject is not yet set
            Bind();
        };

        void RegisterJavascriptObjects(JavascriptObjectWrapper^ windowObject)
        {   
            _windowObject = windowObject;

            //TODO; whats the right timing here
            //TODO: cant get a valid window here 
            //Bind();
        };

        void Bind()
        {
            /*auto currentthread = CefTaskRunner::GetForCurrentThread();
            auto renderThread = CefTaskRunner::GetForThread(CefThreadId::TID_RENDERER);

            if (currentthread == nullptr || !currentthread->IsSame(renderThread))
            {
                CefPostTask(CefThreadId::TID_RENDERER, NewCefRunnableMethod(this, &CefAppUnmanagedWrapper::Bind));
                return;
            }*/
            

            JavascriptObjectWrapper^ windowObject = _windowObject;
            CefRefPtr<CefV8Context> context = _context;

            if (windowObject != nullptr && context != nullptr)
            {
                windowObject->Value = _window;
                windowObject->Bind();
            }
        };

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
