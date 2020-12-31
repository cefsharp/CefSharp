// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Handler;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class SelfHost
    {
        /// <summary>
        /// This function should be called from the application entry point function (typically Program.Main)
        /// to execute a secondary process e.g. gpu, plugin, renderer, utility
        /// This overload is specifically used for .Net Core. For hosting your own BrowserSubProcess
        /// it's preferable to use the Main method provided by this class.
        /// - Pass in command line args
        /// - To support High DPI Displays you should call  Cef.EnableHighDPISupport before any other processing
        /// or add the relevant entries to your app.manifest
        /// </summary>
        /// <param name="args">command line args</param>
        /// <returns>
        /// If called for the browser process (identified by no "type" command-line value) it will return immediately
        /// with a value of -1. If called for a recognized secondary process it will block until the process should exit
        /// and then return the process exit code.
        /// </returns>
        public static int Main(string[] args)
        {
            var type = CommandLineArgsParser.GetArgumentValue(args, CefSharpArguments.SubProcessTypeArgument);

            if (string.IsNullOrEmpty(type))
            {
                //If --type param missing from command line CEF/Chromium assums
                //this is the main process (as all subprocesses must have a type param).
                //Return -1 to indicate this behaviour.
                return -1;
            }

            var browserSubprocessDllPath = Path.Combine(Path.GetDirectoryName(typeof(CefSharp.Core.BrowserSettings).Assembly.Location), "CefSharp.BrowserSubprocess.Core.dll");
#if NETCOREAPP
            var browserSubprocessDll = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(browserSubprocessDllPath);
#else
            var browserSubprocessDll = System.Reflection.Assembly.LoadFrom(browserSubprocessDllPath);
#endif
            var browserSubprocessExecutableType = browserSubprocessDll.GetType("CefSharp.BrowserSubprocess.BrowserSubprocessExecutable");
            var browserSubprocessExecutable = Activator.CreateInstance(browserSubprocessExecutableType);

            var mainMethod = browserSubprocessExecutableType.GetMethod("MainSelfHost", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            var argCount = mainMethod.GetParameters();

            var methodArgs = new object[] {args } ;

            var exitCode = mainMethod.Invoke(null, methodArgs);

            return (int)exitCode;
        }
    }
}

namespace CefSharp.Core
{
    public static class ObjectFactory
    {
        /// <summary>
        /// Create a new instance of <see cref="IBrowserSettings"/>
        /// </summary>
        /// <param name="autoDispose">Dispose of browser setings after it has been used to create a browser</param>
        /// <returns>returns new instance of <see cref="IBrowserSettings"/></returns>
        public static IBrowserSettings CreateBrowserSettings(bool autoDispose)
        {
            return new CefSharp.Core.BrowserSettings(autoDispose);
        }

        /// <summary>
        /// Create a new instance of <see cref="IWindowInfo"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IWindowInfo"/></returns>
        public static IWindowInfo CreateWindowInfo()
        {
            return new CefSharp.Core.WindowInfo();
        }

        /// <summary>
        /// Create a new instance of <see cref="IPostData"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IPostData"/></returns>
        public static IPostData CreatePostData()
        {
            return new CefSharp.Core.PostData();
        }

        /// <summary>
        /// Create a new instance of <see cref="IPostDataElement"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IPostDataElement"/></returns>
        public static IPostDataElement CreatePostDataElement()
        {
            return new CefSharp.Core.PostDataElement();
        }

        /// <summary>
        /// Create a new instance of <see cref="IRequest"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IRequest"/></returns>
        public static IRequest CreateRequest()
        {
            return new CefSharp.Core.Request();
        }

        /// <summary>
        /// Create a new instance of <see cref="IUrlRequest"/>
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="urlRequestClient">url request client</param>
        /// <returns>returns new instance of <see cref="IUrlRequest"/></returns>
        public static IUrlRequest CreateUrlRequest(IRequest request, IUrlRequestClient urlRequestClient)
        {
            return new CefSharp.Core.UrlRequest(request, urlRequestClient);
        }

        /// <summary>
        /// Create a new instance of <see cref="IUrlRequest"/>
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="urlRequestClient">url request client</param>
        /// <param name="requestContext">request context</param>
        /// <returns>returns new instance of <see cref="IUrlRequest"/></returns>
        public static IUrlRequest CreateUrlRequest(IRequest request, IUrlRequestClient urlRequestClient, IRequestContext requestContext)
        {
            return new CefSharp.Core.UrlRequest(request, urlRequestClient, requestContext);
        }
    }
}

//TODO: Split out into seperate classes
namespace CefSharp
{
    /// <inheritdoc/>
    public class BrowserSettings : CefSharp.Core.BrowserSettings
    {
        /// <inheritdoc/>
        public BrowserSettings(bool autoDispose = false) : base(autoDispose)
        {

        }
    }

    /// <inheritdoc/>
    public static class Cef
    {
        public static TaskFactory UIThreadTaskFactory
        {
            get { return Core.Cef.UIThreadTaskFactory; }
        }
        public static TaskFactory IOThreadTaskFactory
        {
            get { return Core.Cef.IOThreadTaskFactory; }
        }
        public static TaskFactory FileThreadTaskFactory
        {
            get { return Core.Cef.FileThreadTaskFactory; }
        }

        public static void AddDisposable(IDisposable item)
        {
            Core.Cef.AddDisposable(item);
        }

        public static void RemoveDisposable(IDisposable item)
        {
            Core.Cef.RemoveDisposable(item);
        }

        /// <summary>Gets a value that indicates whether CefSharp is initialized.</summary>
        /// <value>true if CefSharp is initialized; otherwise, false.</value>
        public static bool IsInitialized
        {
            get { return Core.Cef.IsInitialized; }
        }

        /// <summary>Gets a value that indicates the version of CefSharp currently being used.</summary>
        /// <value>The CefSharp version.</value>
        public static string CefSharpVersion
        {
            get { return Core.Cef.CefSharpVersion; }
        }

        /// <summary>Gets a value that indicates the CEF version currently being used.</summary>
        /// <value>The CEF Version</value>
        public static string CefVersion
        {
            get { return Core.Cef.CefVersion; }
        }

        /// <summary>Gets a value that indicates the Chromium version currently being used.</summary>
        /// <value>The Chromium version.</value>
        public static string ChromiumVersion
        {
            get { return Core.Cef.ChromiumVersion; }
        }

        /// <summary>
        /// Gets a value that indicates the Git Hash for CEF version currently being used.
        /// </summary>
        /// <value>The Git Commit Hash</value>
        public static string CefCommitHash
        {
            get { return Core.Cef.CefCommitHash; }
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize and Shutdown <strong>MUST</strong> be called on your main
        /// application thread (typically the UI thread). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="settings">CefSharp configuration settings.</param>
        /// <returns>true if successful; otherwise, false.</returns>
        public static bool Initialize(CefSettingsBase settings)
        {
            using (settings.settings)
            {
                return Core.Cef.Initialize(settings.settings);
            }
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize/Shutdown <strong>MUST</strong> be called on your main
        /// application thread (typically the UI thread). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="settings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies available, throws exception if any are missing</param>
        /// <returns>true if successful; otherwise, false.</returns>
        public static bool Initialize(CefSettingsBase settings, bool performDependencyCheck)
        {
            using (settings.settings)
            {
                return Core.Cef.Initialize(settings.settings, performDependencyCheck);
            }
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize/Shutdown <strong>MUST</strong> be called on your main
        /// application thread (typically the UI thread). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="settings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies available, throws exception if any are missing</param>
        /// <param name="browserProcessHandler">The handler for functionality specific to the browser process. Null if you don't wish to handle these events</param>
        /// <returns>true if successful; otherwise, false.</returns>
        public static bool Initialize(CefSettingsBase settings, bool performDependencyCheck, IBrowserProcessHandler browserProcessHandler)
        {
            using (settings.settings)
            {
                return Core.Cef.Initialize(settings.settings, performDependencyCheck, browserProcessHandler);
            }
        }

        /// <summary>
        /// Initializes CefSharp with user-provided settings.
        /// It's important to note that Initialize/Shutdown <strong>MUST</strong> be called on your main
        /// application thread (typically the UI thread). If you call them on different
        /// threads, your application will hang. See the documentation for Cef.Shutdown() for more details.
        /// </summary>
        /// <param name="settings">CefSharp configuration settings.</param>
        /// <param name="performDependencyCheck">Check that all relevant dependencies available, throws exception if any are missing</param>
        /// <param name="cefApp">Implement this interface to provide handler implementations. Null if you don't wish to handle these events</param>
        /// <returns>true if successful; otherwise, false.</returns>
        public static bool Initialize(CefSettingsBase settings, bool performDependencyCheck, IApp cefApp)
        {
            using (settings.settings)
            {
                return Core.Cef.Initialize(settings.settings, performDependencyCheck, cefApp);
            }
        }

        /// <summary>
        /// Run the CEF message loop. Use this function instead of an application-
        /// provided message loop to get the best balance between performance and CPU
        /// usage. This function should only be called on the main application thread and
        /// only if Cef.Initialize() is called with a
        /// CefSettings.MultiThreadedMessageLoop value of false. This function will
        /// block until a quit message is received by the system.
        /// </summary>
        public static void RunMessageLoop()
        {
            Core.Cef.RunMessageLoop();
        }

        /// <summary>
        /// Quit the CEF message loop that was started by calling Cef.RunMessageLoop().
        /// This function should only be called on the main application thread and only
        /// if Cef.RunMessageLoop() was used.
        /// </summary>
        public static void QuitMessageLoop()
        {
            Core.Cef.QuitMessageLoop();
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
        public static void DoMessageLoopWork()
        {
            Core.Cef.DoMessageLoopWork();
        }

        /// <summary>
        /// This function should be called from the application entry point function to execute a secondary process.
        /// It can be used to run secondary processes from the browser client executable (default behavior) or
        /// from a separate executable specified by the CefSettings.browser_subprocess_path value.
        /// If called for the browser process (identified by no "type" command-line value) it will return immediately with a value of -1.
        /// If called for a recognized secondary process it will block until the process should exit and then return the process exit code.
        /// The |application| parameter may be empty. The |windows_sandbox_info| parameter is only used on Windows and may be NULL (see cef_sandbox_win.h for details). 
        /// </summary>
        public static int ExecuteProcess()
        {
            return Core.Cef.ExecuteProcess();
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
        ///
        /// Scripts in separate frames or iframes and hosted from the same protocol and
        /// domain suffix can execute cross-origin JavaScript if both pages set the
        /// document.domain value to the same domain suffix. For example,
        /// scheme://foo.example.com and scheme://bar.example.com can communicate using
        /// JavaScript if both domains set document.domain="example.com".
        ///
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
        ///
        /// This method cannot be used to bypass the restrictions on local or display
        /// isolated schemes. See the comments on <see cref="CefCustomScheme"/> for more
        /// information.
        ///
        /// This function may be called on any thread. Returns false if <paramref name="sourceOrigin"/>
        /// is invalid or the whitelist cannot be accessed.
        /// </remarks>
        public static bool AddCrossOriginWhitelistEntry(
            string sourceOrigin,
            string targetProtocol,
            string targetDomain,
            bool allowTargetSubdomains)
        {
            return Core.Cef.AddCrossOriginWhitelistEntry(
                sourceOrigin,
                targetProtocol,
                targetDomain,
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
        public static bool RemoveCrossOriginWhitelistEntry(string sourceOrigin,
            string targetProtocol,
            string targetDomain,
            bool allowTargetSubdomains)

        {
            return Core.Cef.RemoveCrossOriginWhitelistEntry(
                sourceOrigin,
                targetProtocol,
                targetDomain,
                allowTargetSubdomains);
        }

        /// <summary>Remove all entries from the cross-origin access whitelist.</summary>
        /// <remarks>
        /// Remove all entries from the cross-origin access whitelist. Returns false if
        /// the whitelist cannot be accessed.
        /// </remarks>
        public static bool ClearCrossOriginWhitelist()
        {
            return Core.Cef.ClearCrossOriginWhitelist();
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
        /// <param name="callback">If non-NULL it will be executed asynchronously on the CEF UI thread after the manager's storage has been initialized.</param>
        /// <returns>A the global cookie manager or null if the RequestContext has not yet been initialized.</returns>
        public static ICookieManager GetGlobalCookieManager(ICompletionCallback callback = null)
        {


            return Core.Cef.GetGlobalCookieManager(callback);
        }

        /// <summary>
        /// Called prior to calling Cef.Shutdown, this disposes of any remaining
        /// ChromiumWebBrowser instances. In WPF this is used from Dispatcher.ShutdownStarted
        /// to release the unmanaged resources held by the ChromiumWebBrowser instances.
        /// Generally speaking you don't need to call this yourself.
        /// </summary>
        public static void PreShutdown()
        {
            Core.Cef.PreShutdown();
        }

        /// <summary>
        /// Shuts down CefSharp and the underlying CEF infrastructure. This method is safe to call multiple times; it will only
        /// shut down CEF on the first call (all subsequent calls will be ignored).
        /// This method should be called on the main application thread to shut down the CEF browser process before the application exits. 
        /// If you are Using CefSharp.OffScreen then you must call this explicitly before your application exits or it will hang.
        /// This method must be called on the same thread as Initialize. If you don't call Shutdown explicitly then CefSharp.Wpf and CefSharp.WinForms
        /// versions will do their best to call Shutdown for you, if your application is having trouble closing then call thus explicitly.
        /// </summary>
        public static void Shutdown()
        {
            Core.Cef.Shutdown();
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
        public static void ShutdownWithoutChecks()
        {
            Core.Cef.ShutdownWithoutChecks();
        }

        /// <summary>
        /// Clear all scheme handler factories registered with the global request context.
        /// Returns false on error. This function may be called on any thread in the browser process.
        /// Using this function is equivalent to calling Cef.GetGlobalRequestContext().ClearSchemeHandlerFactories().
        /// </summary>
        /// <returns>Returns false on error.</returns>
        public static bool ClearSchemeHandlerFactories()
        {
            return Core.Cef.ClearSchemeHandlerFactories();
        }

        /// <summary>
        /// Visit web plugin information. Can be called on any thread in the browser process.
        /// </summary>
        public static void VisitWebPluginInfo(IWebPluginInfoVisitor visitor)
        {
            Core.Cef.VisitWebPluginInfo(visitor);
        }

        /// <summary>
        /// Async returns a list containing Plugin Information
        /// (Wrapper around CefVisitWebPluginInfo)
        /// </summary>
        /// <returns>Returns List of <see cref="WebPluginInfo"/> structs.</returns>
        public static Task<List<WebPluginInfo>> GetPlugins()
        {
            return Core.Cef.GetPlugins();
        }

        /// <summary>
        /// Cause the plugin list to refresh the next time it is accessed regardless of whether it has already been loaded.
        /// </summary>
        public static void RefreshWebPlugins()
        {
            Core.Cef.RefreshWebPlugins();
        }

        /// <summary>
        /// Unregister an internal plugin. This may be undone the next time RefreshWebPlugins() is called. 
        /// </summary>
        /// <param name="path">Path (directory + file).</param>
        public static void UnregisterInternalWebPlugin(string path)
        {
            Core.Cef.UnregisterInternalWebPlugin(path);
        }

        /// <summary>
        /// Call during process startup to enable High-DPI support on Windows 7 or newer.
        /// Older versions of Windows should be left DPI-unaware because they do not
        /// support DirectWrite and GDI fonts are kerned very badly.
        /// </summary>
        public static void EnableHighDPISupport()
        {
            Core.Cef.EnableHighDPISupport();
        }

        /// <summary>
        /// Returns true if called on the specified CEF thread.
        /// </summary>
        /// <returns>Returns true if called on the specified thread.</returns>
        public static bool CurrentlyOnThread(CefThreadIds threadId)
        {
            return Core.Cef.CurrentlyOnThread(threadId);
        }

        /// <summary>
        /// Gets the Global Request Context. Make sure to Dispose of this object when finished.
        /// The earlier possible place to access the IRequestContext is in IBrowserProcessHandler.OnContextInitialized.
        /// Alternative use the ChromiumWebBrowser BrowserInitialized (OffScreen) or IsBrowserInitializedChanged (WinForms/WPF) events.
        /// </summary>
        /// <returns>Returns the global request context or null if the RequestContext has not been initialized yet.</returns>
        public static IRequestContext GetGlobalRequestContext()
        {
            return Core.Cef.GetGlobalRequestContext();
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
        public static UInt32 ColorSetARGB(UInt32 a, UInt32 r, UInt32 g, UInt32 b)
        {
            return Core.Cef.ColorSetARGB(a, r, g, b);
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
        public static bool CrashReportingEnabled
        {
            get { return Core.Cef.CrashReportingEnabled; }
        }

        /// <summary>
        /// Sets or clears a specific key-value pair from the crash metadata.
        /// </summary>
        public static void SetCrashKeyValue(string c, string value)
        {
            Core.Cef.SetCrashKeyValue(value, value);
        }

        /// <summary>
        /// Register the Widevine CDM plugin.
        /// 
        /// The client application is responsible for downloading an appropriate
        /// platform-specific CDM binary distribution from Google, extracting the
        /// contents, and building the required directory structure on the local machine.
        /// The <see cref="CefSharp.IBrowserHost.StartDownload"/> method class can be used
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
        /// value of <see cref="CefSharp.CdmRegistrationErrorCode.IncorrectContents"/>.
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
        /// <param name="callback">optional callback - <see cref="IRegisterCdmCallback.OnRegistrationComplete"/> 
        /// will be executed asynchronously once registration is complete</param>
        public static void RegisterWidevineCdm(string path, IRegisterCdmCallback callback = null)
        {
            Core.Cef.RegisterWidevineCdm(path, callback);
        }

        /// <summary>
        /// Register the Widevine CDM plugin.
        ///
        /// See <see cref="RegisterWidevineCdm(string, IRegisterCdmCallback)"/> for more details.
        /// </summary>
        /// <param name="path"> is a directory that contains the Widevine CDM files</param>
        /// <returns>Returns a Task that can be awaited to receive the <see cref="CdmRegistration"/> response.</returns>
        public static Task<CdmRegistration> RegisterWidevineCdmAsync(string path)
        {
            return Core.Cef.RegisterWidevineCdmAsync(path);
        }

        /// <summary>
        /// Returns the mime type for the specified file extension or an empty string if unknown.
        /// </summary>
        /// <param name="extension">file extension</param>
        /// <returns>Returns the mime type for the specified file extension or an empty string if unknown.</returns>
        public static string GetMimeType(string extension)
        {
            return Core.Cef.GetMimeType(extension);
        }

        /// <summary>
        /// WaitForBrowsersToClose is not enabled by default, call this method
        /// before Cef.Initialize to enable. If you aren't calling Cef.Initialize
        /// explicitly then this should be called before creating your first
        /// ChromiumWebBrowser instance.
        /// </summary>
        public static void EnableWaitForBrowsersToClose()
        {
            Core.Cef.EnableWaitForBrowsersToClose();
        }

        /// <summary>
        /// Helper method to ensure all ChromiumWebBrowser instances have been
        /// closed/disposed, should be called before Cef.Shutdown.
        /// Disposes all remaining ChromiumWebBrowser instances
        /// then waits for CEF to release its remaining CefBrowser instances.
        /// Finally a small delay of 50ms to allow for CEF to finish it's cleanup.
        /// Should only be called when MultiThreadedMessageLoop = true;
        /// (Hasn't been tested when when CEF integrates into main message loop).
        /// </summary>
        public static void WaitForBrowsersToClose()
        {
            Core.Cef.WaitForBrowsersToClose();
        }
    }

    /// <summary>
    /// Initialization settings. Many of these and other settings can also configured using command-line switches.
    /// WPF/WinForms/OffScreen each have their own CefSettings implementation that sets
    /// relevant settings e.g. OffScreen starts with audio muted.
    /// </summary>
    public abstract class CefSettingsBase
    {
        internal Core.CefSettingsBase settings = new Core.CefSettingsBase();

#if NETCOREAPP
        public CefSettingsBase() : base()
        {
            if(!File.Exists(BrowserSubprocessPath))
            {
                if(Initializer.LibCefLoaded)
                {
                    BrowserSubprocessPath = Initializer.BrowserSubProcessPath;
                }
            }
        }
#endif
        /// <summary>
        /// Free the unmanaged CefSettingsBase instance.
        /// Under normal circumstances you shouldn't need to call this
        /// The unmanaged resource will be freed after <see cref="Cef.Initialize(CefSettingsBase)"/> (or one of the overloads) is called.
        /// </summary>
        public void Dispose()
        {
            settings?.Dispose();
            settings = null;
        }

        /// <summary>
        /// Add Customs schemes to this collection.
        /// </summary>
        public IEnumerable<CefCustomScheme> CefCustomSchemes
        {
            get { return settings.CefCustomSchemes; }
        }

        /// <summary>
        /// Add custom command line argumens to this collection, they will be added in OnBeforeCommandLineProcessing. The
        /// CefSettings.CommandLineArgsDisabled value can be used to start with an empty command-line object. Any values specified in
        /// CefSettings that equate to command-line arguments will be set before this method is called.
        /// </summary>
        public CommandLineArgDictionary CefCommandLineArgs
        {
            get { return settings.CefCommandLineArgs; }
        }

        /// <summary>
        /// Set to true to disable configuration of browser process features using standard CEF and Chromium command-line arguments.
        /// Configuration can still be specified using CEF data structures or by adding to CefCommandLineArgs.
        /// </summary>
        public bool CommandLineArgsDisabled
        {
            get { return settings.CommandLineArgsDisabled; }
            set { settings.CommandLineArgsDisabled = value; }
        }

        /// <summary>
        /// Set to true to control browser process main (UI) thread message pump scheduling via the
        /// IBrowserProcessHandler.OnScheduleMessagePumpWork callback. This option is recommended for use in combination with the
        /// Cef.DoMessageLoopWork() function in cases where the CEF message loop must be integrated into an existing application message
        /// loop (see additional comments and warnings on Cef.DoMessageLoopWork). Enabling this option is not recommended for most users;
        /// leave this option disabled and use either MultiThreadedMessageLoop (the default) if possible.
        /// </summary>
        public bool ExternalMessagePump
        {
            get { return settings.ExternalMessagePump; }
            set { settings.ExternalMessagePump = value; }
        }

        /// <summary>
        /// Set to true to have the browser process message loop run in a separate thread. If false than the CefDoMessageLoopWork()
        /// function must be called from your application message loop. This option is only supported on Windows. The default value is
        /// true.
        /// </summary>
        public bool MultiThreadedMessageLoop
        {
            get { return settings.MultiThreadedMessageLoop; }
            set { settings.MultiThreadedMessageLoop = value; }
        }

        /// <summary>
        /// The path to a separate executable that will be launched for sub-processes. By default the browser process executable is used.
        /// See the comments on Cef.ExecuteProcess() for details. If this value is non-empty then it must be an absolute path.
        /// Also configurable using the "browser-subprocess-path" command-line switch.
        /// Defaults to using the provided CefSharp.BrowserSubprocess.exe instance
        /// </summary>
        public string BrowserSubprocessPath
        {
            get { return settings.BrowserSubprocessPath; }
            set { settings.BrowserSubprocessPath = value; }
        }

        /// <summary>
        /// The location where data for the global browser cache will be stored on disk. In this value is non-empty then it must be
        /// an absolute path that is must be either equal to or a child directory of CefSettings.RootCachePath (if RootCachePath is
        /// empty it will default to this value). If the value is empty then browsers will be created in "incognito mode" where
        /// in-memory caches are used for storage and no data is persisted to disk. HTML5 databases such as localStorage will only
        /// persist across sessions if a cache path is specified. Can be overridden for individual RequestContext instances via the
        /// RequestContextSettings.CachePath value.
        /// </summary>
        public string CachePath
        {
            get { return settings.CachePath; }
            set { settings.CachePath = value; }
        }

        /// <summary>
        /// The root directory that all CefSettings.CachePath and RequestContextSettings.CachePath values must have in common. If this
        /// value is empty and CefSettings.CachePath is non-empty then it will default to the CefSettings.CachePath value.
        /// If this value is non-empty then it must be an absolute path.  Failure to set this value correctly may result in the sandbox
        /// blocking read/write access to the CachePath directory. NOTE: CefSharp does not implement the CHROMIUM SANDBOX. A non-empty
        /// RootCachePath can be used in conjuncation with an empty CefSettings.CachePath in instances where you would like browsers
        /// attached to the Global RequestContext (the default) created in "incognito mode" and instances created with a custom
        /// RequestContext using a disk based cache.
        /// </summary>
        public string RootCachePath
        {
            get { return settings.RootCachePath; }
            set { settings.RootCachePath = value; }
        }

        /// <summary>
        /// The location where user data such as spell checking dictionary files will be stored on disk. If this value is empty then the
        /// default user data directory will be used ("Local Settings\Application Data\CEF\User Data" directory under the user
        /// profile directory on Windows). If this value is non-empty then it must be an absolute path.
        /// </summary>
        public string UserDataPath
        {
            get { return settings.UserDataPath; }
            set { settings.UserDataPath = value; }
        }

        /// <summary>
        /// Set to true in order to completely ignore SSL certificate errors. This is NOT recommended.
        /// </summary>
        public bool IgnoreCertificateErrors
        {
            get { return settings.IgnoreCertificateErrors; }
            set { settings.IgnoreCertificateErrors = value; }
        }

        /// <summary>
        /// The locale string that will be passed to WebKit. If empty the default locale of "en-US" will be used. Also configurable using
        /// the "lang" command-line switch.
        /// </summary>
        public string Locale
        {
            get { return settings.Locale; }
            set { settings.Locale = value; }
        }

        /// <summary>
        /// The fully qualified path for the locales directory. If this value is empty the locales directory must be located in the
        /// module directory. If this value is non-empty then it must be an absolute path. Also configurable using the "locales-dir-path"
        /// command-line switch.
        /// </summary>
        public string LocalesDirPath
        {
            get { return settings.LocalesDirPath; }
            set { settings.LocalesDirPath = value; }
        }

        /// <summary>
        /// The fully qualified path for the resources directory. If this value is empty the cef.pak and/or devtools_resources.pak files
        /// must be located in the module directory. Also configurable using the "resources-dir-path" command-line switch.
        /// </summary>
        public string ResourcesDirPath
        {
            get { return settings.ResourcesDirPath; }
            set { settings.ResourcesDirPath = value; }
        }

        /// <summary>
        /// The directory and file name to use for the debug log. If empty a default log file name and location will be used. On Windows
        /// a "debug.log" file will be written in the main executable directory. Also configurable using the"log-file" command- line
        /// switch.
        /// </summary>
        public string LogFile
        {
            get { return settings.LogFile; }
            set { settings.LogFile = value; }
        }

        /// <summary>
        /// The log severity. Only messages of this severity level or higher will be logged. When set to
        /// <see cref="CefSharp.LogSeverity.Disable"/> no messages will be written to the log file, but Fatal messages will still be
        /// output to stderr. Also configurable using the "log-severity" command-line switch with a value of "verbose", "info", "warning",
        /// "error", "fatal", "error-report" or "disable".
        /// </summary>
        public CefSharp.LogSeverity LogSeverity
        {
            get { return settings.LogSeverity; }
            set { settings.LogSeverity = value; }
        }

        /// <summary>
        /// Custom flags that will be used when initializing the V8 JavaScript engine. The consequences of using custom flags may not be
        /// well tested. Also configurable using the "js-flags" command-line switch.
        /// </summary>
        public string JavascriptFlags
        {
            get { return settings.JavascriptFlags; }
            set { settings.JavascriptFlags = value; }
        }

        /// <summary>
        /// Set to true to disable loading of pack files for resources and locales. A resource bundle handler must be provided for the
        /// browser and render processes via CefApp.GetResourceBundleHandler() if loading of pack files is disabled. Also configurable
        /// using the "disable-pack-loading" command- line switch.
        /// </summary>
        public bool PackLoadingDisabled
        {
            get { return settings.PackLoadingDisabled; }
            set { settings.PackLoadingDisabled = value; }
        }

        /// <summary>
        /// Value that will be inserted as the product portion of the default User-Agent string. If empty the Chromium product version
        /// will be used. If UserAgent is specified this value will be ignored. Also configurable using the "product-version" command-
        /// line switch.
        /// </summary>
        public string ProductVersion
        {
            get { return settings.ProductVersion; }
            set { settings.ProductVersion = value; }
        }

        /// <summary>
        /// Set to a value between 1024 and 65535 to enable remote debugging on the specified port. For example, if 8080 is specified the
        /// remote debugging URL will be http://localhost:8080. CEF can be remotely debugged from any CEF or Chrome browser window. Also
        /// configurable using the "remote-debugging-port" command-line switch.
        /// </summary>
        public int RemoteDebuggingPort
        {
            get { return settings.RemoteDebuggingPort; }
            set { settings.RemoteDebuggingPort = value; }
        }

        /// <summary>
        /// The number of stack trace frames to capture for uncaught exceptions. Specify a positive value to enable the
        /// CefRenderProcessHandler. OnUncaughtException() callback. Specify 0 (default value) and OnUncaughtException() will not be
        /// called. Also configurable using the "uncaught-exception-stack-size" command-line switch.
        /// </summary>
        public int UncaughtExceptionStackSize
        {
            get { return settings.UncaughtExceptionStackSize; }
            set { settings.UncaughtExceptionStackSize = value; }
        }

        /// <summary>
        /// Value that will be returned as the User-Agent HTTP header. If empty the default User-Agent string will be used. Also
        /// configurable using the "user-agent" command-line switch.
        /// </summary>
        public string UserAgent
        {
            get { return settings.UserAgent; }
            set { settings.UserAgent = value; }
        }

        /// <summary>
        /// Set to true (1) to enable windowless (off-screen) rendering support. Do not enable this value if the application does not use
        /// windowless rendering as it may reduce rendering performance on some systems.
        /// </summary>
        public bool WindowlessRenderingEnabled
        {
            get { return settings.WindowlessRenderingEnabled; }
            set { settings.WindowlessRenderingEnabled = value; }
        }

        /// <summary>
        /// To persist session cookies (cookies without an expiry date or validity interval) by default when using the global cookie
        /// manager set this value to true. Session cookies are generally intended to be transient and most Web browsers do not persist
        /// them. A CachePath value must also be specified to enable this feature. Also configurable using the "persist-session-cookies"
        /// command-line switch. Can be overridden for individual RequestContext instances via the
        /// RequestContextSettings.PersistSessionCookies value.
        /// </summary>
        public bool PersistSessionCookies
        {
            get { return settings.PersistSessionCookies; }
            set { settings.PersistSessionCookies = value; }
        }

        /// <summary>
        /// To persist user preferences as a JSON file in the cache path directory set this value to true. A CachePath value must also be
        /// specified to enable this feature. Also configurable using the "persist-user-preferences" command-line switch. Can be
        /// overridden for individual RequestContext instances via the RequestContextSettings.PersistUserPreferences value.
        /// </summary>
        public bool PersistUserPreferences
        {
            get { return settings.PersistUserPreferences; }
            set { settings.PersistUserPreferences = value; }
        }

        /// <summary>
        /// Comma delimited ordered list of language codes without any whitespace that will be used in the "Accept-Language" HTTP header.
        /// May be set globally using the CefSettings.AcceptLanguageList value. If both values are empty then "en-US,en" will be used.
        /// 
        /// </summary>
        public string AcceptLanguageList
        {
            get { return settings.AcceptLanguageList; }
            set { settings.AcceptLanguageList = value; }
        }

        /// <summary>
        /// Background color used for the browser before a document is loaded and when no document color is specified. The alpha
        /// component must be either fully opaque (0xFF) or fully transparent (0x00). If the alpha component is fully opaque then the RGB
        /// components will be used as the background color. If the alpha component is fully transparent for a WinForms browser then the
        /// default value of opaque white be used. If the alpha component is fully transparent for a windowless (WPF/OffScreen) browser
        /// then transparent painting will be enabled.
        /// </summary>
        public UInt32 BackgroundColor
        {
            get { return settings.BackgroundColor; }
            set { settings.BackgroundColor = value; }
        }

        /// <summary>
        /// GUID string used for identifying the application. This is passed to the system AV function for scanning downloaded files. By
        /// default, the GUID will be an empty string and the file will be treated as an untrusted file when the GUID is empty.
        /// </summary>
        public string ApplicationClientIdForFileScanning
        {
            get { return settings.ApplicationClientIdForFileScanning; }
            set { settings.ApplicationClientIdForFileScanning = value; }
        }

        /// <summary>
        /// Registers a custom scheme using the provided settings.
        /// </summary>
        /// <param name="scheme">The CefCustomScheme which provides the details about the scheme.</param>
        public void RegisterScheme(CefCustomScheme scheme)
        {
            settings.RegisterScheme(scheme);
        }

        /// <summary>
        /// Set command line argument to disable GPU Acceleration. WebGL will use
        /// software rendering via Swiftshader (https://swiftshader.googlesource.com/SwiftShader#introduction)
        /// </summary>
        public void DisableGpuAcceleration()
        {
            if (!settings.CefCommandLineArgs.ContainsKey("disable-gpu"))
            {
                settings.CefCommandLineArgs.Add("disable-gpu");
            }
        }

        /// <summary>
        /// Set command line argument to enable Print Preview See
        /// https://bitbucket.org/chromiumembedded/cef/issues/123/add-support-for-print-preview for details.
        /// </summary>
        public void EnablePrintPreview()
        {
            if (!settings.CefCommandLineArgs.ContainsKey("enable-print-preview"))
            {
                settings.CefCommandLineArgs.Add("enable-print-preview");
            }
        }

        /// <summary>
        /// Set command line arguments for best OSR (Offscreen and WPF) Rendering performance Swiftshader will be used for WebGL, look at the source
        /// to determine which flags best suite your requirements. See https://swiftshader.googlesource.com/SwiftShader#introduction for
        /// details on Swiftshader
        /// </summary>
        public void SetOffScreenRenderingBestPerformanceArgs()
        {
            // Use software rendering and compositing (disable GPU) for increased FPS
            // and decreased CPU usage. 
            // See https://bitbucket.org/chromiumembedded/cef/issues/1257 for details.
            if (!settings.CefCommandLineArgs.ContainsKey("disable-gpu"))
            {
                settings.CefCommandLineArgs.Add("disable-gpu");
            }

            if (!settings.CefCommandLineArgs.ContainsKey("disable-gpu-compositing"))
            {
                settings.CefCommandLineArgs.Add("disable-gpu-compositing");
            }

            // Synchronize the frame rate between all processes. This results in
            // decreased CPU usage by avoiding the generation of extra frames that
            // would otherwise be discarded. The frame rate can be set at browser
            // creation time via IBrowserSettings.WindowlessFrameRate or changed
            // dynamically using IBrowserHost.SetWindowlessFrameRate. In cefclient
            // it can be set via the command-line using `--off-screen-frame-rate=XX`.
            // See https://bitbucket.org/chromiumembedded/cef/issues/1368 for details.
            if (!settings.CefCommandLineArgs.ContainsKey("enable-begin-frame-scheduling"))
            {
                settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling");
            }
        }
    }

    /// <summary>
    /// Create <see cref="IBrowserAdapter"/> instance via <see cref="Create(IWebBrowserInternal, bool)"/>
    /// This is the primary object for bridging the ChromiumWebBrowser implementation and VC++
    /// </summary>
    public static class ManagedCefBrowserAdapter
    {
        public static IBrowserAdapter Create(IWebBrowserInternal webBrowserInternal, bool offScreenRendering)
        {
            return new CefSharp.Core.ManagedCefBrowserAdapter(webBrowserInternal, offScreenRendering);
        }
    }

    /// <inheritdoc/>
    public static class NativeMethodWrapper
    {
        public static void MemoryCopy(IntPtr dest, IntPtr src, int numberOfBytes)
        {
            CefSharp.Core.NativeMethodWrapper.MemoryCopy(dest, src, numberOfBytes);
        }

        public static bool IsFocused(IntPtr handle)
        {
            return CefSharp.Core.NativeMethodWrapper.IsFocused(handle);
        }

        public static void SetWindowPosition(IntPtr handle, int x, int y, int width, int height)
        {
            CefSharp.Core.NativeMethodWrapper.SetWindowPosition(handle, x, y, width, height);
        }

        public static void SetWindowParent(IntPtr child, IntPtr newParent)
        {
            CefSharp.Core.NativeMethodWrapper.SetWindowParent(child, newParent);
        }

        public static void RemoveExNoActivateStyle(IntPtr browserHwnd)
        {
            CefSharp.Core.NativeMethodWrapper.RemoveExNoActivateStyle(browserHwnd);
        }
    }

    /// <inheritdoc/>
    public class PostData : CefSharp.Core.PostData
    {

    }

    /// <inheritdoc/>
    public class PostDataElement : CefSharp.Core.PostDataElement
    {

    }

    /// <inheritdoc/>
    public class Request : CefSharp.Core.Request
    {

    }

    /// <inheritdoc/>
    public class RequestContext : CefSharp.Core.RequestContext
    {
        /// <inheritdoc/>
        public RequestContext() : base()
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContext otherRequestContext) : base(otherRequestContext)
        {

        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContext otherRequestContext, CefSharp.IRequestContextHandler requestContextHandler) : base(otherRequestContext, requestContextHandler)
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContextHandler requestContextHandler) : base(requestContextHandler)
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.RequestContextSettings settings) : base(settings.settings)
        {

        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.RequestContextSettings settings, CefSharp.IRequestContextHandler requestContextHandler) : base(settings.settings, requestContextHandler)
        {
        }

        /// <summary>
        /// Creates a new RequestContextBuilder which can be used to fluently set
        /// preferences
        /// </summary>
        /// <returns>Returns a new RequestContextBuilder</returns>
        public static RequestContextBuilder Configure()
        {
            var builder = new RequestContextBuilder();

            return builder;
        }
    }

    /// <summary>
    /// Fluent style builder for creating IRequestContext instances.
    /// </summary>
    public class RequestContextBuilder
    {
        private RequestContextSettings _settings;
        private IRequestContext _otherContext;
        private RequestContextHandler _handler;

        void ThrowExceptionIfContextAlreadySet()
        {
            if (_otherContext != null)
            {
                throw new Exception("A call to WithSharedSettings has already been made, it is no possible to provide custom settings.");
            }
        }

        void ThrowExceptionIfCustomSettingSpecified()
        {
            if (_settings != null)
            {
                throw new Exception("A call to WithCachePath/PersistUserPreferences has already been made, it's not possible to share settings with another RequestContext.");
            }
        }
        /// <summary>
        /// Create the actual RequestContext instance
        /// </summary>
        /// <returns>Returns a new RequestContext instance.</returns>
        public IRequestContext Create()
        {
            if (_otherContext != null)
            {
                return new CefSharp.Core.RequestContext(_otherContext, _handler);
            }

            if (_settings != null)
            {
                return new CefSharp.Core.RequestContext(_settings.settings, _handler);
            }

            return new CefSharp.Core.RequestContext(_handler);
        }

        /// <summary>
        /// Action is called in IRequestContextHandler.OnRequestContextInitialized
        /// </summary>
        /// <param name="action">called when the context has been initialized.</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder OnInitialize(Action<IRequestContext> action)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.OnInitialize(action);

            return this;
        }

        /// <summary>
        /// Sets the Cache Path
        /// </summary>
        /// <param name="cachePath">
        /// The location where cache data for this request context will be stored on
        /// disk. If this value is non-empty then it must be an absolute path that is
        /// either equal to or a child directory of CefSettings.RootCachePath.
        /// If the value is empty then browsers will be created in "incognito mode"
        /// where in-memory caches are used for storage and no data is persisted to disk.
        /// HTML5 databases such as localStorage will only persist across sessions if a
        /// cache path is specified. To share the global browser cache and related
        /// configuration set this value to match the CefSettings.CachePath value.
        /// </param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithCachePath(string cachePath)
        {
            ThrowExceptionIfContextAlreadySet();

            if (_settings == null)
            {
                _settings = new RequestContextSettings();
            }

            _settings.CachePath = cachePath;

            return this;
        }

        /// <summary>
        /// Invoke this method tp persist user preferences as a JSON file in the cache path directory.
        /// Can be set globally using the CefSettings.PersistUserPreferences value.
        /// This value will be ignored if CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder PersistUserPreferences()
        {
            ThrowExceptionIfContextAlreadySet();

            if (_settings == null)
            {
                _settings = new RequestContextSettings();
            }

            _settings.PersistUserPreferences = true;

            return this;
        }

        /// <summary>
        /// Set the value associated with preference name when the RequestContext
        /// is initialzied. If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Preferences set via the command-line usually cannot be modified.
        /// </summary>
        /// <param name="name">preference key</param>
        /// <param name="value">preference value</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithPreference(string name, object value)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetPreferenceOnContextInitialized(name, value);

            return this;
        }

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="host">proxy host</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithProxyServer(string host)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetProxyOnContextInitialized(host, null);

            return this;
        }

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port (optional)</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithProxyServer(string host, int? port)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetProxyOnContextInitialized(host, port);

            return this;
        }

        /// <summary>
        /// Set the Proxy server when the RequestContext is initialzied.
        /// If value is null the preference will be restored to its
        /// default value. If setting the preference fails no error is throw, you
        /// must check the CEF Log file.
        /// Proxy set via the command-line cannot be modified.
        /// </summary>
        /// <param name="scheme">proxy scheme</param>
        /// <param name="host">proxy host</param>
        /// <param name="port">proxy port (optional)</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithProxyServer(string scheme, string host, int? port)
        {
            if (_handler == null)
            {
                _handler = new RequestContextHandler();
            }

            _handler.SetProxyOnContextInitialized(scheme, host, port);

            return this;
        }

        /// <summary>
        /// Shares storage with other RequestContext
        /// </summary>
        /// <param name="other">shares storage with this RequestContext</param>
        /// <returns>Returns RequestContextBuilder instance</returns>
        public RequestContextBuilder WithSharedSettings(IRequestContext other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ThrowExceptionIfCustomSettingSpecified();

            _otherContext = other;

            return this;
        }
    }

    /// <inheritdoc/>
    public class RequestContextSettings
    {
        internal Core.RequestContextSettings settings = new Core.RequestContextSettings();

        /// <summary>
        /// To persist session cookies (cookies without an expiry date or validity
        /// interval) by default when using the global cookie manager set this value to
        /// true. Session cookies are generally intended to be transient and most
        /// Web browsers do not persist them. Can be set globally using the
        /// CefSettings.PersistSessionCookies value. This value will be ignored if
        /// CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        public bool PersistSessionCookies
        {
            get { return settings.PersistSessionCookies; }
            set { settings.PersistSessionCookies = value; }
        }

        /// <summary>
        /// To persist user preferences as a JSON file in the cache path directory set
        /// this value to true. Can be set globally using the
        /// CefSettings.PersistUserPreferences value. This value will be ignored if
        /// CachePath is empty or if it matches the CefSettings.CachePath value.
        /// </summary>
        public bool PersistUserPreferences
        {
            get { return settings.PersistUserPreferences; }
            set { settings.PersistUserPreferences = value; }
        }

        /// <summary>
        /// The location where cache data for this request context will be stored on
        /// disk. If this value is non-empty then it must be an absolute path that is
        /// either equal to or a child directory of CefSettings.RootCachePath.
        /// If the value is empty then browsers will be created in "incognito mode"
        /// where in-memory caches are used for storage and no data is persisted to disk.
        /// HTML5 databases such as localStorage will only persist across sessions if a
        /// cache path is specified. To share the global browser cache and related
        /// configuration set this value to match the CefSettings.CachePath value.
        /// </summary>
        public String CachePath
        {
            get { return settings.CachePath; }
            set { settings.CachePath = value; }
        }

        /// <summary>
        /// Comma delimited ordered list of language codes without any whitespace that
        /// will be used in the "Accept-Language" HTTP header. Can be set globally
        /// using the CefSettings.accept_language_list value or overridden on a per-
        /// browser basis using the BrowserSettings.AcceptLanguageList value. If
        /// all values are empty then "en-US,en" will be used. This value will be
        /// ignored if CachePath matches the CefSettings.CachePath value.
        /// </summary>
        public String AcceptLanguageList
        {
            get { return settings.AcceptLanguageList; }
            set { settings.AcceptLanguageList = value; }
        }

        /// <summary>
        /// Set to true to ignore errors related to invalid SSL certificates.
        /// Enabling this setting can lead to potential security vulnerabilities like
        /// "man in the middle" attacks. Applications that load content from the
        /// internet should not enable this setting. Can be set globally using the
        /// CefSettings.IgnoreCertificateErrors value. This value will be ignored if
        /// CachePath matches the CefSettings.cache_path value.
        /// </summary>
        public bool IgnoreCertificateErrors
        {
            get { return settings.IgnoreCertificateErrors; }
            set { settings.IgnoreCertificateErrors = value; }
        }
    }

    /// <inheritdoc/>
    public class UrlRequest : CefSharp.Core.UrlRequest
    {
        public UrlRequest(IRequest request, IUrlRequestClient urlRequestClient) : base(request, urlRequestClient)
        {
        }

        public UrlRequest(IRequest request, IUrlRequestClient urlRequestClient, IRequestContext requestContext) : base(request, urlRequestClient, requestContext)
        {
        }
    }

    /// <inheritdoc/>
    public class WindowInfo : CefSharp.Core.WindowInfo
    {

    }

    public static class DragData
    {
        public static IDragData Create()
        {
            return Core.DragData.Create();
        }
    }
}
