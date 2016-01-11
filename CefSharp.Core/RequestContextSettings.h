// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_request_context.h"

namespace CefSharp
{
    /// <summary>
    /// RequestContextSettings
    /// </summary>
    public ref class RequestContextSettings
    {
    private:
        CefRequestContextSettings* _settings;

    public:
        RequestContextSettings() : _settings(new CefRequestContextSettings())
        {
        }

        !RequestContextSettings()
        {
            delete _settings;
        }

        ~RequestContextSettings()
        {
            this->!RequestContextSettings();
        }

        property bool PersistSessionCookies
        {
            bool get() { return _settings->persist_session_cookies == 1; }
            void set(bool value) { _settings->persist_session_cookies = value; }
        }

        property String^ CachePath
        {
            String^ get() { return StringUtils::ToClr(_settings->cache_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_settings->cache_path, value); }
        }

        property String^ AcceptLanguageList
        {
            String^ get() { return StringUtils::ToClr(_settings->accept_language_list); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_settings->accept_language_list, value); }
        }

        property bool IgnoreCertificateErrors
        {
            bool get() { return _settings->ignore_certificate_errors == 1; }
            void set(bool value) { _settings->ignore_certificate_errors = value; }
        }

        operator CefRequestContextSettings()
        {
            return *_settings;
        }
    };
}