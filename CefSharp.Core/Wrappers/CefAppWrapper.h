// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefSubprocessWrapper.h"
#include "include/cef_app.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    public ref class CefAppWrapper
    {
    private:
        MCefRefPtr<CefAppUnmanagedWrapper> cefApp;
    internal:
        CefSubprocess^ _managedApp;
        List<CefBrowserWrapper^>^ browserWrappers;

    public:
        CefAppWrapper(CefSubprocess^ managedApp);

        int Run(array<String^>^ args);
    };

    private class CefAppUnmanagedWrapper : CefApp, CefRenderProcessHandler
    {
    private:
        gcroot<CefAppWrapper^> _cefAppWrapper;

    public:
        CefAppUnmanagedWrapper(CefAppWrapper^ cefAppWrapper) :
            _cefAppWrapper(cefAppWrapper)
        {
        }

        ~CefAppUnmanagedWrapper()
        {
            _cefAppWrapper = nullptr;
        }

        virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE
        {
            return this;
        };

        virtual DECL void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE
        {
            auto wrapper = gcnew CefBrowserWrapper(browser);
            _cefAppWrapper->browserWrappers->Add(wrapper);
            _cefAppWrapper->_managedApp->OnBrowserCreated(wrapper);
        };

        virtual DECL void CefAppUnmanagedWrapper::OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE
        {
            auto browserId = browser->GetIdentifier();
            CefBrowserWrapper^ wrapper = nullptr;
            for (int i = 0; i < _cefAppWrapper->browserWrappers->Count; i++)
            {
                if (_cefAppWrapper->browserWrappers[i]->BrowserId == browserId)
                {
                    wrapper = _cefAppWrapper->browserWrappers[i];
                    _cefAppWrapper->browserWrappers->RemoveAt(i);
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
