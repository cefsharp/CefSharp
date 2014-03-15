// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <msclr/lock.h>
#include <include/cef_version.h>
#include <include/cef_runnable.h>

#include "Internals/CefSharpApp.h"
#include "Internals/CookieVisitor.h"
#include "Internals/StringUtils.h"
#include "ManagedCefBrowserAdapter.h"
#include "CefSettings.h"
#include "SchemeHandlerWrapper.h"
#include "UnmanagedTaskWrapper.h"

using namespace System::Collections::Generic;
using namespace System::Reflection;
using namespace System::ComponentModel::Composition;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    public ref class Cef : CefManagedBase
    {
    private:

        static Cef()
        {
            Instance = gcnew Cef();
            CefManagedBase::Instance = Instance;
        };

        Cef()
        {
            IOTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_IO));
            RenderTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_RENDERER));
        }


    public:
        
        static Cef^ Instance;

        /// <summary>Gets a value that indicates the CEF version currently being used.</summary>
        /// <value>The CEF Version</value>
        static property Version^ CefVersion
        {
            Version^ get()
            {
                return gcnew Version(CEF_REVISION, 0);
            }
        }

        /// <summary>Gets a value that indicates the Chromium version currently being used.</summary>
        /// <value>The Chromium version.</value>
        static property Version^ ChromiumVersion
        {
            Version^ get()
            {
                return gcnew Version(
                    CHROME_VERSION_MAJOR, CHROME_VERSION_MINOR,
                    CHROME_VERSION_BUILD, CHROME_VERSION_PATCH);
            }
        }

        bool Initialize()
        {
            return Initialize(gcnew CefSettings());
        }

        /// <summary>Initializes CefSharp with user-provided settings.</summary>
        ///<param name="cefSettings">CefSharp configuration settings.</param>
        /// <return>true if successful; otherwise, false.</return>
        virtual bool DoInitialize(CefSettingsBase^ cefSettings) override
        {
            bool success = false;

            auto realSettings = (CefSettings^)cefSettings;

            CefMainArgs main_args;
            CefRefPtr<CefSharpApp> app(new CefSharpApp(realSettings));

            int exitCode = CefExecuteProcess(main_args, app.get());

            if (exitCode >= 0)
            {
                // Something went "wrong", but it may also be caused in the case where we are the secondary process, so we
                // can't really throw exceptions or anything like that.
                return false;
            }

            success = CefInitialize(main_args, *realSettings->_cefSettings, app.get());
            app->CompleteSchemeRegistrations();

            return success;
        }

        /// <summary>Visits all cookies using the provided Cookie Visitor. The returned cookies are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
        virtual bool VisitAllCookies(ICookieVisitor^ visitor) override
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

        /// <summary>Visits a subset of the cookies. The results are filtered by the given url scheme, host, domain and path. 
        /// If <paramref name="includeHttpOnly"/> is true, HTTP-only cookies will also be included in the results. The returned cookies 
        /// are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
        /// <param name="includeHttpOnly">A flag that determines whether HTTP-only cookies will be shown in results.</param>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
        virtual bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor) override
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

        /// <summary>Sets a CefSharp Cookie. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        /// <return>false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used); otherwise true.</return>
        virtual bool DoSetCookie(String^ url, String^ name, String^ value, String^ domain, String^ path, bool secure, bool httponly, bool has_expires, DateTime expires) override
        {
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

            return CefCookieManager::GetGlobalManager()->SetCookie(StringUtils::ToNative(url), cookie);
        }

        /// <summary>Deletes all cookies that matches all the provided parameters. If both <paramref name="url"/> and <paramref name="name"/> are empty, all cookies will be deleted.</summary>
        /// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
        /// <param name="name">The name of the cookie. If an empty string is provided, any URL will be matched.</param>
        /// <return>false if a non-empty invalid URL is specified, or if cookies cannot be accessed; otherwise, true.</return>
        virtual bool DoDeleteCookies(String^ url, String^ name) override
        {
            return CefCookieManager::GetGlobalManager()->DeleteCookies(StringUtils::ToNative(url), StringUtils::ToNative(name));;
        }

        /// <summary> Sets the directory path that will be used for storing cookie data. If <paramref name="path"/> is empty data will be stored in 
        /// memory only. Otherwise, data will be stored at the specified path. To persist session cookies (cookies without an expiry 
        /// date or validity interval) set <paramref name="persistSessionCookies"/> to true. Session cookies are generally intended to be transient and 
        /// most Web browsers do not persist them.</summary>
        /// <param name="path">The file path to write cookies to.</param>
        /// <param name="persistSessionCookies">A flag that determines whether session cookies will be persisted or not.</param>
        /// <return> false if a non-empty invalid URL is specified, or if the CookieManager is not available; otherwise, true.</return>
        virtual bool DoSetCookiePath(String^ path, bool persistSessionCookies) override
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

        virtual void DoDispose(bool disposing) override
        {
            GC::Collect();
            GC::WaitForPendingFinalizers();

            CefShutdown();

            CefManagedBase::DoDispose(disposing);
        };
    };
}
