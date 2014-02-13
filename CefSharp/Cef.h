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
#include "ManagedCefBrowserAdapter.h"
#include "CefErrorCode.h"
#include "CefSettings.h"
#include "ISchemeHandlerFactory.h"
#include "SchemeHandlerWrapper.h"

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

        static void ParentProcessExitHandler(Object^ sender, EventArgs^ e)
        {
            if (Cef::IsInitialized)
            {
                Cef::Shutdown();
            }
        }

    internal:
        static IDictionary<String^, Object^>^ GetBoundObjects()
        {
            return _boundObjects;
        }

    public:

        /// <summary> Gets whether CefSharp is initialized. </summary>
        /// <value> True if CefSharp is initalized, false otherwise </value>
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

        /// <summary> Returns CefSharp Version </summary>
        /// <value> CefSharp Version as a String</value>
        static property String^ CefSharpVersion
        {
            String^ get()
            {
                Assembly^ assembly = Assembly::GetAssembly(Cef::typeid);
                return assembly->GetName()->Version->ToString();
            }
        }

        /// <summary> Returns CEF Version </summary>
        /// <value>CEF Version as a String</value>
        static property String^ CefVersion
        {
            String^ get()
            {
                return String::Format("r{0}", CEF_REVISION);
            }
        }

        /// <summary> Returns Chromium Version </summary>
        /// <value> Chromium Version as a String</value>
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

        /// <summary> Initialize CefSharp with default settings. </summary>
        /// <return> True if successful and false otherwise.</return>
        static bool Initialize()
        {
            auto cefSettings = gcnew CefSettings();
            return Initialize(cefSettings);
        }

        /// <summary> Initialize CefSharp settings. </summary>
        ///<param name="cefSettings">CefSharp configuration settings.</param>
        /// <return> True if successful and false otherwise.</return>
        static bool Initialize(CefSettings^ cefSettings)
        {
            bool success = false;

            // TODO: is it really sensible to completely skip initialization if we get called multiple times, but with
            // (potentially) different settings...? :)
            if (!IsInitialized)
            {
                CefMainArgs main_args;
                CefRefPtr<CefSharpApp> app(new CefSharpApp(cefSettings));

                int exitCode = CefExecuteProcess(main_args, app.get());

                if (exitCode >= 0)
                {
                    // Something went "wrong", but it may also be caused in the case where we are the secondary process, so we
                    // can't really throw exceptions or anything like that.
                    return false;
                }

                success = CefInitialize(main_args, *(cefSettings->_cefSettings), app.get());
                app->CompleteSchemeRegistrations();
                _initialized = success;

                if (_initialized)
                {
                    AppDomain::CurrentDomain->ProcessExit += gcnew EventHandler(ParentProcessExitHandler);
                }
            }

            return success;
        }

        /// <summary>Bind a C# class to a javascript object.</summary>
        ///<param name="name">Name of javascript object.</param>
        ///<param name="objectToBind">C# object to bind.</param>
        /// <return> Returns True</return>
        static bool RegisterJsObject(String^ name, Object^ objectToBind)
        {
            _boundObjects[name] = objectToBind;
            return true;
        }

        /// <summary> Vist all cookies. The returned cookies are ordered by longest path, then by earliest creation date.</summary>
        ///<param name="visitor">CefSharp Cookie Visitor</param>
        /// <return> Returns false if cookies cannot be accessed, otherwise true.</return>
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

        /// <summary> Vist a subset of cookies.  The results are filtered by the given url scheme, host, domain and path. 
        /// If |includeHttpOnly| is true HTTP-only cookies will also be included in the results. The returned cookies 
        /// are ordered by longest path, then by earliest creation date.</summary>
        ///<param name="url">Cookie Url</param>
        ///<param name="includeHttpOnly">Allow HTTP-only cookies to be shown in results</param>
        ///<param name="visitor">CefSharp Cookie Visitor</param>
        /// <return> Returns false if cookies cannot be accessed, otherwise true.</return>
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

        /// <summary>Set a CefSharp Cookie. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        ///<param name="url">Cookie Url</param>
        ///<param name="name">Cookie Name</param>
        ///<param name="value">Cookie Value</param>
        ///<param name="domain">Cookie Domain</param>
        ///<param name="path">Cookie Path</param>
        ///<param name="secure">Marks cookie as secure (i.e. its scope is limited to secure channels, typically HTTPS).</param>
        ///<param name="httponly">Marks the cookiet as HTTP Only(i.e. the cookie is inaccessible to client-side scripts).</param>
        ///<param name="has_expires">The cookie expiration date is only used if this is true.</param>
        ///<param name="expires">Date of cookie expiration. Only used if has_expires is true.</param>
        /// <return> Returns false if cookie cannot be set (Like if illegal charecters such as ';' are used), otherwise true.</return>
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

        /// <summary>Set a CefSharp Cookie using mostly default parameters. This function expects each attribute to be well-formed. It will check for disallowed
        /// characters (e.g. the ';' character is disallowed within the cookie value attribute) and will return false without setting
        /// the cookie if such characters are found.</summary>
        ///<param name="url">Cookie Url</param>
        ///<param name="name">Cookie Name</param>
        ///<param name="value">Cookie Value</param>
        ///<param name="domain">Cookie Domain</param>
        ///<param name="expires">Date of cookie expiration. Only used if has_expires is true.</param>
        /// <return> Returns false if cookie cannot be set (Like if illegal charecters such as ';' are used), otherwise true.</return>
        static bool SetCookie(String^ url, String^ domain, String^ name, String^ value, DateTime expires)
        {
            return SetCookie(url, name, value, domain, "/", false, false, true, expires);
        }

        /// <summary> Delete all cookies that match the both following parameters. If both |url| and |name| are empty all cookies will be deleted.</summary>
        ///<param name="url">(Optional) Cookie Url</param>
        ///<param name="name">(Optional) Name of Cookie/param>
        /// <return> Returns false if a non-empty invalid URL is specified or if cookies cannot be accessed, otherwise true.</return>
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

        /// <summary> Sets the directory path that will be used for storing cookie data. If |path| is empty data will be stored in 
        /// memory only. Otherwise, data will be stored at the specified |path|. To persist session cookies (cookies without an expiry 
        /// date or validity interval) set |persist_session_cookies| to true. Session cookies are generally intended to be transient and 
        /// most Web browsers do not persist them. Returns false if cookies cannot be accessed.</summary>
        ///<param name="path"> (Optional) File path to write cookies to.</param>
        ///<param name="persistSessionCookies">(Optional) Persist Session Cookies.</param>
        /// <return> Returns false if a non-empty invalid URL is specified or if cookies cannot be accessed, otherwise true.</return>
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

        /// <summary> Shuts down CefSharp and the underlying CEF infrastructure. 
        /// This method is safe to call multiple times; it will only
        /// shut down CEF on the first calls (all following calls will be ignored).
        /// </summary>
        static void Shutdown()
        {
            if (IsInitialized)
            {
                CefShutdown();
                IsInitialized = false;
            }
        }
    };
}