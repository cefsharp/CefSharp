// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

using namespace CefSharp::Internals::JavascriptBinding;
using namespace System;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private ref class JavascriptProxy : IJavascriptProxy
        {
            SubprocessCefApp* _subprocessCefApp;

        public:
            // TODO: Don't hardwire the address like this, since it makes it impossible to run multiple apps that use
            // CefSharp simultaneously...
            literal String^ BaseAddress = "net.pipe://localhost";
            literal String^ ServiceName = "JavaScriptProxy";
            static String^ Address = BaseAddress + "/" + ServiceName;

            JavascriptProxy()
            {
                _subprocessCefApp = SubprocessCefApp::GetInstance();
            }

            virtual Object^ EvaluateScript(int browserId, int frameId, String^ script, double timeout)
            {
                auto browser = _subprocessCefApp->GetBrowserById(browserId);
                auto frame = browser->GetFrame(frameId);
                auto v8Context = frame->GetV8Context();

                if (v8Context.get() && v8Context->Enter())
                {
                    CefRefPtr<CefV8Value> result;
                    CefRefPtr<CefV8Exception> exception;

                    v8Context->Eval("10 + 20", result, exception);
                }

                return "gurka";
            }
        };
    }
}
