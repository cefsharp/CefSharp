// Copyright © 2015 The CefSharp Authors. All rights reserved.
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

    internal:
        operator CefRequestContextSettings()
        {
            return *_settings;
        }

    public:
        /// <summary>
        /// Default constructor
        /// </summary>
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

        /// <summary>
        /// To persist session cookies (cookies without an expiry date or validity
        /// interval) by default when using the global cookie manager set this value to
        /// true. Session cookies are generally intended to be transient and most
        /// Web browsers do not persist them. Can be set globally using the
        /// CefSettings.PersistSessionCookies value. This value will be ignored if
        /// CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        property bool PersistSessionCookies
        {
            bool get() { return _settings->persist_session_cookies == 1; }
            void set(bool value) { _settings->persist_session_cookies = value; }
        }

        /// <summary>
        /// To persist user preferences as a JSON file in the cache path directory set
        /// this value to true. Can be set globally using the
        /// CefSettings.PersistUserPreferences value. This value will be ignored if
        /// CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        property bool PersistUserPreferences
        {
            bool get() { return _settings->persist_user_preferences == 1; }
            void set(bool value) { _settings->persist_user_preferences = value; }
        }

        /// <summary>
        /// The location where cache data will be stored on disk. If empty then
        /// browsers will be created in "incognito mode" where in-memory caches are
        /// used for storage and no data is persisted to disk. HTML5 databases such as
        /// localStorage will only persist across sessions if a cache path is
        /// specified. To share the global browser cache and related configuration set
        /// this value to match the CefSettings.CachePath value.
        /// </summary>
        property String^ CachePath
        {
            String^ get() { return StringUtils::ToClr(_settings->cache_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_settings->cache_path, value); }
        }

        /// <summary>
        /// Comma delimited ordered list of language codes without any whitespace that
        /// will be used in the "Accept-Language" HTTP header. Can be set globally
        /// using the CefSettings.accept_language_list value or overridden on a per-
        /// browser basis using the BrowserSettings.AcceptLanguageList value. If
        /// all values are empty then "en-US,en" will be used. This value will be
        /// ignored if CachePath matches the CefSettings.CachePath value.
        /// </summary>
        property String^ AcceptLanguageList
        {
            String^ get() { return StringUtils::ToClr(_settings->accept_language_list); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_settings->accept_language_list, value); }
        }

        /// <summary>
        /// Set to true to enable date-based expiration of built in network security information (i.e. certificate transparency logs,
        /// HSTS preloading and pinning information). Enabling this option improves network security but may cause HTTPS load failures when
        /// using CEF binaries built more than 10 weeks in the past. See https://www.certificate-transparency.org/ and
        /// https://www.chromium.org/hsts for details. Can be set globally using the CefSettings.EnableNetSecurityExpiration value.
        /// </summary>
        property bool EnableNetSecurityExpiration
        {
            bool get() { return _settings->enable_net_security_expiration == 1; }
            void set(bool value) { _settings->enable_net_security_expiration = value; }
        }

        /// <summary>
        /// Set to true to ignore errors related to invalid SSL certificates.
        /// Enabling this setting can lead to potential security vulnerabilities like
        /// "man in the middle" attacks. Applications that load content from the
        /// internet should not enable this setting. Can be set globally using the
        /// CefSettings.IgnoreCertificateErrors value. This value will be ignored if
        /// CachePath matches the CefSettings.cache_path value.
        /// </summary>
        property bool IgnoreCertificateErrors
        {
            bool get() { return _settings->ignore_certificate_errors == 1; }
            void set(bool value) { _settings->ignore_certificate_errors = value; }
        }
    };
}