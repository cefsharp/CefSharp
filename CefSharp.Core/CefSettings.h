// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Internals/StringUtils.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class CefSettings : CefSettingsBase
    {
    internal:
        ::CefSettings* _cefSettings;

        cef_log_severity_t SeverityToNative(CefSharp::LogSeverity severity)
        {
            switch (severity)
            {
            case CefSharp::LogSeverity::Verbose:
                return LOGSEVERITY_VERBOSE;
            case CefSharp::LogSeverity::Info:
                return LOGSEVERITY_INFO;
            case CefSharp::LogSeverity::Warning:
                return LOGSEVERITY_WARNING;
            case CefSharp::LogSeverity::Error:
                return LOGSEVERITY_ERROR;
            case CefSharp::LogSeverity::ErrorReport:
                return LOGSEVERITY_ERROR_REPORT;
            case CefSharp::LogSeverity::Disable:
            default:
                return LOGSEVERITY_DISABLE;
            }
        }

        CefSharp::LogSeverity SeverityToManaged(cef_log_severity_t severity)
        {
            switch (severity)
            {
            case LOGSEVERITY_VERBOSE:
                return CefSharp::LogSeverity::Verbose;
            case LOGSEVERITY_INFO:
                return CefSharp::LogSeverity::Info;
            case LOGSEVERITY_WARNING:
                return CefSharp::LogSeverity::Warning;
            case LOGSEVERITY_ERROR:
                return CefSharp::LogSeverity::Error;
            case LOGSEVERITY_ERROR_REPORT:
                return CefSharp::LogSeverity::ErrorReport;
            case LOGSEVERITY_DISABLE:
            default:
                return CefSharp::LogSeverity::Disable;
            }
        }

    public:
        CefSettings() : _cefSettings(new ::CefSettings())
        {
            _cefSettings->multi_threaded_message_loop = true;
        }

        !CefSettings() { delete _cefSettings; }
        ~CefSettings() { delete _cefSettings; }

        virtual property bool MultiThreadedMessageLoop
        {
            bool get() override { return _cefSettings->multi_threaded_message_loop == 1; }
        }

        virtual property String^ BrowserSubprocessPath
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->browser_subprocess_path); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->browser_subprocess_path, value); }
        }

        virtual property String^ CachePath
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->cache_path); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->cache_path, value); }
        }

        virtual property bool IgnoreCertificateErrors
        {
            bool get() { return _cefSettings->ignore_certificate_errors == 1; }
            void set(bool value) { _cefSettings->ignore_certificate_errors = value; }
        }

        virtual property String^ Locale
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->locale); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->locale, value); }
        }

        virtual property String^ LocalesDirPath
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->locales_dir_path); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->locales_dir_path, value); }
        }

        virtual property String^ LogFile
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->log_file); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->log_file, value); }
        }

        virtual property CefSharp::LogSeverity LogSeverity
        {
            CefSharp::LogSeverity get() override { return SeverityToManaged(_cefSettings->log_severity); }
            void set(CefSharp::LogSeverity value) override { _cefSettings->log_severity = SeverityToNative(value); }
        }

        virtual property bool PackLoadingDisabled
        {
            bool get() override { return _cefSettings->pack_loading_disabled == 1; }
            void set(bool value) override { _cefSettings->pack_loading_disabled = value; }
        }

        virtual property String^ ProductVersion
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->product_version); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->product_version, value); }
        }

        virtual property int RemoteDebuggingPort
        {
            int get() override { return _cefSettings->remote_debugging_port; }
            void set(int value) override { _cefSettings->remote_debugging_port = value; }
        }

        virtual property String^ UserAgent
        {
            String^ get() override { return StringUtils::ToClr(_cefSettings->user_agent); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_cefSettings->user_agent, value); }
        }
    };
}
