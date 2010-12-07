// CefSharp.h

#include "stdafx.h"
#pragma once

#include "BrowserControl.h"
#include "Request.h"
#include "ReturnValue.h"

using namespace System;
using namespace System::IO;

namespace CefSharp 
{
    public ref class CEF sealed
    {
    public:
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
            return CefInitialize(*settings->_cefSettings, *browserSettings->_browserSettings);
        }

        static void Shutdown()
        {
            CefShutdown();
        }
    };
}
