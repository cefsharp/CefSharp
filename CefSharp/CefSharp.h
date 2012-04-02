#include "stdafx.h"
#pragma once

#include "include/cef_app.h"
#include "include/cef_cookie.h"
#include "include/cef_version.h"
#include "include/cef_runnable.h"
#include "include/cef_task.h"
#include "Settings.h"
#include "SchemeHandler.h"
#include "StringUtil.h"
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::IO;
using namespace System::Collections::Generic;
using namespace msclr::interop;

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

        // this method is meant to be called on the IO-thread, which we'll do through CefPostTask
    	static void IOT_SetCookie(const CefString& url, const CefCookie& cookie)
		{
			CefSetCookie(url, cookie);
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
                Assembly^ assembly = Assembly::GetAssembly(CEF::typeid);
                return assembly->GetName()->Version->ToString();
            }
        }

        static property String^ CefVersion
        {
            String^ get()
            {
                return String::Format("r{0}", CEF_REVISION);
            }
        }

        static property String^ ChromiumVersion
        {
            String^ get()
            {
                return String::Format("{0}.{1}.{2}.{3}",
                    CHROME_VERSION_MAJOR, CHROME_VERSION_MINOR,
                    CHROME_VERSION_BUILD, CHROME_VERSION_PATCH);
            }
        }

        static bool Initialize(Settings^ settings)
        {
            bool success = false;
            if (!IsInitialized)
            {
                success = CefInitialize(*settings->_cefSettings, nullptr);
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
        
    	static bool SetCookiePath(String^ path)
		{
			CefString cef_path = toNative(path);
			return CefSetCookiePath(cef_path);
		}
        
    	static void SetCookie(String^ url, String^ domain, String^ name, String^ value, DateTime^ expires)
		{
			CefString cef_url = toNative(url);
			
			CefCookie cookie;
			CefString(&cookie.name).FromString(marshal_as<std::string>(name));
			CefString(&cookie.value).FromString(marshal_as<std::string>(value));
			CefString(&cookie.domain).FromString(marshal_as<std::string>(domain));
			CefString(&cookie.path).FromString("/");
			cookie.has_expires = true;
			cookie.expires.year = expires->Year;
			cookie.expires.month = expires->Month;
			cookie.expires.day_of_month = expires->Day;

			CefPostTask(TID_IO, NewCefRunnableFunction(IOT_SetCookie, cef_url, cookie));
		}
    };
}
