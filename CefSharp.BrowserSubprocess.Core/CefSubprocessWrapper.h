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
#include "CefTaskScheduler.h"

using namespace CefSharp::Internals;
using namespace System;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

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
            RenderThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_RENDERER));
        }

        virtual void DoDispose( bool disposing ) override
        {
            _cefBrowser = nullptr;
            _unmanagedWrapper = nullptr;
            CefBrowserBase::DoDispose( disposing );
        }

        virtual Object^ DoEvaluateScript(System::Int64 frameId, String^ script) override
        {
            return _unmanagedWrapper->EvaluateScriptCallback(frameId, StringUtils::ToNative(script));
        }
    };
}
