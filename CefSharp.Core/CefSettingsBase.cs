// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using CefSharp.Internals;
using System;
using System.Collections.Generic;

namespace CefSharp
{
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
            if(!System.IO.File.Exists(BrowserSubprocessPath))
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
        /// **Experimental**
        /// Set to true to enable use of the Chrome runtime in CEF. This feature is
        /// considered experimental and is not recommended for most users at this time.
        /// See issue https://bitbucket.org/chromiumembedded/cef/issues/2969/support-chrome-windows-with-cef-callbacks for details.
        /// </summary>
        public bool ChromeRuntime
        {
            get { return settings.ChromeRuntime; }
            set { settings.ChromeRuntime = value; }
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
        /// will be used. If UserAgent is specified this value will be ignored. Also configurable using the "user-agent-product" command-
        /// line switch.
        /// </summary>
        public string UserAgentProduct
        {
            get { return settings.UserAgentProduct; }
            set { settings.UserAgentProduct = value; }
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
        /// Comma delimited list of schemes supported by the associated
        /// ICookieManager. If CookieableSchemesExcludeDefaults is false the
        /// default schemes ("http", "https", "ws" and "wss") will also be supported.
        /// Specifying a CookieableSchemesList value and setting
        /// CookieableSchemesExcludeDefaults to true will disable all loading
        /// and saving of cookies for this manager. Can be overridden
        /// for individual RequestContext instances via the
        /// RequestContextSettings.CookieableSchemesList and
        /// RequestContextSettings.CookieableSchemesExcludeDefaults values.
        /// </summary>
        public string CookieableSchemesList
        {
            get { return settings.CookieableSchemesList; }
            set { settings.CookieableSchemesList = value; }
        }

        /// <summary>
        /// If CookieableSchemesExcludeDefaults is false the
        /// default schemes ("http", "https", "ws" and "wss") will also be supported.
        /// Specifying a CookieableSchemesList value and setting
        /// CookieableSchemesExcludeDefaults to true will disable all loading
        /// and saving of cookies for this manager. Can be overridden
        /// for individual RequestContext instances via the
        /// RequestContextSettings.CookieableSchemesList and
        /// RequestContextSettings.CookieableSchemesExcludeDefaults values.
        /// </summary>
        public bool CookieableSchemesExcludeDefaults
        {
            get { return settings.CookieableSchemesExcludeDefaults; }
            set { settings.CookieableSchemesExcludeDefaults = value; }
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
}
