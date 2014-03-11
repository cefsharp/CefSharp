// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
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
        CefSubprocessBase^ _managedApp;

    public:
        CefAppWrapper(CefSubprocessBase^ managedApp);

        int Run(array<String^>^ args);
    };

    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<CefAppWrapper^> _cefAppWrapper;
        CefRefPtr<CefBrowser> _browser;

    public:
        CefAppUnmanagedWrapper(CefAppWrapper^ cefAppWrapper) :
            _cefAppWrapper(cefAppWrapper)
        {
            // make shure CEF Cingleton is instanciated so it can be used
            Cef::Instance->ToString();
        }

        ~CefAppUnmanagedWrapper()
        {
            _cefAppWrapper = nullptr;
            _browser = nullptr;
        }

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE
        {
            return this;
        };

        virtual DECL void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE
        {
            _browser = browser;
            // TODO: Could destroy this CefBrowserWrapper in OnBrowserDestroyed(), but it doesn't seem to be reliably called...
            _cefAppWrapper->_managedApp->OnBrowserCreated(gcnew CefBrowserWrapper(browser));
        }

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
