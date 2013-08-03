// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <msclr/lock.h>
#include <include/cef_version.h>
#include <include/cef_runnable.h>

#include "Internals/CefSharpApp.h"
#include "Internals/CookieVisitor.h"
#include "Internals/StringUtils.h"
#include "CefBrowserWrapper.h"
#include "CefErrorCode.h"
#include "CefSettings.h"
#include "SchemeHandler.h"

using namespace System::Collections::Generic;
using namespace System::Reflection;

namespace CefSharp
{
    public ref class Cef sealed
    {
    private:
        static HANDLE _event;
        static Object^ _sync;
        static bool _result;

        static bool _initialized = false;
        static IDictionary<String^, Object^>^ _boundObjects;

        static Cef()
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

        // TODO: Add proper XML comments for all of these, so we can get proper IntelliSense in C# code.
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
                Assembly^ assembly = Assembly::GetAssembly(Cef::typeid);
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
                // Need explicit cast here to avoid C4965 warning when the minor version is zero.
                return String::Format("{0}.{1}.{2}.{3}",
                    CHROME_VERSION_MAJOR, (Object^) CHROME_VERSION_MINOR,
                    CHROME_VERSION_BUILD, CHROME_VERSION_PATCH);
            }
        }

        static bool Initialize()
        {
            auto cefSettings = gcnew CefSettings();
            return Initialize(cefSettings);
        }

        static bool Initialize(CefSettings^ cefSettings)
        {
            bool success = false;

            // TODO: is it really sensible to completely skip initialization if we get called multiple times, but with
            // (potentially) different settings...? :)
            if (!IsInitialized)
            {
                CefMainArgs main_args;
                CefRefPtr<CefSharpApp> app(new CefSharpApp);

                int exitCode = CefExecuteProcess(main_args, app.get());
        
                if (exitCode >= 0)
                {
                    // Something went "wrong", but it may also be caused in the case where we are the secondary process, so we
                    // can't really throw exceptions or anything like that.
                    return false;
                }

                success = CefInitialize(main_args, *(cefSettings->_cefSettings), app.get());
                _initialized = success;
            }

            return success;
        }

        static bool RegisterScheme(String^ schemeName, String^ hostName, bool is_standard, ISchemeHandlerFactory^ factory)
        {
            hostName = hostName ? hostName : String::Empty;

            CefRefPtr<CefSchemeHandlerFactory> wrapper = new SchemeHandlerFactoryWrapper(factory);
            return CefRegisterSchemeHandlerFactory(StringUtils::ToNative(schemeName), StringUtils::ToNative(hostName), wrapper);
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
            CefRefPtr<CefCookieManager> manager = CefCookieManager::GetGlobalManager();

            if (manager != nullptr)
            {
                return manager->VisitAllCookies(static_cast<CefRefPtr<CefCookieVisitor>>(cookieVisitor));
            }
            else
            {
                return false;
            }
        }

        static bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor)
        {
            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);
            CefRefPtr<CefCookieManager> manager = CefCookieManager::GetGlobalManager();

            if (manager != nullptr)
            {
                return manager->VisitUrlCookies(StringUtils::ToNative(url), includeHttpOnly,
                    static_cast<CefRefPtr<CefCookieVisitor>>(cookieVisitor));
            }
            else
            {
                return false;
            }
        }

        static bool SetCookie(String^ url, String^ name, String^ value, String^ domain, String^ path, bool secure, bool httponly, bool has_expires, DateTime expires)
        {
            msclr::lock l(_sync);
            _result = false;

            CefCookie cookie;
            StringUtils::AssignNativeFromClr(cookie.name, name);
            StringUtils::AssignNativeFromClr(cookie.value, value);
            StringUtils::AssignNativeFromClr(cookie.domain, domain);
            StringUtils::AssignNativeFromClr(cookie.path, path);
            cookie.secure = secure;
            cookie.httponly = httponly;
            cookie.has_expires = has_expires;
            cookie.expires.year = expires.Year;
            cookie.expires.month = expires.Month;
            cookie.expires.day_of_month = expires.Day;

            if (CefCurrentlyOn(TID_IO))
            {
                IOT_SetCookie(StringUtils::ToNative(url), cookie);
            }
            else
            {
                CefPostTask(TID_IO, NewCefRunnableFunction(IOT_SetCookie,
                    StringUtils::ToNative(url), cookie));
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
                IOT_DeleteCookies(StringUtils::ToNative(url), StringUtils::ToNative(name));
            }
            else
            {
                CefPostTask(TID_IO, NewCefRunnableFunction(IOT_DeleteCookies,
                    StringUtils::ToNative(url), StringUtils::ToNative(name)));
            }

            WaitForSingleObject(_event, INFINITE);
            return _result;
        }

        static bool SetCookiePath(String^ path, bool persistSessionCookies)
        {
            CefRefPtr<CefCookieManager> manager = CefCookieManager::GetGlobalManager();

            if (manager != nullptr)
            {
                return manager->SetStoragePath(StringUtils::ToNative(path), persistSessionCookies);
            }
            else
            {
                return false;
            }
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