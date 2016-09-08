// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <msclr/lock.h>
#include <msclr/marshal.h>
#include <include/cef_version.h>
#include <include/cef_origin_whitelist.h>
#include <include/cef_web_plugin.h>

#include "Internals/CefSharpApp.h"
#include "Internals/CookieManager.h"
#include "Internals/PluginVisitor.h"
#include "CefSettings.h"
#include "RequestContext.h"
#include "SchemeHandlerFactoryWrapper.h"
#include "Internals/CefTaskScheduler.h"
#include "Internals/CefGetGeolocationCallbackWrapper.h"

using namespace System::Collections::Generic; 
using namespace System::Linq;
using namespace System::Reflection;
using namespace msclr::interop;

namespace CefSharp
{
    public ref class Cef sealed
    {
    private:
        static Object^ _sync;

        static bool _initialized = false;
        static HashSet<IDisposable^>^ _disposables;

        static Cef()
        {
            _sync = gcnew Object();
            _disposables = gcnew HashSet<IDisposable^>();
        }

    public:

        static property TaskFactory^ UIThreadTaskFactory;
        static property TaskFactory^ IOThreadTaskFactory;
        static property TaskFactory^ FileThreadTaskFactory;

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
                return String::Format("r{0}", CEF_VERSION);
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

        /// <summary>
        /// Gets a value that indicates the Git Hash for CEF version currently being used.
        /// </summary>
        /// <value>The Git Commit Hash</value>
        static property String^ CefCommitHash
        {
            String^ get()
            {
                return CEF_COMMIT_HASH;
            }
        }

        /// <summary>
        /// Initializes CefSharp with the default settings.
        /// This function should be called on the main application thread to initialize the CEF browser process.
        /// </summary>
        /// <return>true if successful; otherwise, false.</return>
        static bool Initialize()
        {
            auto cefSettings = gcnew CefSettings();
            return Initialize(cefSettings);
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// This function should be called on the main application thread to initialize the CEF browser process.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <return>true if successful; otherwise, false.</return>
        static bool Initialize(CefSettings^ cefSettings)
        {
            return Initialize(cefSettings, false, nullptr);
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// This function should be called on the main application thread to initialize the CEF browser process.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
        /// <return>true if successful; otherwise, false.</return>
        static bool Initialize(CefSettings^ cefSettings, bool performDependencyCheck, IBrowserProcessHandler^ browserProcessHandler)
        {
            if (IsInitialized)
            {
                // NOTE: Can only initialize Cef once, to make this explicitly clear throw exception on subsiquent attempts
                throw gcnew Exception("Cef can only be initialized once. Use Cef.IsInitialized to guard against this exception.");
            }
            
            if (cefSettings->BrowserSubprocessPath == nullptr)
            {
                throw gcnew Exception("CefSettings BrowserSubprocessPath cannot be null.");
            }

            if(performDependencyCheck)
            {
                DependencyChecker::AssertAllDependenciesPresent(cefSettings->Locale, cefSettings->LocalesDirPath, cefSettings->ResourcesDirPath, cefSettings->PackLoadingDisabled, cefSettings->BrowserSubprocessPath);
            }

            UIThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_UI));
            IOThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_IO));
            FileThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_FILE));

            CefMainArgs main_args;
            CefRefPtr<CefSharpApp> app(new CefSharpApp(cefSettings, browserProcessHandler));

            auto success = CefInitialize(main_args, *(cefSettings->_cefSettings), app.get(), NULL);

            //Register SchemeHandlerFactories - must be called after CefInitialize
            for each (CefCustomScheme^ cefCustomScheme in cefSettings->CefCustomSchemes)
            {
                auto domainName = cefCustomScheme->DomainName ? cefCustomScheme->DomainName : String::Empty;

                CefRefPtr<CefSchemeHandlerFactory> wrapper = new SchemeHandlerFactoryWrapper(cefCustomScheme->SchemeHandlerFactory);
                CefRegisterSchemeHandlerFactory(StringUtils::ToNative(cefCustomScheme->SchemeName), StringUtils::ToNative(domainName), wrapper);
            }

            _initialized = success;

            return success;
        }

        /// <summary>
        /// Run the CEF message loop. Use this function instead of an application-
        /// provided message loop to get the best balance between performance and CPU
        /// usage. This function should only be called on the main application thread and
        /// only if Cef.Initialize() is called with a
        /// CefSettings.MultiThreadedMessageLoop value of false. This function will
        /// block until a quit message is received by the system.
        /// </summary>
        static void RunMessageLoop()
        {
            CefRunMessageLoop();
        }

        /// <summary>
        /// Quit the CEF message loop that was started by calling Cef.RunMessageLoop().
        /// This function should only be called on the main application thread and only
        /// if Cef.RunMessageLoop() was used.
        /// </summary>
        static void QuitMessageLoop()
        {
            CefQuitMessageLoop();
        }

        /// <summary>
        /// Perform a single iteration of CEF message loop processing.This function is
        /// provided for cases where the CEF message loop must be integrated into an
        /// existing application message loop. Use of this function is not recommended
        /// for most users; use CefSettings.MultiThreadedMessageLoop if possible (the deault).
        /// When using this function care must be taken to balance performance
        /// against excessive CPU usage. It is recommended to enable the
        /// CefSettings.ExternalMessagePump option when using
        /// this function so that IBrowserProcessHandler.OnScheduleMessagePumpWork()
        /// callbacks can facilitate the scheduling process. This function should only be
        /// called on the main application thread and only if Cef.Initialize() is called
        /// with a CefSettings.MultiThreadedMessageLoop value of false. This function
        /// will not block.
        /// </summary>
        static void DoMessageLoopWork()
        {
            CefDoMessageLoopWork();
        }


        /// <summary>
        /// This function should be called from the application entry point function to execute a secondary process.
        /// It can be used to run secondary processes from the browser client executable (default behavior) or
        /// from a separate executable specified by the CefSettings.browser_subprocess_path value.
        /// If called for the browser process (identified by no "type" command-line value) it will return immediately with a value of -1.
        /// If called for a recognized secondary process it will block until the process should exit and then return the process exit code.
        /// The |application| parameter may be empty. The |windows_sandbox_info| parameter is only used on Windows and may be NULL (see cef_sandbox_win.h for details). 
        /// </summary>
        static int ExecuteProcess()
        {
            auto hInstance = Process::GetCurrentProcess()->Handle;

            CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());
            //TODO: Look at ways to expose an instance of CefApp
            //CefRefPtr<CefSharpApp> app(new CefSharpApp(nullptr, nullptr));

            return CefExecuteProcess(cefMainArgs, NULL, NULL);
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

        /// <summary>
        /// Returns the global cookie manager.
        /// </summary>
        static ICookieManager^ GetGlobalCookieManager()
        {
            auto cookieManager = CefCookieManager::GetGlobalManager(NULL);
            if (cookieManager.get())
            {
                return gcnew CookieManager(cookieManager);
            }
            return nullptr;
        }

        /// <summary>
        /// Shuts down CefSharp and the underlying CEF infrastructure. This method is safe to call multiple times; it will only
        /// shut down CEF on the first call (all subsequent calls will be ignored).
        /// This function should be called on the main application thread to shut down the CEF browser process before the application exits. 
        /// </summary>
        static void Shutdown()
        {
            if (IsInitialized)
            { 
                msclr::lock l(_sync);

                UIThreadTaskFactory = nullptr;
                IOThreadTaskFactory = nullptr;
                FileThreadTaskFactory = nullptr;

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
        /// Async returns a list containing Plugin Information
        /// (Wrapper around CefVisitWebPluginInfo)
        /// </summary>
        /// <return>Returns List of <see cref="Plugin"/> structs.</return>
        static Task<List<Plugin>^>^ GetPlugins()
        {
            CefRefPtr<PluginVisitor> visitor = new PluginVisitor();
            
            CefVisitWebPluginInfo(visitor);

            return visitor->GetTask();
        }

        /// <summary>
        /// Cause the plugin list to refresh the next time it is accessed regardless of whether it has already been loaded.
        /// </summary>
        static void RefreshWebPlugins()
        {
            CefRefreshWebPlugins();
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
        /// Call during process startup to enable High-DPI support on Windows 7 or newer.
        /// Older versions of Windows should be left DPI-unaware because they do not
        /// support DirectWrite and GDI fonts are kerned very badly.
        /// </summary>
        static void EnableHighDPISupport()
        {
            CefEnableHighDPISupport();
        }

        /// <summary>
        /// Request a one-time geolocation update.
        /// This function bypasses any user permission checks so should only be
        /// used by code that is allowed to access location information. 
        /// </summary>
        /// <return>Returns 'best available' location info or, if the location update failed, with error info.</return>
        static Task<Geoposition^>^ GetGeolocationAsync()
        {
            auto callback = new CefGetGeolocationCallbackWrapper();
            
            CefGetGeolocation(callback);

            return callback->GetTask();
        }

        /// <summary>
        /// Returns true if called on the specified CEF thread.
        /// </summary>
        /// <return>Returns true if called on the specified thread.</return>
        static bool CurrentlyOnThread(CefThreadIds threadId)
        {
            return CefCurrentlyOn((CefThreadId)threadId);
        }

        /// <summary>
        /// Gets the Global Request Context. Make sure to Dispose of this object when finished.
        /// </summary>
        /// <return>Returns the global request context or null.</return>
        static IRequestContext^ GetGlobalRequestContext()
        {
            auto context = CefRequestContext::GetGlobalContext();

            if (context.get())
            {
                return gcnew RequestContext(context);
            }

            return nullptr;
        }
    };
}
