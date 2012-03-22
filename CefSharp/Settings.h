#include "stdafx.h"
#pragma once

using namespace System;

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
    
    public ref class Settings
    {
    internal:
        CefSettings* _cefSettings;

        // CefSharp doesn't support single thread message loop yet
        property bool MultiThreadedMessageLoop
        {
            bool get() { return _cefSettings->multi_threaded_message_loop; }
            void set(bool value) { _cefSettings->multi_threaded_message_loop = value; }
        }

        void AddPluginPath(const cef_string_t *path)
        {
            if (!_cefSettings->extra_plugin_paths)
            {
                _cefSettings->extra_plugin_paths = cef_string_list_alloc();
            }

            cef_string_list_append(_cefSettings->extra_plugin_paths, path);
        }

    public:
        Settings() : _cefSettings(new CefSettings())
        {
            MultiThreadedMessageLoop = true;
        }

        !Settings() { delete _cefSettings; }
        ~Settings() { delete _cefSettings; }

        property String^ CachePath
        {
            String^ get() { return toClr(_cefSettings->cache_path); }
            void set(String^ value) { assignFromString(_cefSettings->cache_path, value); }
        }

        property String^ UserAgent
        {
            String^ get() { return toClr(_cefSettings->user_agent); }
            void set(String^ value) { assignFromString(_cefSettings->user_agent, value); }
        }

        property String^ ProductVersion
        {
            String^ get() { return toClr(_cefSettings->product_version); }
            void set(String^ value) { assignFromString(_cefSettings->product_version, value); }
        }

        property String^ Locale
        {
            String^ get() { return toClr(_cefSettings->locale); }
            void set(String^ value) { assignFromString(_cefSettings->locale, value); }
        }

        void AddPluginPath(String^ path)
        {
            cef_string_t str;

            memset(&str, 0, sizeof(cef_string_t));
            assignFromString(str, path);

            AddPluginPath(&str);
        }

        property String^ LogFile
        {
            String^ get() { return toClr(_cefSettings->log_file); }
            void set(String^ value) { assignFromString(_cefSettings->log_file, value); }
        }

        property CefSharp::LogSeverity LogSeverity
        {
            CefSharp::LogSeverity get() { return static_cast<CefSharp::LogSeverity>(_cefSettings->log_severity); }
            void set(CefSharp::LogSeverity value) { _cefSettings->log_severity = static_cast<cef_log_severity_t>(value); }
        }

        property bool AutoDetectProxySettings
        {
            bool get() { return _cefSettings->auto_detect_proxy_settings_enabled; }
            void set(bool value) { _cefSettings->auto_detect_proxy_settings_enabled = value; }
        }

        property String^ PackFilePath
        {
            String^ get() { return toClr(_cefSettings->pack_file_path); }
            void set(String^ value) { assignFromString(_cefSettings->pack_file_path, value); }
        }

        property String^ LocalesDirPath
        {
            String^ get() { return toClr(_cefSettings->locales_dir_path); }
            void set(String^ value) { assignFromString(_cefSettings->locales_dir_path, value); }
        }

        property bool PackLoadingDisabled
        {
            bool get() { return _cefSettings->pack_loading_disabled; }
            void set(bool value) { _cefSettings->pack_loading_disabled = value; }
        }
    };
}