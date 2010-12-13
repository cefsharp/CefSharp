// CefSharp.h
#include "stdafx.h"
#pragma once

#include "Settings.h"
#include "BrowserSettings.h"
#include "SchemeHandler.h"

using namespace System;
using namespace System::IO;

namespace CefSharp 
{
    public ref class CEF sealed
    {
    private:
        static bool _initialized = false;

    public:
        static property bool IsInitialized
        {
            bool get()
            {
                return _initialized;
            }
        }

        static property String^ CefSharpVersion
        {
            String^ get()
            {
                return "0.1";
            }
        }

        static property String^ CefVersion
        {
            String^ get()
            {
                return "trunk r149";
            }
        }

        static property String^ ChromiumVersion
        {
            String^ get()
            {
                return "trunk r66269";
            }
        }

        static bool Initialize(Settings^ settings, BrowserSettings^ browserSettings)
        {
            bool success = false;
            if (!IsInitialized)
            {
                success = CefInitialize(*settings->_cefSettings, *browserSettings->_browserSettings);
                _initialized = success;
            }
            return success;
        }

        static bool RegisterScheme(String^ schemeName, String^ hostName, ISchemeHandlerFactory^ factory)
        {
            hostName = hostName ? hostName : "";

            CefRefPtr<SchemeHandlerFactoryWrapper> wrapper = new SchemeHandlerFactoryWrapper(factory);
            return CefRegisterScheme(convertFromString(schemeName), convertFromString(hostName), static_cast<CefRefPtr<CefSchemeHandlerFactory>>(wrapper));
        }

        static bool RegisterScheme(String^ schemeName, ISchemeHandlerFactory^ factory)
        {
            return RegisterScheme(schemeName, nullptr, factory);
        }


        static void Shutdown()
        {
            if (IsInitialized)
            {
                CefShutdown();
            }
        }
    };
}
