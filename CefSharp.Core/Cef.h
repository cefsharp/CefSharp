// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEFSHARP_CORE_CEF_H_
#define CEFSHARP_CORE_CEF_H_

#pragma once

#include "Stdafx.h"

#include <msclr/lock.h>
#include <msclr/marshal.h>
#include <include/cef_version.h>
#include <include/cef_origin_whitelist.h>
#include <include/cef_web_plugin.h>
#include <include/cef_crash_util.h>
#include <include/cef_parser.h>
#include <include/internal/cef_types.h>

#include "Internals/CefSharpApp.h"
#include "Internals/CefWebPluginInfoVisitorAdapter.h"
#include "Internals/CefTaskScheduler.h"
#include "Internals/CefRegisterCdmCallbackAdapter.h"
#include "CookieManager.h"
#include "CefSettingsBase.h"
#include "RequestContext.h"

using namespace System::Collections::Generic;
using namespace System::Linq;
using namespace System::Reflection;
using namespace msclr::interop;

namespace CefSharp
{
    /// <summary>
    /// Global CEF methods are exposed through this class. e.g. CefInitalize maps to Cef.Initialize
    /// CEF API Doc https://magpcss.org/ceforum/apidocs3/projects/(default)/(_globals).html
    /// This class cannot be inherited.
    /// </summary>
    public ref class Cef sealed
    {
    private:
        static Object^ _sync;

        static bool _initialized = false;
        static HashSet<IDisposable^>^ _disposables;
        static int _initializedThreadId;
        static bool _multiThreadedMessageLoop = true;
        static bool _waitForBrowsersToCloseEnabled = false;

        static Cef()
        {
            _sync = gcnew Object();
            _disposables = gcnew HashSet<IDisposable^>();
        }

        static bool CurrentOnUiThread()
        {
            return CefCurrentlyOn(CefThreadId::TID_UI);
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
                    CHROME_VERSION_MAJOR, (Object^)CHROME_VERSION_MINOR,
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
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize and Shutdown <strong>MUST</strong> be called on your main
        /// applicaiton thread (Typically the UI thead). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <returns>true if successful; otherwise, false.</returns>
        static bool Initialize(CefSettingsBase^ cefSettings)
        {
            auto cefApp = gcnew DefaultApp(nullptr, cefSettings->CefCustomSchemes);

            return Initialize(cefSettings, false, cefApp);
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize/Shutdown <strong>MUST</strong> be called on your main
        /// applicaiton thread (Typically the UI thead). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
        /// <param name="browserProcessHandler">The handler for functionality specific to the browser process. Null if you don't wish to handle these events</param>
        /// <returns>true if successful; otherwise, false.</returns>
        static bool Initialize(CefSettingsBase^ cefSettings, bool performDependencyCheck, IBrowserProcessHandler^ browserProcessHandler)
        {
            auto cefApp = gcnew DefaultApp(browserProcessHandler, cefSettings->CefCustomSchemes);

            return Initialize(cefSettings, performDependencyCheck, cefApp);
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize/Shutdown <strong>MUST</strong> be called on your main
        /// applicaiton thread (Typically the UI thead). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="cefSettings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
        /// <param name="cefApp">Implement this interface to provide handler implementations. Null if you don't wish to handle these events</param>
        /// <returns>true if successful; otherwise, false.</returns>
        static bool Initialize(CefSettingsBase^ cefSettings, bool performDependencyCheck, IApp^ cefApp)
        {
            if (IsInitialized)
            {
                // NOTE: Can only initialize Cef once, to make this explicitly clear throw exception on subsiquent attempts
                throw gcnew Exception("CEF can only be initialized once per process. This is a limitation of the underlying " +
                    "CEF/Chromium framework. You can change many (not all) settings at runtime through RequestContext.SetPreference. " +
                    "See https://github.com/cefsharp/CefSharp/wiki/General-Usage#request-context-browser-isolation " +
                    "Use Cef.IsInitialized to guard against this exception. If you are seeing this unexpectedly then you are likely " +
                    "calling Cef.Initialize after you've created an instance of ChromiumWebBrowser, it must be before the first instance is created.");
            }

            //Empty string is acceptable, the main application executable will be used
            if (cefSettings->BrowserSubprocessPath == nullptr)
            {
                throw gcnew Exception("CefSettings BrowserSubprocessPath cannot be null.");
            }

            PathCheck::AssertAbsolute(cefSettings->RootCachePath, "CefSettings.RootCachePath");
            PathCheck::AssertAbsolute(cefSettings->CachePath, "CefSettings.CachePath");
            PathCheck::AssertAbsolute(cefSettings->LocalesDirPath, "CefSettings.LocalesDirPath");
            PathCheck::AssertAbsolute(cefSettings->BrowserSubprocessPath, "CefSettings.BrowserSubprocessPath");


            if (performDependencyCheck)
            {
                DependencyChecker::AssertAllDependenciesPresent(cefSettings->Locale, cefSettings->LocalesDirPath, cefSettings->ResourcesDirPath, cefSettings->PackLoadingDisabled, cefSettings->BrowserSubprocessPath);
            }
            else if (!File::Exists(cefSettings->BrowserSubprocessPath))
            {
                throw gcnew FileNotFoundException("CefSettings.BrowserSubprocessPath not found.", cefSettings->BrowserSubprocessPath);
            }

            UIThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_UI));
            IOThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_IO));
            FileThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_FILE));

            //Allows us to execute Tasks on the CEF UI thread in CefSharp.dll
            CefThread::UiThreadTaskFactory = UIThreadTaskFactory;
            CefThread::CurrentOnUiThreadDelegate = gcnew Func<bool>(&CurrentOnUiThread); ;

            //To allow FolderSchemeHandlerFactory to access GetMimeType we pass in a Func
            CefSharp::SchemeHandler::FolderSchemeHandlerFactory::GetMimeTypeDelegate = gcnew Func<String^, String^>(&GetMimeType);

            CefRefPtr<CefSharpApp> app(new CefSharpApp(cefSettings, cefApp));
            CefMainArgs main_args;

            auto success = CefInitialize(main_args, *(cefSettings->_cefSettings), app.get(), NULL);

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
        /// <returns>Returns false if is invalid or the whitelist cannot be accessed.</returns>
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
        /// Returns the global cookie manager. By default data will be stored at CefSettings.CachePath if specified or in memory otherwise.
        /// Using this method is equivalent to calling Cef.GetGlobalRequestContext().GetCookieManager()
        /// The cookie managers storage is created in an async fashion, whilst this method may return a cookie manager instance,
        /// there may be a short delay before you can Get/Write cookies.
        /// To be sure the cookie manager has been initialized use one of the following
        /// - Use the GetGlobalCookieManager(ICompletionCallback) overload and access the ICookieManager after
        ///   ICompletionCallback.OnComplete has been called.
        /// - Access the ICookieManager instance in IBrowserProcessHandler.OnContextInitialized.
        /// - Use the ChromiumWebBrowser BrowserInitialized (OffScreen) or IsBrowserInitializedChanged (WinForms/WPF) events.
        /// </summary>
        /// <returns>A the global cookie manager or null if the RequestContext has not yet been initialized.</returns>
        static ICookieManager^ GetGlobalCookieManager()
        {
            return GetGlobalCookieManager(nullptr);
        }

        /// <summary>
        /// Returns the global cookie manager. By default data will be stored at CefSettings.CachePath if specified or in memory otherwise.
        /// Using this method is equivalent to calling Cef.GetGlobalRequestContext().GetCookieManager()
        /// The cookie managers storage is created in an async fashion, whilst this method may return a cookie manager instance,
        /// there may be a short delay before you can Get/Write cookies.
        /// To be sure the cookie manager has been initialized use one of the following
        /// - Access the ICookieManager after ICompletionCallback.OnComplete has been called
        /// - Access the ICookieManager instance in IBrowserProcessHandler.OnContextInitialized.
        /// - Use the ChromiumWebBrowser BrowserInitialized (OffScreen) or IsBrowserInitializedChanged (WinForms/WPF) events.
        /// </summary>
        /// <param name="callback">If non-NULL it will be executed asnychronously on the CEF UI thread after the manager's storage has been initialized.</param>
        /// <returns>A the global cookie manager or null if the RequestContext has not yet been initialized.</returns>
        static ICookieManager^ GetGlobalCookieManager(ICompletionCallback^ callback)
        {
            CefRefPtr<CefCompletionCallback> c = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

            auto cookieManager = CefCookieManager::GetGlobalManager(c);
            if (cookieManager.get())
            {
                return gcnew CookieManager(cookieManager);
            }

            return nullptr;
        }

        /// <summary>
        /// Called prior to calling Cef.Shutdown, this diposes of any remaning
        /// ChromiumWebBrowser instances. In WPF this is used from Dispatcher.ShutdownStarted
        /// to release the unmanaged resources held by the ChromiumWebBrowser instances.
        /// Generally speaking you don't need to call this yourself.
        /// </summary>
        static void PreShutdown()
        {
            msclr::lock l(_sync);

            for each(IDisposable^ diposable in Enumerable::ToList(_disposables))
            {
                delete diposable;
            }

            _disposables->Clear();

            GC::Collect();
            GC::WaitForPendingFinalizers();
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
                        throw gcnew Exception("Cef.Shutdown must be called on the same thread that Cef.Initialize was called - typically your UI thread. " +
                            "If you called Cef.Initialize on a Thread other than the UI thread then you will need to call Cef.Shutdown on the same thread. " +
                            "Cef.Initialize was called on ManagedThreadId: " + _initializedThreadId + "where Cef.Shutdown is being called on " +
                            "ManagedThreadId: " + Thread::CurrentThread->ManagedThreadId);
                    }

                    UIThreadTaskFactory = nullptr;
                    IOThreadTaskFactory = nullptr;
                    FileThreadTaskFactory = nullptr;
                    CefThread::UiThreadTaskFactory = nullptr;
                    CefThread::CurrentOnUiThreadDelegate = nullptr;

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
        /// Clear all scheme handler factories registered with the global request context.
        /// Returns false on error. This function may be called on any thread in the browser process.
        /// Using this function is equivalent to calling Cef.GetGlobalRequestContext().ClearSchemeHandlerFactories().
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
            CefVisitWebPluginInfo(new CefWebPluginInfoVisitorAdapter(visitor));
        }

        /// <summary>
        /// Async returns a list containing Plugin Information
        /// (Wrapper around CefVisitWebPluginInfo)
        /// </summary>
        /// <returns>Returns List of <see cref="WebPluginInfo"/> structs.</returns>
        static Task<List<WebPluginInfo^>^>^ GetPlugins()
        {
            auto taskVisitor = gcnew TaskWebPluginInfoVisitor();
            CefRefPtr<CefWebPluginInfoVisitorAdapter> visitor = new CefWebPluginInfoVisitorAdapter(taskVisitor);

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
        /// The earlier possible place to access the IRequestContext is in IBrowserProcessHandler.OnContextInitialized.
        /// Alternative use the ChromiumWebBrowser BrowserInitialized (OffScreen) or IsBrowserInitializedChanged (WinForms/WPF) events.
        /// </summary>
        /// <returns>Returns the global request context or null if the RequestContext has not been initialized yet.</returns>
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
        /// Helper function (wrapper around the CefColorSetARGB macro) which combines
        /// the 4 color components into an uint32 for use with BackgroundColor property
        /// </summary>
        /// <param name="a">Alpha</param>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns>Returns the color.</returns>
        static uint32 ColorSetARGB(uint32 a, uint32 r, uint32 g, uint32 b)
        {
            return CefColorSetARGB(a, r, g, b);
        }

        /// <summary>
        /// Crash reporting is configured using an INI-style config file named
        /// crash_reporter.cfg. This file must be placed next to
        /// the main application executable. File contents are as follows:
        ///
        ///  # Comments start with a hash character and must be on their own line.
        ///
        ///  [Config]
        ///  ProductName=&lt;Value of the &quot;prod&quot; crash key; defaults to &quot;cef&quot;&gt;
        ///  ProductVersion=&lt;Value of the &quot;ver&quot; crash key; defaults to the CEF version&gt;
        ///  AppName=&lt;Windows only; App-specific folder name component for storing crash
        ///           information; default to &quot;CEF&quot;&gt;
        ///  ExternalHandler=&lt;Windows only; Name of the external handler exe to use
        ///                   instead of re-launching the main exe; default to empty>
        ///  ServerURL=&lt;crash server URL; default to empty&gt;
        ///  RateLimitEnabled=&lt;True if uploads should be rate limited; default to true&gt;
        ///  MaxUploadsPerDay=&lt;Max uploads per 24 hours, used if rate limit is enabled;
        ///                    default to 5&gt;
        ///  MaxDatabaseSizeInMb=&lt;Total crash report disk usage greater than this value
        ///                       will cause older reports to be deleted; default to 20&gt;
        ///  MaxDatabaseAgeInDays=&lt;Crash reports older than this value will be deleted;
        ///                        default to 5&gt;
        ///
        ///  [CrashKeys]
        ///  my_key1=&lt;small|medium|large&gt;
        ///  my_key2=&lt;small|medium|large&gt;
        ///
        /// Config section:
        ///
        /// If &quot;ProductName&quot; and/or &quot;ProductVersion&quot; are set then the specified values
        /// will be included in the crash dump metadata. 
        ///
        /// If &quot;AppName&quot; is set on Windows then crash report information (metrics,
        /// database and dumps) will be stored locally on disk under the
        /// &quot;C:\Users\[CurrentUser]\AppData\Local\[AppName]\User Data&quot; folder. 
        ///
        /// If &quot;ExternalHandler&quot; is set on Windows then the specified exe will be
        /// launched as the crashpad-handler instead of re-launching the main process
        /// exe. The value can be an absolute path or a path relative to the main exe
        /// directory. 
        ///
        /// If &quot;ServerURL&quot; is set then crashes will be uploaded as a multi-part POST
        /// request to the specified URL. Otherwise, reports will only be stored locally
        /// on disk.
        ///
        /// If &quot;RateLimitEnabled&quot; is set to true then crash report uploads will be rate
        /// limited as follows:
        ///  1. If &quot;MaxUploadsPerDay&quot; is set to a positive value then at most the
        ///     specified number of crashes will be uploaded in each 24 hour period.
        ///  2. If crash upload fails due to a network or server error then an
        ///     incremental backoff delay up to a maximum of 24 hours will be applied for
        ///     retries.
        ///  3. If a backoff delay is applied and &quot;MaxUploadsPerDay&quot; is > 1 then the
        ///     &quot;MaxUploadsPerDay&quot; value will be reduced to 1 until the client is
        ///     restarted. This helps to avoid an upload flood when the network or
        ///     server error is resolved.
        ///
        /// If &quot;MaxDatabaseSizeInMb&quot; is set to a positive value then crash report storage
        /// on disk will be limited to that size in megabytes. For example, on Windows
        /// each dump is about 600KB so a &quot;MaxDatabaseSizeInMb&quot; value of 20 equates to
        /// about 34 crash reports stored on disk.
        ///
        /// If &quot;MaxDatabaseAgeInDays&quot; is set to a positive value then crash reports older
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
        /// if your key is named &quot;mykey&quot; then the value will be broken into ordered
        /// chunks and submitted using keys named &quot;mykey-1&quot;, &quot;mykey-2&quot;, etc.
        /// </summary>
        /// <returns>Returns true if crash reporting is enabled.</returns>
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
        /// The <see cref="CefSharp::IBrowserHost::StartDownload"/> method class can be used
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
        /// value of <see cref="CefSharp::CdmRegistrationErrorCode::IncorrectContents"/>.
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
        /// callback will receive an ErrorCode value of <see cref="CdmRegistrationErrorCode::Incompatible"/>.
        ///
        /// If registration is not supported at the time that Cef.RegisterWidevineCdm() is called then callback
        /// will receive an ErrorCode value of <see cref="CdmRegistrationErrorCode::NotSupported"/>.
        /// </summary>
        /// <param name="path"> is a directory that contains the Widevine CDM files</param>
        /// <param name="callback">optional callback - <see cref="IRegisterCdmCallback::OnRegistrationComplete"/> 
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

        /// <summary>
        /// Returns the mime type for the specified file extension or an empty string if unknown.
        /// </summary>
        /// <param name="extension">file extension</param>
        /// <returns>Returns the mime type for the specified file extension or an empty string if unknown.</returns>
        static String^ GetMimeType(String^ extension)
        {
            if (extension == nullptr)
            {
                throw gcnew ArgumentNullException("extension");
            }

            if (extension->StartsWith("."))
            {
                extension = extension->Substring(1, extension->Length - 1);
            }

            auto mimeType = StringUtils::ToClr(CefGetMimeType(StringUtils::ToNative(extension)));

            //Lookup to see if we have a custom mapping
            //MimeTypeMapping::GetCustomMapping will Fallback
            //to application/octet-stream if no mapping found
            if (String::IsNullOrEmpty(mimeType))
            {
                return MimeTypeMapping::GetCustomMapping(extension);
            }

            return mimeType;
        }

        /// <summary>
        /// WaitForBrowsersToClose is not enabled by default, call this method
        /// before Cef.Initialize to enable. If you aren't calling Cef.Initialize
        /// explicitly then this should be called before creating your first
        /// ChromiumWebBrowser instance.
        /// </summary>
        static void EnableWaitForBrowsersToClose()
        {
            if (_waitForBrowsersToCloseEnabled)
            {
                return;
            }

            if (IsInitialized)
            {
                throw gcnew Exception("Must be enabled before Cef.Initialize is called. ");
            }

            _waitForBrowsersToCloseEnabled = true;

            BrowserRefCounter::Instance = gcnew BrowserRefCounter();
        }

        /// <summary>
        /// Helper method to ensure all ChromiumWebBrowser instances have been
        /// closed/disposed, should be called before Cef.Shutdown.
        /// Disposes all remaning ChromiumWebBrowser instances
        /// then waits for CEF to release it's remaning CefBrowser instances.
        /// Finally a small delay of 50ms to allow for CEF to finish it's cleanup.
        /// Should only be called when MultiThreadedMessageLoop = true;
        /// (Hasn't been tested when when CEF integrates into main message loop).
        /// </summary>
        static void WaitForBrowsersToClose()
        {
            if (!_waitForBrowsersToCloseEnabled)
            {
                throw gcnew Exception("This feature is currently disabled. Call Cef.EnableWaitForBrowsersToClose before calling Cef.Initialize to enable.");
            }

            //Dispose of any remaining browser instances
            for each(IDisposable^ diposable in Enumerable::ToList(_disposables))
            {
                delete diposable;
            }

            //Clear the list as we've disposed of them all now.
            _disposables->Clear();

            //Wait for the browsers to close
            BrowserRefCounter::Instance->WaitForBrowsersToClose(500);

            //A few extra ms to allow for CEF to finish 
            Thread::Sleep(50);
        }
    };
}
#endif  // CEFSHARP_CORE_CEF_H_
