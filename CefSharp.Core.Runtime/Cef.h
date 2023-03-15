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
#include <include/cef_crash_util.h>
#include <include/cef_parser.h>
#include <include/cef_task.h>
#include <include/internal/cef_types.h>

#include "Internals/CefSharpApp.h"
#include "Internals/CefTaskScheduler.h"
#include "Internals/CefTaskDelegate.h"
#include "CookieManager.h"
#include "CefSettingsBase.h"
#include "RequestContext.h"

using namespace System::Collections::Generic;
using namespace System::Linq;
using namespace System::Reflection;
using namespace msclr::interop;

namespace CefSharp
{
    namespace Core
    {
        /// <summary>
        /// Global CEF methods are exposed through this class. e.g. CefInitalize maps to Cef.Initialize
        /// CEF API Doc https://magpcss.org/ceforum/apidocs3/projects/(default)/(_globals).html
        /// This class cannot be inherited.
        /// </summary>
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class Cef sealed
        {
        private:
            static Object^ _sync;

            static bool _initialized = false;
            static bool _hasShutdown = false;
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
            }

            /// <summary>Gets a value that indicates whether CefSharp was shutdown.</summary>
            /// <value>true if CefSharp was shutdown; otherwise, false.</value>
            static property bool IsShutdown
            {
                bool get()
                {
                    return _hasShutdown;
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
            /// Parse the specified url into its component parts.
            /// Uses a GURL to parse the Url. GURL is Google's URL parsing library.
            /// </summary>
            /// <param name="url">url</param>
            /// <returns>Returns null if the URL is empty or invalid.</returns>
            static UrlParts^ ParseUrl(String^ url)
            {
                if (String::IsNullOrEmpty(url))
                {
                    return nullptr;
                }

                CefURLParts parts;

                if (CefParseURL(StringUtils::ToNative(url), parts))
                {
                    auto url = gcnew UrlParts();
                    url->Fragment = StringUtils::ToClr(parts.fragment);
                    url->Host = StringUtils::ToClr(parts.host);
                    url->Origin = StringUtils::ToClr(parts.origin);
                    url->Password = StringUtils::ToClr(parts.password);
                    url->Path = StringUtils::ToClr(parts.path);
                    url->Query = StringUtils::ToClr(parts.query);
                    url->Scheme = StringUtils::ToClr(parts.scheme);
                    url->Spec = StringUtils::ToClr(parts.spec);
                    url->Username = StringUtils::ToClr(parts.username);

                    auto portString = StringUtils::ToClr(parts.port);
                    if (!String::IsNullOrEmpty(portString))
                    {
                        int port = 0;

                        if (int::TryParse(portString, port))
                        {
                            url->Port = port;
                        }
                    }

                    return url;
                }

                return nullptr;
            }

            /// <summary>
            /// Initializes CefSharp with user-provided settings.
            /// It's important to note that Initialize and Shutdown <strong>MUST</strong> be called on your main
            /// application thread (typically the UI thread). If you call them on different
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
            /// application thread (typically the UI thread). If you call them on different
            /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
            /// </summary>
            /// <param name="cefSettings">CefSharp configuration settings.</param>
            /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
            /// <returns>true if successful; otherwise, false.</returns>
            static bool Initialize(CefSettingsBase^ cefSettings, bool performDependencyCheck)
            {
                auto cefApp = gcnew DefaultApp(nullptr, cefSettings->CefCustomSchemes);

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
            /// application thread (typically the UI thread). If you call them on different
            /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
            /// </summary>
            /// <param name="cefSettings">CefSharp configuration settings.</param>
            /// <param name="performDependencyCheck">Check that all relevant dependencies avaliable, throws exception if any are missing</param>
            /// <param name="cefApp">Implement this interface to provide handler implementations. Null if you don't wish to handle these events</param>
            /// <returns>true if successful; otherwise, false.</returns>
            static bool Initialize(CefSettingsBase^ cefSettings, bool performDependencyCheck, IApp^ cefApp)
            {
                if (_initialized)
                {
                    // NOTE: Can only initialize Cef once, to make this explicitly clear throw exception on subsiquent attempts
                    throw gcnew Exception("Cef.Initialize can only be called once per process. This is a limitation of the underlying " +
                        "CEF/Chromium framework. You can change many (not all) settings at runtime through RequestContext.SetPreference. " +
                        "See https://github.com/cefsharp/CefSharp/wiki/General-Usage#request-context-browser-isolation " +
                        "Use Cef.IsInitialized to check if Cef.Initialize has already been called to avoid this exception. " +
                        "If you are seeing this unexpectedly then you are likely " +
                        "calling Cef.Initialize after you've created an instance of ChromiumWebBrowser, it must be called before the first instance is created.");
                }

                if (_hasShutdown)
                {
                    // NOTE: CefShutdown has already been called.
                    throw gcnew Exception("Cef.Shutdown has already been called. Cef.Initialize can only be called once per process. " +
                        "This is a limitation of the underlying CEF/Chromium framework. Calling Cef.Initialize after Cef.Shutdown is not supported. "
                        "You can change many (not all) settings at runtime through RequestContext.SetPreference." +
                        "See https://github.com/cefsharp/CefSharp/wiki/General-Usage#request-context-browser-isolation");
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
                FileThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_FILE_BACKGROUND));

                //Allows us to execute Tasks on the CEF UI thread in CefSharp.dll
                CefThread::Initialize(UIThreadTaskFactory, gcnew Func<bool>(&CurrentOnUiThread));

                //To allow FolderSchemeHandlerFactory to access GetMimeType we pass in a Func
                CefSharp::SchemeHandler::FolderSchemeHandlerFactory::GetMimeTypeDelegate = gcnew Func<String^, String^>(&GetMimeType);

                CefRefPtr<CefSharpApp> app(new CefSharpApp(cefSettings->ExternalMessagePump,
                                                           cefSettings->CommandLineArgsDisabled,
                                                           cefSettings->CefCommandLineArgs,
                                                           cefSettings->CefCustomSchemes,
                                                           cefApp));
                CefMainArgs main_args;
                CefSettings settings = *(cefSettings->_cefSettings);

                auto success = CefInitialize(main_args, settings, app.get(), nullptr);

                if (!success)
                {
                    CefSharp::Internals::GlobalContextInitialized::SetResult(false);
                }

                _initialized = success;
                _multiThreadedMessageLoop = cefSettings->MultiThreadedMessageLoop;

                _initializedThreadId = Thread::CurrentThread->ManagedThreadId;

                //We took a copy of CefSettings earlier, now we set our pointer to nullptr
                delete cefSettings;

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
            /// for most users; use CefSettings.MultiThreadedMessageLoop if possible (the default).
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

                return CefExecuteProcess(cefMainArgs, nullptr, nullptr);
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
                CefRefPtr<CefCompletionCallback> c = callback == nullptr ? nullptr : new CefCompletionCallbackAdapter(callback);

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
                if (_initialized)
                {
                    msclr::lock l(_sync);

                    if (_initialized)
                    {
                        if (_initializedThreadId != Thread::CurrentThread->ManagedThreadId)
                        {
                            throw gcnew Exception("Cef.Shutdown must be called on the same thread that Cef.Initialize was called - typically your UI thread. " +
                                "If you called Cef.Initialize on a Thread other than the UI thread then you will need to call Cef.Shutdown on the same thread. " +
                                "Cef.Initialize was called on ManagedThreadId: " + _initializedThreadId + " where Cef.Shutdown is being called on " +
                                "ManagedThreadId: " + Thread::CurrentThread->ManagedThreadId);
                        }

                        UIThreadTaskFactory = nullptr;
                        IOThreadTaskFactory = nullptr;
                        FileThreadTaskFactory = nullptr;

                        CefThread::Shutdown();

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
                        _initialized = false;
                        _hasShutdown = true;
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
                if (_initialized)
                {
                    msclr::lock l(_sync);

                    if (_initialized)
                    {
                        CefShutdown();
                        _initialized = false;
                        _hasShutdown = true;
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

            static int GetMinLogLevel()
            {
                return cef_get_min_log_level();
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

                if (_initialized)
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
                WaitForBrowsersToClose(750);
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
            /// <param name="timeoutInMiliseconds">The timeout in miliseconds.</param>
            static void WaitForBrowsersToClose(int timeoutInMiliseconds)
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
                BrowserRefCounter::Instance->WaitForBrowsersToClose(timeoutInMiliseconds);

                //A few extra ms to allow for CEF to finish 
                Thread::Sleep(50);
            }

            /// <summary>
            /// Post an action for delayed execution on the specified thread.
            /// </summary>
            /// <param name="threadId">thread id</param>
            /// <param name="action">action to execute</param>
            /// <param name="delayInMs">delay in ms</param>
            /// <returns>bool</returns>
            static bool PostDelayedAction(CefThreadIds threadId, Action^ action, int delayInMs)
            {
                auto task = new CefTaskDelegate(action);

                return CefPostDelayedTask((cef_thread_id_t)threadId, task, delayInMs);
            }

            /// <summary>
            /// Post an action for execution on the specified thread.
            /// </summary>
            /// <param name="threadId">thread id</param>
            /// <param name="action">action to execute</param>
            /// <returns>bool</returns>
            static bool PostAction(CefThreadIds threadId, Action^ action)
            {
                auto task = new CefTaskDelegate(action);

                return CefPostTask((cef_thread_id_t)threadId, task);
            }
        };
    }
}
#endif  // CEFSHARP_CORE_CEF_H_
