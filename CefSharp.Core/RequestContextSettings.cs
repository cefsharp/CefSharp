// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;

namespace CefSharp
{
    /// <summary>
    /// RequestContext Settings
    /// </summary>
    public class RequestContextSettings
    {
        internal Core.RequestContextSettings settings = new Core.RequestContextSettings();

        /// <summary>
        /// To persist session cookies (cookies without an expiry date or validity
        /// interval) by default when using the global cookie manager set this value to
        /// true. Session cookies are generally intended to be transient and most
        /// Web browsers do not persist them. Can be set globally using the
        /// CefSettings.PersistSessionCookies value. This value will be ignored if
        /// CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        public bool PersistSessionCookies
        {
            get { return settings.PersistSessionCookies; }
            set { settings.PersistSessionCookies = value; }
        }

        /// <summary>
        /// To persist user preferences as a JSON file in the cache path directory set
        /// this value to true. Can be set globally using the
        /// CefSettings.PersistUserPreferences value. This value will be ignored if
        /// CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        public bool PersistUserPreferences
        {
            get { return settings.PersistUserPreferences; }
            set { settings.PersistUserPreferences = value; }
        }

        /// <summary>
        /// The location where cache data for this request context will be stored on
        /// disk. If this value is non-empty then it must be an absolute path that is
        /// either equal to or a child directory of CefSettings.RootCachePath.
        /// If the value is empty then browsers will be created in "incognito mode"
        /// where in-memory caches are used for storage and no data is persisted to disk.
        /// HTML5 databases such as localStorage will only persist across sessions if a
        /// cache path is specified. To share the global browser cache and related
        /// configuration set this value to match the CefSettings.CachePath value.
        /// </summary>
        public String CachePath
        {
            get { return settings.CachePath; }
            set { settings.CachePath = value; }
        }

        /// <summary>
        /// Comma delimited ordered list of language codes without any whitespace that
        /// will be used in the "Accept-Language" HTTP header. Can be set globally
        /// using the CefSettings.accept_language_list value or overridden on a per-
        /// browser basis using the BrowserSettings.AcceptLanguageList value. If
        /// all values are empty then "en-US,en" will be used. This value will be
        /// ignored if CachePath matches the CefSettings.CachePath value.
        /// </summary>
        public String AcceptLanguageList
        {
            get { return settings.AcceptLanguageList; }
            set { settings.AcceptLanguageList = value; }
        }

        /// <summary>
        /// Set to true to ignore errors related to invalid SSL certificates.
        /// Enabling this setting can lead to potential security vulnerabilities like
        /// "man in the middle" attacks. Applications that load content from the
        /// internet should not enable this setting. Can be set globally using the
        /// CefSettings.IgnoreCertificateErrors value. This value will be ignored if
        /// CachePath matches the CefSettings.cache_path value.
        /// </summary>
        public bool IgnoreCertificateErrors
        {
            get { return settings.IgnoreCertificateErrors; }
            set { settings.IgnoreCertificateErrors = value; }
        }
    }
}
