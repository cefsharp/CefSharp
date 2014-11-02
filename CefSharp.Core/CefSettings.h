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
    public ref class CefSettings
    {
    private:
        List<CefCustomScheme^>^ cefCustomSchemes;
        IDictionary<String^, String^>^ cefCommandLineArgs;

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
            case LOGSEVERITY_DISABLE:
            default:
                return CefSharp::LogSeverity::Disable;
            }
        }

    public:
        CefSettings() : _cefSettings(new ::CefSettings())
        {
            _cefSettings->multi_threaded_message_loop = true;
            BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe";
            cefCustomSchemes = gcnew List<CefCustomScheme^>();
            cefCommandLineArgs = gcnew Dictionary<String^, String^>();
        }

        !CefSettings() { delete _cefSettings; }
        ~CefSettings() { delete _cefSettings; }

        virtual property IEnumerable<CefCustomScheme^>^ CefCustomSchemes
        {
            IEnumerable<CefCustomScheme^>^ get() { return cefCustomSchemes; }
        }

        virtual property IDictionary<String^, String^>^ CefCommandLineArgs
        {
            IDictionary<String^, String^>^ get() { return cefCommandLineArgs; }
        }

        virtual property bool MultiThreadedMessageLoop
        {
            bool get() { return _cefSettings->multi_threaded_message_loop == 1; }
        }

        virtual property String^ BrowserSubprocessPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->browser_subprocess_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->browser_subprocess_path, value); }
        }

        virtual property String^ CachePath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->cache_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->cache_path, value); }
        }

        virtual property bool IgnoreCertificateErrors
        {
            bool get() { return _cefSettings->ignore_certificate_errors == 1; }
            void set(bool value) { _cefSettings->ignore_certificate_errors = value; }
        }

        virtual property String^ Locale
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->locale); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->locale, value); }
        }

        virtual property String^ LocalesDirPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->locales_dir_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->locales_dir_path, value); }
        }

        virtual property String^ LogFile
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->log_file); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->log_file, value); }
        }

        virtual property CefSharp::LogSeverity LogSeverity
        {
            CefSharp::LogSeverity get() { return SeverityToManaged(_cefSettings->log_severity); }
            void set(CefSharp::LogSeverity value) { _cefSettings->log_severity = SeverityToNative(value); }
        }

        virtual property bool PackLoadingDisabled
        {
            bool get() { return _cefSettings->pack_loading_disabled == 1; }
            void set(bool value) { _cefSettings->pack_loading_disabled = value; }
        }

        virtual property String^ ProductVersion
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->product_version); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->product_version, value); }
        }

        virtual property int RemoteDebuggingPort
        {
            int get() { return _cefSettings->remote_debugging_port; }
            void set(int value) { _cefSettings->remote_debugging_port = value; }
        }

        virtual property String^ UserAgent
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->user_agent); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->user_agent, value); }
        }

        /// <summary>
        /// Registers a custom scheme using the provided settings.
        /// </summary>
        /// <param name="cefCustomScheme">The CefCustomScheme which provides the details about the scheme.</param>
        void RegisterScheme(CefCustomScheme^ cefCustomScheme)
        {
            cefCustomSchemes->Add(cefCustomScheme);
        }
    };
}
