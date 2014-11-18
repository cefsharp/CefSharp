// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <msclr/lock.h>
#include <include/cef_version.h>
#include <include/cef_runnable.h>

#include "Internals/CefSharpApp.h"
#include "Internals/CookieVisitor.h"
#include "Internals/CompletionHandler.h"
#include "Internals/StringUtils.h"
#include "ManagedCefBrowserAdapter.h"
#include "CefSettings.h"
#include "SchemeHandlerWrapper.h"

using namespace System::Collections::Generic; 
using namespace System::Linq;
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
        static HashSet<IDisposable^>^ _disposables;

        static Cef()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
            _sync = gcnew Object();
            _disposables = gcnew HashSet<IDisposable^>();
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

        static void ParentProcessExitHandler(Object^ sender, EventArgs^ e)
        {
            if (Cef::IsInitialized)
            {
                Cef::Shutdown();
            }
        }

    public:
        static void AddDisposable(IDisposable^ item)
        {
            msclr::lock l(_sync);

            _disposables->Add(item);
        }

        static void RemoveDisposable(IDisposable^ item)
        {
            msclr::lock l(_sync);

            _disposables->Remove(item);
        }

        /// <summary>Gets a value that indicates whether CefSharp is initialized.</summary>
        /// <value>true if CefSharp is initialized; otherwise, false.</value>
        static property bool IsInitialized
        {
            bool get()
            {
                return _initialized;
            }

        private:
            void set(bool value)
            {
                _initialized = value;
            }
        }

        /// <summary>Gets a value that indicates the version of CefSharp currently being used.</summary>
        /// <value>The CefSharp version.</value>
        static property String^ CefSharpVersion
        {
            String^ get()
            {
                Assembly^ assembly = Assembly::GetAssembly(Cef::typeid);
                return assembly->GetName()->Version->ToString();
            }
        }

        /// <summary>Gets a value that indicates the CEF version currently being used.</summary>
        /// <value>The CEF Version</value>
        static property String^ CefVersion
        {
            String^ get()
            {
                return String::Format("r{0}", CEF_REVISION);
            }
        }

        /// <summary>Gets a value that indicates the Chromium version currently being used.</summary>
        /// <value>The Chromium version.</value>
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

        /// <summary>Initializes CefSharp with the default settings.</summary>
        /// <return>true if successful; otherwise, false.</return>
        static bool Initialize()
        {
            auto cefSettings = gcnew CefSettings();
            return Initialize(cefSettings);
        }

        /// <summary>Initializes CefSharp with user-provided settings.</summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <return>true if successful; otherwise, false.</return>
        static bool Initialize(CefSettings^ cefSettings)
        {
            bool success = false;

            // TODO: is it really sensible to completely skip initialization if we get called multiple times, but with
            // (potentially) different settings...? :)
            if (!IsInitialized)
            {
                CefMainArgs main_args;
                CefRefPtr<CefSharpApp> app(new CefSharpApp(cefSettings));

                int exitCode = CefExecuteProcess(main_args, app.get(), NULL);

                if (exitCode >= 0)
                {
                    // Something went "wrong", but it may also be caused in the case where we are the secondary process, so we
                    // can't really throw exceptions or anything like that.
                    return false;
                }

                success = CefInitialize(main_args, *(cefSettings->_cefSettings), app.get(), NULL);
                app->CompleteSchemeRegistrations();
                _initialized = success;

                if (_initialized)
                {
                    AppDomain::CurrentDomain->ProcessExit += gcnew EventHandler(ParentProcessExitHandler);
                }
            }

            return success;
        }

        /// <summary>Visits all cookies using the provided Cookie Visitor. The returned cookies are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
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

        /// <summary>Visits a subset of the cookies. The results are filtered by the given url scheme, host, domain and path. 
        /// If <paramref name="includeHttpOnly"/> is true, HTTP-only cookies will also be included in the results. The returned cookies 
        /// are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="url">The URL to use for filtering a subset of the cookies available.</param>
        /// <param name="includeHttpOnly">A flag that determines whether HTTP-only cookies will be shown in results.</param>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
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

        /// <summary>Sets a CefSharp Cookie. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        /// <param name="url">The cookie URL</param>
        /// <param name="name">The cookie name</param>
        /// <param name="value">The cookie value</param>
        /// <param name="domain">The cookie domain</param>
        /// <param name="path">The cookie path</param>
        /// <param name="secure">A flag that determines whether the cookie will be marked as "secure" (i.e. its scope is limited to secure channels, typically HTTPS).</param>
        /// <param name="httponly">A flag that determines whether the cookie will be marked as HTTP Only (i.e. the cookie is inaccessible to client-side scripts).</param>
        /// <param name="has_expires">A flag that determines whether the cookie has an expiration date. Must be set to true when the "expires" parameter is provided.</param>
        /// <param name="expires">The DateTime when the cookie will be treated as expired. Will only be taken into account if the "has_expires" is set to true.</param>
        /// <return>false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used); otherwise true.</return>
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
            cookie.expires.hour = expires.Hour;
            cookie.expires.minute = expires.Minute;
            cookie.expires.second = expires.Second;
            cookie.expires.millisecond = expires.Millisecond;

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

        /// <summary>Sets a cookie using mostly default parameters. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        /// <param name="url">The cookie URL</param>
        /// <param name="name">The cookie name.</param>
        /// <param name="value">The cookie value.</param>
        /// <param name="domain">The cookie domain.</param>
        /// <param name="expires">The DateTime when the cookie will be treated as expired.</param>
        /// <return>false if the cookie cannot be set (e.g. if illegal charecters such as ';' are used); otherwise true.</return>
        static bool SetCookie(String^ url, String^ domain, String^ name, String^ value, DateTime expires)
        {
            return SetCookie(url, name, value, domain, "/", false, false, true, expires);
        }

        /// <summary>Deletes all cookies that matches all the provided parameters. If both <paramref name="url"/> and <paramref name="name"/> are empty, all cookies will be deleted.</summary>
        /// <param name="url">The cookie URL. If an empty string is provided, any URL will be matched.</param>
        /// <param name="name">The name of the cookie. If an empty string is provided, any URL will be matched.</param>
        /// <return>false if a non-empty invalid URL is specified, or if cookies cannot be accessed; otherwise, true.</return>
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

        /// <summary> Sets the directory path that will be used for storing cookie data. If <paramref name="path"/> is empty data will be stored in 
        /// memory only. Otherwise, data will be stored at the specified path. To persist session cookies (cookies without an expiry 
        /// date or validity interval) set <paramref name="persistSessionCookies"/> to true. Session cookies are generally intended to be transient and 
        /// most Web browsers do not persist them.</summary>
        /// <param name="path">The file path to write cookies to.</param>
        /// <param name="persistSessionCookies">A flag that determines whether session cookies will be persisted or not.</param>
        /// <return> false if a non-empty invalid URL is specified, or if the CookieManager is not available; otherwise, true.</return>
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

        /// <summary> Flush the backing store (if any) to disk and execute the specified |handler| on the IO thread when done. Returns </summary>
        /// <param name="handler">A user-provided ICompletionHandler implementation.</param>
        /// <return>Returns false if cookies cannot be accessed.</return>
        static bool FlushStore(ICompletionHandler^ handler)
        {
            CefRefPtr<CefCookieManager> manager = CefCookieManager::GetGlobalManager();

            if (manager == nullptr)
            {
                return false;
            }

            auto wrapper = new CompletionHandler(handler);

            return manager->FlushStore(static_cast<CefRefPtr<CefCompletionCallback>>(wrapper));
        }

        /// <summary>Shuts down CefSharp and the underlying CEF infrastructure. This method is safe to call multiple times; it will only
        /// shut down CEF on the first call (all subsequent calls will be ignored).
        /// </summary>
        static void Shutdown()
        {
            if (IsInitialized)
            { 
                {
                    msclr::lock l(_sync);
                    for each(IDisposable^ diposable in Enumerable::ToList(_disposables))
                    {
                        delete diposable;
                    }
                }
                GC::Collect();
                GC::WaitForPendingFinalizers();

                CefShutdown();
                IsInitialized = false;
            }
        }
    };
}
