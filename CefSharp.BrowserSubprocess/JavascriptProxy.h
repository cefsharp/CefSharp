// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
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
        public:
            // TODO: Don't hardwire the address like this, since it makes it impossible to run multiple apps that use
            // CefSharp simultaneously...
            literal String^ BaseAddress = "net.pipe://localhost";
            literal String^ ServiceName = "JavaScriptProxy";
            static String^ Address = BaseAddress + "/" + ServiceName;

            virtual Object^ EvaluateScript(String^ script, double timeout)
            {
                return "gurka";
            }
        };
    }
}
