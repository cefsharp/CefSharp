// CefSharp.h
#include "stdafx.h"
#pragma once

#include "Settings.h"
#include "SchemeHandler.h"

using namespace System;
using namespace System::IO;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public ref class CEF sealed
    {
    private:
        static bool _initialized = false;
        static IDictionary<String^, Object^>^ _boundObjects;

        static CEF()
        {
            _boundObjects = gcnew Dictionary<String^, Object^>();
        }

    internal:
        static IDictionary<String^, Object^>^ GetBoundObjects()
        {
            return _boundObjects;
        }

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
                return "0.3";
            }
        }

        static property String^ CefVersion
        {
            String^ get()
            {
                return "trunk r293";
            }
        }

        static property String^ ChromiumVersion
        {
            String^ get()
            {
                return "trunk r102269";
            }
        }

        static bool Initialize(Settings^ settings)
        {
            bool success = false;
            if (!IsInitialized)
            {
                success = CefInitialize(*settings->_cefSettings);
                _initialized = success;
            }
            return success;
        }

        static bool RegisterScheme(String^ schemeName, String^ hostName, bool is_standard, ISchemeHandlerFactory^ factory)
        {
            hostName = hostName ? hostName : "";

            CefRefPtr<CefSchemeHandlerFactory> wrapper = new SchemeHandlerFactoryWrapper(factory);
            CefRegisterCustomScheme(toNative(schemeName), is_standard, false, false);
            return CefRegisterSchemeHandlerFactory(toNative(schemeName), toNative(hostName), wrapper);
        }

        static bool RegisterScheme(String^ schemeName, ISchemeHandlerFactory^ factory)
        {
            return RegisterScheme(schemeName, nullptr, true, factory);
        }

        static bool RegisterJsObject(String^ name, Object^ objectToBind)
        {
            _boundObjects[name] = objectToBind;
            return true;
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
