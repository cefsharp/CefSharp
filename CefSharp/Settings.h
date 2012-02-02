#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public enum class LogSeverity : int 
    {
        Verbose = LOGSEVERITY_VERBOSE,
        Info = LOGSEVERITY_INFO,
        Warning = LOGSEVERITY_WARNING,
        Error = LOGSEVERITY_ERROR,
        ErrorReport = LOGSEVERITY_ERROR_REPORT,
        Disable = LOGSEVERITY_DISABLE // Disables logging completely.
    };
    
    public ref class Settings
    {
    internal:
        CefSettings* _cefSettings;

        // CefSharp doesn't support single thread message loop yet
        property bool MultiThreadedMessageLoop
        {
            bool get() { return _cefSettings->multi_threaded_message_loop; }
            void set(bool val) { _cefSettings->multi_threaded_message_loop = val; }
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

        !Settings()    { delete _cefSettings; }
        ~Settings() { delete _cefSettings; }

        property String^ CachePath
        {
            String^ get()
            {
                return toClr(_cefSettings->cache_path);
            }

            void set(String^ path)
            {
                assignFromString(_cefSettings->cache_path, path);
            }
        }

        property String^ UserAgent
        {
            String^ get()
            {
                return toClr(_cefSettings->user_agent);
            }

            void set(String^ userAgent)
            {
                assignFromString(_cefSettings->user_agent, userAgent);
            }
        }

        property String^ ProductVersion
        {
            String^ get()
            {
                return toClr(_cefSettings->product_version);
            }

            void set(String^ productVersion)
            {
                assignFromString(_cefSettings->product_version, productVersion);
            }
        }

        property String^ Locale
        {
            String^ get()
            {
                return toClr(_cefSettings->locale);
            }

            void set(String^ locale)
            {
                assignFromString(_cefSettings->locale, locale);
            }
        }

        void AddPluginPath(String^ path)
        {
            cef_string_t str;

            memset(&str, 0, sizeof(cef_string_t));
            assignFromString(str, path);

            AddPluginPath(&str);
            // XXX: free?
        }

        property String^ LogFile
        {
            String^ get()
            {
                return toClr(_cefSettings->log_file);
            }

            void set(String^ log_file)
            {
                assignFromString(_cefSettings->log_file, log_file);
            }
        }

        property CefSharp::LogSeverity LogSeverity
        {
            CefSharp::LogSeverity get()
            {
                return static_cast<CefSharp::LogSeverity>(_cefSettings->log_severity);
            }

            void set(CefSharp::LogSeverity log_severity)
            {
                _cefSettings->log_severity = static_cast<cef_log_severity_t>(log_severity);
            }
        }
    };

}
