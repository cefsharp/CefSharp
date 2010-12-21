#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp 
{
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

    public:
        Settings() : _cefSettings(new CefSettings())
        {
            MultiThreadedMessageLoop = true;
        }

        !Settings()	{ delete _cefSettings; }
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
    };

}