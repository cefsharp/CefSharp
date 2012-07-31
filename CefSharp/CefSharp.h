#include "stdafx.h"
#pragma once

#include <msclr/lock.h>
#include "include/cef_app.h"
#include "include/cef_cookie.h"
#include "include/cef_runnable.h"
#include "include/cef_version.h"
#include "include/cef_task.h"
#include "CookieVisitor.h"
#include "Settings.h"
#include "SchemeHandler.h"
#include "StringUtil.h"

using namespace System;
using namespace System::IO;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public ref class CEF sealed
    {
    private:
        static HANDLE _event;
        static Object^ _sync;
        static bool _result;

        static bool _initialized = false;
        static IDictionary<String^, Object^>^ _boundObjects;

        static CEF()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
            _sync = gcnew Object();
            _boundObjects = gcnew Dictionary<String^, Object^>();
        }

        static void IOT_SetCookie(const CefString& url, const CefCookie& cookie)
        {
            _result = CefCookieManager::GetGlobalManager()->SetCookie(url, cookie);
            SetEvent(_event);
        }

        static void IOT_DeleteCookies(const CefString& url, const CefString& name)
        {
            _result = CefCookieManager::GetGlobalManager()->DeleteCookies(url, name);
            SetEvent(_event);
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
            hostName = hostName ? hostName : String::Empty;

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

        static bool VisitAllCookies(ICookieVisitor^ visitor)
        {
            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);
            return CefCookieManager::GetGlobalManager()->
                VisitAllCookies(static_cast<CefRefPtr<CefCookieVisitor>>(cookieVisitor));
        }

        static bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor)
        {
            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);
            return CefCookieManager::GetGlobalManager()->
                VisitUrlCookies(toNative(url), includeHttpOnly, static_cast<CefRefPtr<CefCookieVisitor>>(cookieVisitor));
        }

        static bool SetCookie(String^ url, String^ name, String^ value, String^ domain, String^ path, bool secure, bool httponly, bool has_expires, DateTime expires)
        {
            msclr::lock l(_sync);
            _result = false;

            CefCookie cookie;
            assignFromString(cookie.name, name);
            assignFromString(cookie.value, value);
            assignFromString(cookie.domain, domain);
            assignFromString(cookie.path, path);
            cookie.secure = secure;
            cookie.httponly = httponly;
            cookie.has_expires = has_expires;
            cookie.expires.year = expires.Year;
            cookie.expires.month = expires.Month;
            cookie.expires.day_of_month = expires.Day;

            if (CefCurrentlyOn(TID_IO))
            {
                IOT_SetCookie(toNative(url), cookie);
            }
            else
            {
                CefPostTask(TID_IO, NewCefRunnableFunction(IOT_SetCookie,
                    toNative(url), cookie));
            }

            WaitForSingleObject(_event, INFINITE);
            return _result;
        }

        static bool SetCookie(String^ url, String^ domain, String^ name, String^ value, DateTime expires)
        {
            return SetCookie(url, name, value, domain, "/", false, false, true, expires);
        }

        static bool DeleteCookies(String^ url, String^ name)
        {
            msclr::lock l(_sync);
            _result = false;

            if (CefCurrentlyOn(TID_IO))
            {
                IOT_DeleteCookies(toNative(url), toNative(name));
            }
            else
            {
                CefPostTask(TID_IO, NewCefRunnableFunction(IOT_DeleteCookies,
                    toNative(url), toNative(name)));
            }

            WaitForSingleObject(_event, INFINITE);
            return _result;
        }

        static bool SetCookiePath(String^ path)
        {
            return CefCookieManager::GetGlobalManager()->SetStoragePath(toNative(path));
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
