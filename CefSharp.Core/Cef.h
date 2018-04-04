// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <msclr/lock.h>
#include <msclr/marshal.h>
#include <include/cef_version.h>
#include <include/cef_origin_whitelist.h>
#include <include/cef_web_plugin.h>
#include <include/cef_crash_util.h>

#include "Internals/CefSharpApp.h"
#include "Internals/PluginVisitor.h"
#include "Internals/CefTaskScheduler.h"
#include "Internals/CefRegisterCdmCallbackAdapter.h"
#include "CookieManager.h"
#include "CefSettings.h"
#include "RequestContext.h"
#include "SchemeHandlerFactoryWrapper.h"

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
        static int _initializedThreadId;
        static bool _multiThreadedMessageLoop = true;

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
        /// This function can only be called once, subsiquent calls will result in an Exception.
        /// It's important to note that Initialize and Shutdown <strong>MUST</strong> be called on your main
        /// applicaiton thread (Typically the UI thead). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <returns>true if successful; otherwise, false.</returns>
        static bool Initialize()
        {
            auto cefSettings = gcnew CefSettings();
            return Initialize(cefSettings);
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize and Shutdown <strong>MUST</strong> be called on your main
        /// applicaiton thread (Typically the UI thead). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <returns>true if successful; otherwise, false.</returns>
        static bool Initialize(CefSettings^ cefSettings)
        {
            return Initialize(cefSettings, false, nullptr);
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize/Shutdown <strong>MUST</strong> be called on your main
        /// applicaiton thread (Typically the UI thead). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
        /// <returns>true if successful; otherwise, false.</returns>
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

            if (CefSharpSettings::Proxy != nullptr && !cefSettings->CommandLineArgsDisabled)
            {
                cefSettings->CefCommandLineArgs->Add("proxy-server", CefSharpSettings::Proxy->IP + ":" + CefSharpSettings::Proxy->Port);

                if (!String::IsNullOrEmpty(CefSharpSettings::Proxy->BypassList))
                {
                    cefSettings->CefCommandLineArgs->Add("proxy-bypass-list", CefSharpSettings::Proxy->BypassList);
                }
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
            _multiThreadedMessageLoop = cefSettings->MultiThreadedMessageLoop;

            _initializedThreadId = Thread::CurrentThread->ManagedThreadId;

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
        ///  Returns a cookie manager that neither stores nor retrieves cookies. All
        /// usage of cookies will be blocked including cookies accessed via the network
        /// (request/response headers), via JavaScript (document.cookie), and via
        /// CefCookieManager methods. No cookies will be displayed in DevTools. If you
        /// wish to only block cookies sent via the network use the IRequestHandler
        /// CanGetCookies and CanSetCookie methods instead.
        /// </summary>
        static ICookieManager^ GetBlockingCookieManager()
        {
            auto cookieManager = CefCookieManager::GetBlockingManager();
            if (cookieManager.get())
            {
                return gcnew CookieManager(cookieManager);
            }
            return nullptr;
        }

        /// <summary>
        /// Shuts down CefSharp and the underlying CEF infrastructure. This method is safe to call multiple times; it will only
        /// shut down CEF on the first call (all subsequent calls will be ignored).
        /// This method should be called on the main application thread to shut down the CEF browser process before the application exits. 
        /// If you are Using CefSharp.OffScreen then you must call this explicitly before your application exits or it will hang.
        /// This method must be called on the same thread as Initialize. If you don't call Shutdown explicitly then CefSharp.Wpf and CefSharp.WinForms
        /// versions will do their best to call Shutdown for you, if your application is having trouble closing then call thus explicitly.
        /// </summary>
        static void Shutdown()
        {
            if (IsInitialized)
            {
                msclr::lock l(_sync);

                if (IsInitialized)
                {
                    if (_initializedThreadId != Thread::CurrentThread->ManagedThreadId)
                    {
                        throw gcnew Exception("Shutdown must be called on the same thread that Initialize was called - typically your UI thread. CefSharp was initialized on ManagedThreadId: " + Thread::CurrentThread->ManagedThreadId);
                    }

                    UIThreadTaskFactory = nullptr;
                    IOThreadTaskFactory = nullptr;
                    FileThreadTaskFactory = nullptr;

                    for each(IDisposable^ diposable in Enumerable::ToList(_disposables))
                    {
                        delete diposable;
                    }
                
                    GC::Collect();
                    GC::WaitForPendingFinalizers();

                    if (!_multiThreadedMessageLoop)
                    {
                        // We need to run the message pump until it is idle. However we don't have
                        // that information here so we run the message loop "for a while".
                        // See https://github.com/cztomczak/cefpython/issues/245 for an excellent description
                        for (int i = 0; i < 10; i++)
                        {
                            DoMessageLoopWork();

                            // Sleep to allow the CEF proc to do work.
                            Sleep(50);
                        }
                    }

                    CefShutdown();
                    IsInitialized = false;
                }
            }
        }

        /// <summary>
        /// This method should only be used by advanced users, if you're unsure then use Cef.Shutdown().
        /// This function should be called on the main application thread to shut down
        /// the CEF browser process before the application exits. This method simply obtains a lock
        /// and calls the native CefShutdown method, only IsInitialized is checked. All ChromiumWebBrowser
        /// instances MUST be Disposed of before calling this method. If calling this method results in a crash
        /// or hangs then you're likely hanging on to some unmanaged resources or haven't closed all of your browser
        /// instances
        /// </summary>
        static void ShutdownWithoutChecks()
        {
            if (IsInitialized)
            {
                msclr::lock l(_sync);

                if (IsInitialized)
                {
                    CefShutdown();
                    IsInitialized = false;
                }
            }
        }

        /// <summary>
        /// Clear all registered scheme handler factories.
        /// </summary>
        /// <returns>Returns false on error.</returns>
        static bool ClearSchemeHandlerFactories()
        {
            return CefClearSchemeHandlerFactories();
        }

        /// <summary>
        /// Visit web plugin information. Can be called on any thread in the browser process.
        /// </summary>
        static void VisitWebPluginInfo(IWebPluginInfoVisitor^ visitor)
        {
            CefVisitWebPluginInfo(new PluginVisitor(visitor));
        }

        /// <summary>
        /// Async returns a list containing Plugin Information
        /// (Wrapper around CefVisitWebPluginInfo)
        /// </summary>
        /// <returns>Returns List of <see cref="Plugin"/> structs.</returns>
        static Task<List<WebPluginInfo^>^>^ GetPlugins()
        {
            auto taskVisitor = gcnew TaskWebPluginInfoVisitor();
            CefRefPtr<PluginVisitor> visitor = new PluginVisitor(taskVisitor);
            
            CefVisitWebPluginInfo(visitor);

            return taskVisitor->Task;
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
        /// Returns true if called on the specified CEF thread.
        /// </summary>
        /// <returns>Returns true if called on the specified thread.</returns>
        static bool CurrentlyOnThread(CefThreadIds threadId)
        {
            return CefCurrentlyOn((CefThreadId)threadId);
        }

        /// <summary>
        /// Gets the Global Request Context. Make sure to Dispose of this object when finished.
        /// </summary>
        /// <returns>Returns the global request context or null.</returns>
        static IRequestContext^ GetGlobalRequestContext()
        {
            auto context = CefRequestContext::GetGlobalContext();

            if (context.get())
            {
                return gcnew RequestContext(context);
            }

            return nullptr;
        }

        /// <summary>
        /// Crash reporting is configured using an INI-style config file named
        /// crash_reporter.cfg. This file must be placed next to
        /// the main application executable. File contents are as follows:
        ///
        ///  # Comments start with a hash character and must be on their own line.
        ///
        ///  [Config]
        ///  ProductName=<Value of the "prod" crash key; defaults to "cef">
        ///  ProductVersion=<Value of the "ver" crash key; defaults to the CEF version>
        ///  AppName=<Windows only; App-specific folder name component for storing crash
        ///           information; default to "CEF">
        ///  ExternalHandler=<Windows only; Name of the external handler exe to use
        ///                   instead of re-launching the main exe; default to empty>
        ///  ServerURL=<crash server URL; default to empty>
        ///  RateLimitEnabled=<True if uploads should be rate limited; default to true>
        ///  MaxUploadsPerDay=<Max uploads per 24 hours, used if rate limit is enabled;
        ///                    default to 5>
        ///  MaxDatabaseSizeInMb=<Total crash report disk usage greater than this value
        ///                       will cause older reports to be deleted; default to 20>
        ///  MaxDatabaseAgeInDays=<Crash reports older than this value will be deleted;
        ///                        default to 5>
        ///
        ///  [CrashKeys]
        ///  my_key1=<small|medium|large>
        ///  my_key2=<small|medium|large>
        ///
        /// Config section:
        ///
        /// If "ProductName" and/or "ProductVersion" are set then the specified values
        /// will be included in the crash dump metadata. 
        ///
        /// If "AppName" is set on Windows then crash report information (metrics,
        /// database and dumps) will be stored locally on disk under the
        /// "C:\Users\[CurrentUser]\AppData\Local\[AppName]\User Data" folder. On other
        /// platforms the CefSettings.user_data_path value will be used.
        ///
        /// If "ExternalHandler" is set on Windows then the specified exe will be
        /// launched as the crashpad-handler instead of re-launching the main process
        /// exe. The value can be an absolute path or a path relative to the main exe
        /// directory. 
        ///
        /// If "ServerURL" is set then crashes will be uploaded as a multi-part POST
        /// request to the specified URL. Otherwise, reports will only be stored locally
        /// on disk.
        ///
        /// If "RateLimitEnabled" is set to true then crash report uploads will be rate
        /// limited as follows:
        ///  1. If "MaxUploadsPerDay" is set to a positive value then at most the
        ///     specified number of crashes will be uploaded in each 24 hour period.
        ///  2. If crash upload fails due to a network or server error then an
        ///     incremental backoff delay up to a maximum of 24 hours will be applied for
        ///     retries.
        ///  3. If a backoff delay is applied and "MaxUploadsPerDay" is > 1 then the
        ///     "MaxUploadsPerDay" value will be reduced to 1 until the client is
        ///     restarted. This helps to avoid an upload flood when the network or
        ///     server error is resolved.
        ///
        /// If "MaxDatabaseSizeInMb" is set to a positive value then crash report storage
        /// on disk will be limited to that size in megabytes. For example, on Windows
        /// each dump is about 600KB so a "MaxDatabaseSizeInMb" value of 20 equates to
        /// about 34 crash reports stored on disk.
        ///
        /// If "MaxDatabaseAgeInDays" is set to a positive value then crash reports older
        /// than the specified age in days will be deleted.
        ///
        /// CrashKeys section:
        ///
        /// Any number of crash keys can be specified for use by the application. Crash
        /// key values will be truncated based on the specified size (small = 63 bytes,
        /// medium = 252 bytes, large = 1008 bytes). The value of crash keys can be set
        /// from any thread or process using the Cef.SetCrashKeyValue function. These
        /// key/value pairs will be sent to the crash server along with the crash dump
        /// file. Medium and large values will be chunked for submission. For example,
        /// if your key is named "mykey" then the value will be broken into ordered
        /// chunks and submitted using keys named "mykey-1", "mykey-2", etc.
        /// </summary>
        /// <returns>Returns the global request context or null.</returns>
        static property bool CrashReportingEnabled
        {
            bool get()
            {
                return CefCrashReportingEnabled();
            }
        }

        /// <summary>
        /// Sets or clears a specific key-value pair from the crash metadata.
        /// </summary>
        static void SetCrashKeyValue(String^ key, String^ value)
        {
            CefSetCrashKeyValue(StringUtils::ToNative(key), StringUtils::ToNative(value));
        }

        /// <summary>
        /// Register the Widevine CDM plugin.
        /// 
        /// The client application is responsible for downloading an appropriate
        /// platform-specific CDM binary distribution from Google, extracting the
        /// contents, and building the required directory structure on the local machine.
        /// The <see cref="IBrowserHost.StartDownload"/> method class can be used
        /// to implement this functionality in CefSharp. Contact Google via
        /// https://www.widevine.com/contact.html for details on CDM download.
        /// 
        /// 
        /// path is a directory that must contain the following files:
        ///   1. manifest.json file from the CDM binary distribution (see below).
        ///   2. widevinecdm file from the CDM binary distribution (e.g.
        ///      widevinecdm.dll on Windows).
        ///   3. widevidecdmadapter file from the CEF binary distribution (e.g.
        ///      widevinecdmadapter.dll on Windows).
        ///
        /// If any of these files are missing or if the manifest file has incorrect
        /// contents the registration will fail and callback will receive an ErrorCode
        /// value of <see cref="CdmRegistrationErrorCode.IncorrectContents"/>.
        ///
        /// The manifest.json file must contain the following keys:
        ///   A. "os": Supported OS (e.g. "mac", "win" or "linux").
        ///   B. "arch": Supported architecture (e.g. "ia32" or "x64").
        ///   C. "x-cdm-module-versions": Module API version (e.g. "4").
        ///   D. "x-cdm-interface-versions": Interface API version (e.g. "8").
        ///   E. "x-cdm-host-versions": Host API version (e.g. "8").
        ///   F. "version": CDM version (e.g. "1.4.8.903").
        ///   G. "x-cdm-codecs": List of supported codecs (e.g. "vp8,vp9.0,avc1").
        ///
        /// A through E are used to verify compatibility with the current Chromium
        /// version. If the CDM is not compatible the registration will fail and
        /// callback will receive an ErrorCode value of <see cref="CdmRegistrationErrorCode.Incompatible"/>.
        ///
        /// If registration is not supported at the time that Cef.RegisterWidevineCdm() is called then callback
        /// will receive an ErrorCode value of <see cref="CdmRegistrationErrorCode.NotSupported"/>.
        /// </summary>
        /// <param name="path"> is a directory that contains the Widevine CDM files</param>
        /// <param name="callback">optional callback - <see cref="IRegisterCdmCallback.OnRegistrationCompletecallback"/> 
        /// will be executed asynchronously once registration is complete</param>
        static void RegisterWidevineCdm(String^ path, [Optional] IRegisterCdmCallback^ callback)
        {
            CefRefPtr<CefRegisterCdmCallbackAdapter> adapter = NULL;

            if (callback != nullptr)
            {
                adapter = new CefRegisterCdmCallbackAdapter(callback);
            }

            CefRegisterWidevineCdm(StringUtils::ToNative(path), adapter);
        }

        /// <summary>
        /// Register the Widevine CDM plugin.
        ///
        /// See <see cref="RegisterWidevineCdm(String, IRegisterCdmCallback)"/> for more details.
        /// </summary>
        /// <param name="path"> is a directory that contains the Widevine CDM files</param>
        /// <returns>Returns a Task that can be awaited to receive the <see cref="CdmRegistration"/> response.</returns>
        static Task<CdmRegistration^>^ RegisterWidevineCdmAsync(String^ path)
        {
            auto callback = gcnew TaskRegisterCdmCallback();
            
            RegisterWidevineCdm(path, callback);

            return callback->Task;
        }
    };
}
