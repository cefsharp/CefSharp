// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <msclr/lock.h>
#include <include/cef_version.h>
#include <include/cef_runnable.h>
#include <include/cef_origin_whitelist.h>
#include <include/cef_web_plugin.h>

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
        /// <summary>
        /// Called on the browser process UI thread immediately after the CEF context has been initialized. 
        /// </summary>
        static property Action^ OnContextInitialized;

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
            return Initialize(cefSettings, true, false);
        }

        /// <summary>Initializes CefSharp with user-provided settings.</summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <param name="shutdownOnProcessExit">When the Current AppDomain (relative to the thread called on)
        /// Exits(ProcessExit event) then Shudown will be called.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
        /// <return>true if successful; otherwise, false.</return>
        static bool Initialize(CefSettings^ cefSettings, bool shutdownOnProcessExit, bool performDependencyCheck)
        {
            bool success = false;

            // NOTE: Can only initialize Cef once, so subsiquent calls are ignored.
            if (!IsInitialized)
            {
                if (cefSettings->BrowserSubprocessPath == nullptr)
                {
                    throw gcnew Exception("CefSettings BrowserSubprocessPath cannot be null.");
                }

                if(performDependencyCheck)
                {
                    DependencyChecker::AssertAllDependenciesPresent(cefSettings->Locale, cefSettings->LocalesDirPath, cefSettings->ResourcesDirPath, cefSettings->PackLoadingDisabled, cefSettings->BrowserSubprocessPath);
                }

                CefMainArgs main_args;
                CefRefPtr<CefSharpApp> app(new CefSharpApp(cefSettings, OnContextInitialized));

                success = CefInitialize(main_args, *(cefSettings->_cefSettings), app.get(), NULL);
                app->CompleteSchemeRegistrations();
                _initialized = success;

                if (_initialized && shutdownOnProcessExit)
                {
                    AppDomain::CurrentDomain->ProcessExit += gcnew EventHandler(ParentProcessExitHandler);
                }
            }

            return success;
        }

        /// <summary>Add an entry to the cross-origin whitelist.</summary>
        /// <param name="sourceOrigin">The origin allowed to be accessed by the target protocol/domain.</param>
        /// <param name="targetProtocol">The target protocol allowed to access the source origin.</param>
        /// <param name="targetDomain">The optional target domain allowed to access the source origin.</param>
        /// <param name="allowTargetSubdomains">If set to true would allow a blah.example.com if the 
        ///     <paramref name="targetDomain"/> was set to example.com
        /// </param>
        /// <remarks>
        /// The same-origin policy restricts how scripts hosted from different origins
        /// (scheme + domain + port) can communicate. By default, scripts can only access
        /// resources with the same origin. Scripts hosted on the HTTP and HTTPS schemes
        /// (but no other schemes) can use the "Access-Control-Allow-Origin" header to
        /// allow cross-origin requests. For example, https://source.example.com can make
        /// XMLHttpRequest requests on http://target.example.com if the
        /// http://target.example.com request returns an "Access-Control-Allow-Origin:
        /// https://source.example.com" response header.
        //
        /// Scripts in separate frames or iframes and hosted from the same protocol and
        /// domain suffix can execute cross-origin JavaScript if both pages set the
        /// document.domain value to the same domain suffix. For example,
        /// scheme://foo.example.com and scheme://bar.example.com can communicate using
        /// JavaScript if both domains set document.domain="example.com".
        //
        /// This method is used to allow access to origins that would otherwise violate
        /// the same-origin policy. Scripts hosted underneath the fully qualified
        /// <paramref name="sourceOrigin"/> URL (like http://www.example.com) will be allowed access to
        /// all resources hosted on the specified <paramref name="targetProtocol"/> and <paramref name="targetDomain"/>.
        /// If <paramref name="targetDomain"/> is non-empty and <paramref name="allowTargetSubdomains"/> if false only
        /// exact domain matches will be allowed. If <paramref name="targetDomain"/> contains a top-
        /// level domain component (like "example.com") and <paramref name="allowTargetSubdomains"/> is
        /// true sub-domain matches will be allowed. If <paramref name="targetDomain"/> is empty and
        /// <paramref name="allowTargetSubdomains"/> if true all domains and IP addresses will be
        /// allowed.
        //
        /// This method cannot be used to bypass the restrictions on local or display
        /// isolated schemes. See the comments on <see cref="CefCustomScheme"/> for more
        /// information.
        ///
        /// This function may be called on any thread. Returns false if <paramref name="sourceOrigin"/>
        /// is invalid or the whitelist cannot be accessed.
        /// </remarks>
        static bool AddCrossOriginWhitelistEntry(
            String^ sourceOrigin,
            String^ targetProtocol,
            String^ targetDomain,
            bool allowTargetSubdomains)
        {
            return CefAddCrossOriginWhitelistEntry(
                    StringUtils::ToNative(sourceOrigin),
                    StringUtils::ToNative(targetProtocol),
                    StringUtils::ToNative(targetDomain),
                    allowTargetSubdomains);
        }

        /// <summary>Remove entry from cross-origin whitelist</summary>
        /// <param name="sourceOrigin">The origin allowed to be accessed by the target protocol/domain.</param>
        /// <param name="targetProtocol">The target protocol allowed to access the source origin.</param>
        /// <param name="targetDomain">The optional target domain allowed to access the source origin.</param>
        /// <param name="allowTargetSubdomains">If set to true would allow a blah.example.com if the 
        ///     <paramref name="targetDomain"/> was set to example.com
        /// </param>
        /// <remarks>
        /// Remove an entry from the cross-origin access whitelist. Returns false if
        /// <paramref name="sourceOrigin"/> is invalid or the whitelist cannot be accessed.
        /// </remarks>
        static bool RemoveCrossOriginWhitelistEntry(String^ sourceOrigin,
            String^ targetProtocol,
            String^ targetDomain,
            bool allowTargetSubdomains)

        {
            return CefRemoveCrossOriginWhitelistEntry(
                StringUtils::ToNative(sourceOrigin),
                StringUtils::ToNative(targetProtocol),
                StringUtils::ToNative(targetDomain),
                allowTargetSubdomains);
        }

        /// <summary>Remove all entries from the cross-origin access whitelist.</summary>
        /// <remarks>
        /// Remove all entries from the cross-origin access whitelist. Returns false if
        /// the whitelist cannot be accessed.
        /// </remarks>
        static bool ClearCrossOriginWhitelist()
        {
            return CefClearCrossOriginWhitelist();
        }

        /// <summary>Visits all cookies using the provided Cookie Visitor. The returned cookies are sorted by longest path, then by earliest creation date.</summary>
        /// <param name="visitor">A user-provided Cookie Visitor implementation.</param>
        /// <return>Returns false if the CookieManager is not available; otherwise, true.</return>
        static bool VisitAllCookies(ICookieVisitor^ visitor)
        {
            CefRefPtr<CefCookieManager> manager = CefCookieManager::GetGlobalManager();

            if (manager == nullptr)
            {
                return false;
            }

            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);
            
            return manager->VisitAllCookies(cookieVisitor);
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
            CefRefPtr<CefCookieManager> manager = CefCookieManager::GetGlobalManager();

            if (manager == nullptr)
            {
                return false;
            }

            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);

            return manager->VisitUrlCookies(StringUtils::ToNative(url), includeHttpOnly, cookieVisitor);
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
        /// <param name="domain">The cookie domain.</param>
        /// <param name="name">The cookie name.</param>
        /// <param name="value">The cookie value.</param>
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

            if (manager == nullptr)
            {
                return false;
            }

            return manager->SetStoragePath(StringUtils::ToNative(path), persistSessionCookies);
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

            CefRefPtr<CefCompletionCallback> wrapper = new CompletionHandler(handler);

            return manager->FlushStore(wrapper);
        }

        /// <summary>Shuts down CefSharp and the underlying CEF infrastructure. This method is safe to call multiple times; it will only
        /// shut down CEF on the first call (all subsequent calls will be ignored).
        /// </summary>
        static void Shutdown()
        {
            if (IsInitialized)
            { 
                OnContextInitialized = nullptr;
                
                msclr::lock l(_sync);
                for each(IDisposable^ diposable in Enumerable::ToList(_disposables))
                {
                    delete diposable;
                }
                
                GC::Collect();
                GC::WaitForPendingFinalizers();

                CefShutdown();
                IsInitialized = false;
            }
        }

        /// <summary>
        /// Clear all registered scheme handler factories.
        /// </summary>
        /// <return>Returns false on error.</return>
        static bool ClearSchemeHandlerFactories()
        {
            return CefClearSchemeHandlerFactories();
        }

        /// <summary>
        /// Add a plugin path (directory + file). This change may not take affect until after RefreshWebPlugins() is called.
        /// </summary>
        /// <param name="path">Path (directory + file).</param>
        static void AddWebPluginPath(String^ path)
        {
            CefAddWebPluginPath(StringUtils::ToNative(path));
        }

        /// <summary>
        /// Add a plugin directory. This change may not take affect until after CefRefreshWebPlugins() is called.
        /// </summary>
        /// <param name="directory">Directory.</param>
        static void AddWebPluginDirectory(String^ directory)
        {
            CefAddWebPluginDirectory(StringUtils::ToNative(directory));
        }

        /// <summary>
        /// Cause the plugin list to refresh the next time it is accessed regardless of whether it has already been loaded.
        /// </summary>
        static void RefreshWebPlugins()
        {
            CefRefreshWebPlugins();
        }

        /// <summary>
        /// Remove a plugin path (directory + file). This change may not take affect until after RefreshWebPlugins() is called. 
        /// </summary>
        /// <param name="path">Path (directory + file).</param>
        static void RemoveWebPluginPath(String^ path)
        {
            CefRemoveWebPluginPath(StringUtils::ToNative(path));
        }

        /// <summary>
        /// Unregister an internal plugin. This may be undone the next time RefreshWebPlugins() is called. 
        /// </summary>
        /// <param name="path">Path (directory + file).</param>
        static void UnregisterInternalWebPlugin(String^ path)
        {
            CefUnregisterInternalWebPlugin(StringUtils::ToNative(path));
        }	

        /// <summary>
        /// Force a plugin to shutdown. 
        /// </summary>
        /// <param name="path">Path (directory + file).</param>
        static void ForceWebPluginShutdown(String^ path)
        {
            CefForceWebPluginShutdown(StringUtils::ToNative(path));
        }
    };
}
