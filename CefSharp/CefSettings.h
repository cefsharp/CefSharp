// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;
using namespace CefSharp::Internals;

namespace CefSharp
{
    public enum class LogSeverity
    {
        Verbose = LOGSEVERITY_VERBOSE,
        Info = LOGSEVERITY_INFO,
        Warning = LOGSEVERITY_WARNING,
        Error = LOGSEVERITY_ERROR,
        ErrorReport = LOGSEVERITY_ERROR_REPORT,
        Disable = LOGSEVERITY_DISABLE,
    };

    public ref class CefSettings
    {
    internal:
        ::CefSettings* _cefSettings;

        property bool MultiThreadedMessageLoop
        {
            bool get() { return _cefSettings->multi_threaded_message_loop; }

            // CefSharp doesn't support single threaded message loop (and there's little point in supporting it), so we make this
            // property read-only externally.
        private:
            void set(bool value) { _cefSettings->multi_threaded_message_loop = value; }
        }

    public:
        CefSettings() : _cefSettings(new ::CefSettings())
        {
            MultiThreadedMessageLoop = true;
        }

        !CefSettings() { delete _cefSettings; }
        ~CefSettings() { delete _cefSettings; }

        property String^ CachePath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->cache_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->cache_path, value); }
        }

        property String^ Locale
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->locale); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->locale, value); }
        }

        property String^ LocalesDirPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->locales_dir_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->locales_dir_path, value); }
        }

        property String^ LogFile
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->log_file); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->log_file, value); }
        }

        property CefSharp::LogSeverity LogSeverity
        {
            CefSharp::LogSeverity get() { return static_cast<CefSharp::LogSeverity>(_cefSettings->log_severity); }
            void set(CefSharp::LogSeverity value) { _cefSettings->log_severity = static_cast<cef_log_severity_t>(value); }
        }

        property bool PackLoadingDisabled
        {
            bool get() { return _cefSettings->pack_loading_disabled; }
            void set(bool value) { _cefSettings->pack_loading_disabled = value; }
        }

        property String^ ProductVersion
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->product_version); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->product_version, value); }
        }

        property int RemoteDebuggingPort
        {
            int get() { return _cefSettings->remote_debugging_port; }
            void set(int value) { _cefSettings->remote_debugging_port = value; }
        }

        property String^ UserAgent
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->user_agent); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->user_agent, value); }
        }
    };
}
