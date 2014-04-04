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

        virtual DECL void CefAppUnmanagedWrapper::OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE
        {
            // TODO: Could destroy this CefBrowserWrapper in OnBrowserDestroyed(), but it doesn't seem to be reliably called...
            _onBrowserCreated->Invoke(gcnew CefBrowserWrapper(browser));
        }

        IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
    };
}
