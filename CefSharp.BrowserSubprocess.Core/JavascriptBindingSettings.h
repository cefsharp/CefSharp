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
                LegacyBindingEnabled = false;
                JavascriptBindingApiEnabled = true;
                JavascriptBindingPropertyName = "CefSharp";
                JavascriptBindingPropertyNameCamelCase = "cefSharp";
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

            property bool LegacyBindingEnabled;
            property bool JavascriptBindingApiEnabled;

            property String^ JavascriptBindingPropertyName;
            property String^ JavascriptBindingPropertyNameCamelCase;

            property bool JavascriptBindingApiHasAllowOrigins;
            property CefRefPtr<CefListValue> JavascriptBindingApiAllowOrigins
            {
                CefRefPtr<CefListValue> get() { return _javascriptBindingApiAllowOrigins.get(); }
                void set(CefRefPtr<CefListValue> value) { _javascriptBindingApiAllowOrigins = value.get(); }
            }
        };
    }
}
