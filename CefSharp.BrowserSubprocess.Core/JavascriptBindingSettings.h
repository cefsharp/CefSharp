// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Stdafx.h"

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private ref class JavascriptBindingSettings
        {
        private:
            MCefRefPtr<CefListValue> _javascriptBindingApiAllowOrigins;

        public:
            JavascriptBindingSettings()
            {
                JavascriptBindingApiEnabled = true;
                JavascriptBindingApiHasAllowOrigins = false;
                JavascriptBindingApiAllowOrigins = nullptr;
            }

            !JavascriptBindingSettings()
            {
                _javascriptBindingApiAllowOrigins = nullptr;
            }

            ~JavascriptBindingSettings()
            {
                this->!JavascriptBindingSettings();
            }

            property bool JavascriptBindingApiEnabled;
            property bool JavascriptBindingApiHasAllowOrigins;
            property CefRefPtr<CefListValue> JavascriptBindingApiAllowOrigins
            {
                CefRefPtr<CefListValue> get() { return _javascriptBindingApiAllowOrigins.get(); }
                void set(CefRefPtr<CefListValue> value) { _javascriptBindingApiAllowOrigins = value.get(); }
            }
        };
    }
}
