// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"
#include "include/cef_runnable.h"
#include "include/cef_v8.h"

#include "TypeUtils.h"
#include "Stdafx.h"

#include "CefBrowserUnmanagedWrapper.h"

using namespace CefSharp::Internals;
using namespace System;
using namespace System::ServiceModel;
using namespace System::Threading;

namespace CefSharp
{
    // "Master class" for wrapping everything that the CefSubprocess needs.
    ref class CefBrowserWrapper : CefBrowserBase
    {
        MCefRefPtr<CefBrowser> _cefBrowser;
        MCefRefPtr<CefBrowserUnmanagedWrapper> _unmanagedWrapper;

    public:

        CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser) :
            _cefBrowser(cefBrowser)
        {
            BrowserId = cefBrowser->GetIdentifier();
            _unmanagedWrapper = new CefBrowserUnmanagedWrapper(cefBrowser);
        }

        virtual void DoDispose( bool disposing ) override
        {
            _cefBrowser = nullptr;
            _unmanagedWrapper = nullptr;
            CefBrowserBase::DoDispose( disposing );
        }

        virtual Object^ EvaluateScript(System::Int32 frameId, String^ script, double timeout) override
        {
            // TODO: Could we do something genericly useful here using C++ lambdas? To avoid having to make a lot of of these...
            // TODO: DON'T USE AUTORESETEVENT STUPIDITY! Even though the code below compiles & runs correctly, it deadlocks the
            // thread from which the request came, which is very, very stupid, especially since V8 and Chromium are built
            // with asynchrony in mind. Instead, we should re-think this API to utilize WCF callbacks instead:
            // http://idunno.org/archive/2008/05/29/wcf-callbacks-a-beginners-guide.aspx
            // That feels much more like 2013, and not 1994... :)
            // TODO: How about concurrency? One way to easily resolve it is to new() up something unique here and use that to
            // invoke the method.
            CefBrowserUnmanagedWrapper* unmanagedWrapper = _unmanagedWrapper.get();
            CefPostTask(TID_RENDERER, NewCefRunnableMethod(unmanagedWrapper,
                &CefBrowserUnmanagedWrapper::EvaluateScriptCallback, frameId, StringUtils::ToNative(script), timeout));
            unmanagedWrapper->WaitHandle->WaitOne();

            if (static_cast<String^>(unmanagedWrapper->EvaluateScriptExceptionMessage) != nullptr)
            {
                throw gcnew FaultException(unmanagedWrapper->EvaluateScriptExceptionMessage);
            }

            return unmanagedWrapper->EvaluateScriptResult;
        }
    };
}
