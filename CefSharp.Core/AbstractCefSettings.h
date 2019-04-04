// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Collections::Generic;
using namespace System::IO;

namespace CefSharp
{
    /// <summary>
    /// Initialization settings. Many of these and other settings can also configured
    /// using command-line switches.
    /// </summary>
    public ref class AbstractCefSettings abstract
    {
    private:
        List<V8Extension^>^ _cefExtensions;
        IDictionary<String^, String^>^ _cefCommandLineArgs;

    internal:
        ::CefSettings* _cefSettings;
        List<CefCustomScheme^>^ _cefCustomSchemes;

    public:
        /// <summary>
        /// Default Constructor
        /// </summary>
        AbstractCefSettings() : _cefSettings(new ::CefSettings())
        {
            _cefSettings->multi_threaded_message_loop = true;
            _cefSettings->no_sandbox = true;
            BrowserSubprocessPath = Path::Combine(Path::GetDirectoryName(this->GetType()->Assembly->Location), "CefSharp.BrowserSubprocess.exe");
            _cefCustomSchemes = gcnew List<CefCustomScheme^>();
            _cefExtensions = gcnew List<V8Extension^>();
            _cefCommandLineArgs = gcnew Dictionary<String^, String^>();

            //Automatically discovered and load a system-wide installation of Pepper Flash.
            _cefCommandLineArgs->Add("enable-system-flash", "1");

            //CEF has switched to the new process model defined that was implemented
            //in the Chromium Site isolation project, we'll continue to use the older
            //process model by default.
            //https://github.com/cefsharp/CefSharp/issues/2553
            _cefCommandLineArgs->Add("process-per-site-instance", "1");
        }

        !AbstractCefSettings()
        {
            delete _cefSettings;
        }

        ~AbstractCefSettings()
        {
            this->!AbstractCefSettings();
        }

        /// <summary>
        /// Add Customs schemes to this collection
        /// </summary>
        property IEnumerable<CefCustomScheme^>^ CefCustomSchemes
        {
            IEnumerable<CefCustomScheme^>^ get() { return _cefCustomSchemes; }
        }

        /// <summary>
        /// List of all V8Extensions to be registered using CefRegisterExtension
        /// in the render process.
        /// </summary>
        virtual property IEnumerable<V8Extension^>^ Extensions
        {
            IEnumerable<V8Extension^>^ get() { return _cefExtensions; }
        }

        /// <summary>
        /// Add custom command line argumens to this collection, they will be
        /// added in OnBeforeCommandLineProcessing.
        // The CefSettings.command_line_args_disabled value can be used to start with an empty command-line object. Any values specified in CefSettings that equate to command-line arguments will be set before this method is called.
        /// </summary>
        virtual property IDictionary<String^, String^>^ CefCommandLineArgs
        {
            IDictionary<String^, String^>^ get() { return _cefCommandLineArgs; }
        }

        /// <summary>
        /// Set to true to disable configuration of browser process features using
        /// standard CEF and Chromium command-line arguments. Configuration can still
        /// be specified using CEF data structures or by adding to CefCommandLineArgs
        /// </summary>
        property bool CommandLineArgsDisabled
        {
            bool get() { return _cefSettings->command_line_args_disabled == 1; }
            void set(bool value) { _cefSettings->command_line_args_disabled = value; }
        }

        /// <summary>
        /// Set to true to enable date-based expiration of built in network security information (i.e. certificate transparency logs,
        /// HSTS preloading and pinning information). Enabling this option improves network security but may cause HTTPS load failures when
        /// using CEF binaries built more than 10 weeks in the past. See https://www.certificate-transparency.org/ and
        /// https://www.chromium.org/hsts for details. Can be set globally using the CefSettings.EnableNetSecurityExpiration value.
        /// </summary>
        property bool EnableNetSecurityExpiration
        {
            bool get() { return _cefSettings->enable_net_security_expiration == 1; }
            void set(bool value) { _cefSettings->enable_net_security_expiration = value; }
        }

        /// <summary>
        /// Set to true to control browser process main (UI) thread message pump
        /// scheduling via the IBrowserProcessHandler.OnScheduleMessagePumpWork
        /// callback. This option is recommended for use in combination with the
        /// Cef.DoMessageLoopWork() function in cases where the CEF message loop must be
        /// integrated into an existing application message loop (see additional
        /// comments and warnings on Cef.DoMessageLoopWork). Enabling this option is not
        /// recommended for most users; leave this option disabled and use either
        /// MultiThreadedMessageLoop (the default) if possible.
        /// </summary>
        property bool ExternalMessagePump
        {
            bool get() { return _cefSettings->external_message_pump == 1; }
            void set(bool value) { _cefSettings->external_message_pump = value; }
        }

        /// <summary>
        /// Set to true to have the browser process message loop run in a separate
        /// thread. If false than the CefDoMessageLoopWork() function must be
        /// called from your application message loop. This option is only supported on
        /// Windows. The default value is true
        /// </summary>
        property bool MultiThreadedMessageLoop
        {
            bool get() { return _cefSettings->multi_threaded_message_loop == 1; }
            void set(bool value) { _cefSettings->multi_threaded_message_loop = value; }
        }

        /// <summary>
        /// The path to a separate executable that will be launched for sub-processes.
        /// By default the browser process executable is used. See the comments on
        /// Cef.ExecuteProcess() for details. Also configurable using the
        /// "browser-subprocess-path" command-line switch. Default is CefSharp.BrowserSubprocess.exe
        /// </summary>
        property String^ BrowserSubprocessPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->browser_subprocess_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->browser_subprocess_path, value); }
        }

        /// <summary>
        /// The location where cache data will be stored on disk. If empty then
        /// browsers will be created in "incognito mode" where in-memory caches are
        /// used for storage and no data is persisted to disk. HTML5 databases such as
        /// localStorage will only persist across sessions if a cache path is
        /// specified. Can be overridden for individual CefRequestContext instances via
        /// the RequestContextSettings.CachePath value.
        /// </summary>
        property String^ CachePath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->cache_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->cache_path, value); }
        }

        /// <summary>
        /// The location where user data such as spell checking dictionary files will
        /// be stored on disk. If empty then the default platform-specific user data
        /// directory will be used ("~/.cef_user_data" directory on Linux,
        /// "~/Library/Application Support/CEF/User Data" directory on Mac OS X,
        /// "Local Settings\Application Data\CEF\User Data" directory under the user
        /// profile directory on Windows).
        /// </summary>
        property String^ UserDataPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->user_data_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->user_data_path, value); }
        }

        /// <summary>
        /// Set to true in order to completely ignore SSL certificate errors.
        /// This is NOT recommended.
        /// </summary>
        property bool IgnoreCertificateErrors
        {
            bool get() { return _cefSettings->ignore_certificate_errors == 1; }
            void set(bool value) { _cefSettings->ignore_certificate_errors = value; }
        }

        /// <summary>
        /// The locale string that will be passed to WebKit. If empty the default
        /// locale of "en-US" will be used. Also configurable using the "lang"
        /// command-line switch.
        /// </summary>
        property String^ Locale
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->locale); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->locale, value); }
        }

        /// <summary>
        /// The fully qualified path for the locales directory. If this value is empty
        /// the locales directory must be located in the module directory.
        /// Also configurable using the "locales-dir-path" command-line switch.
        /// </summary>
        property String^ LocalesDirPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->locales_dir_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->locales_dir_path, value); }
        }

        /// <summary>
        /// The fully qualified path for the resources directory. If this value is
        /// empty the cef.pak and/or devtools_resources.pak files must be located in
        /// the module directory. Also configurable using the "resources-dir-path" command-line
        /// switch.
        /// </summary>
        property String^ ResourcesDirPath
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->resources_dir_path); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->resources_dir_path, value); }
        }

        /// <summary>
        /// The directory and file name to use for the debug log. If empty a default
        /// log file name and location will be used. On Windows and Linux a "debug.log"
        /// file will be written in the main executable directory.
        /// Also configurable using the"log-file" command-line switch.
        /// </summary>
        property String^ LogFile
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->log_file); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->log_file, value); }
        }

        /// <summary>
        /// The log severity. Only messages of this severity level or higher will be
        /// logged. When set to <see cref="CefSharp.LogSeverity.Disable"/> no messages will be written to the log file,
        /// but Fatal messages will still be output to stderr. Also configurable using the "log-severity" command-line switch with
        /// a value of "verbose", "info", "warning", "error", "fatal", "error-report" or "disable".
        /// </summary>
        property CefSharp::LogSeverity LogSeverity
        {
            CefSharp::LogSeverity get() { return (CefSharp::LogSeverity)_cefSettings->log_severity; }
            void set(CefSharp::LogSeverity value) { _cefSettings->log_severity = (cef_log_severity_t)value; }
        }

        /// <summary>
        /// Custom flags that will be used when initializing the V8 JavaScript engine.
        /// The consequences of using custom flags may not be well tested. Also
        /// configurable using the "js-flags" command-line switch.
        /// </summary>
        property String^ JavascriptFlags
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->javascript_flags); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->javascript_flags, value); }
        }

        /// <summary>
        /// Set to true to disable loading of pack files for resources and locales.
        /// A resource bundle handler must be provided for the browser and render
        /// processes via CefApp::GetResourceBundleHandler() if loading of pack files
        /// is disabled. Also configurable using the "disable-pack-loading" command-
        /// line switch.
        /// </summary>
        property bool PackLoadingDisabled
        {
            bool get() { return _cefSettings->pack_loading_disabled == 1; }
            void set(bool value) { _cefSettings->pack_loading_disabled = value; }
        }

        /// <summary>
        /// Value that will be inserted as the product portion of the default
        /// User-Agent string. If empty the Chromium product version will be used. If
        /// |userAgent| is specified this value will be ignored. Also configurable
        /// using the "product-version" command-line switch.
        /// </summary>
        property String^ ProductVersion
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->product_version); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->product_version, value); }
        }

        /// <summary>
        /// Set to a value between 1024 and 65535 to enable remote debugging on the
        /// specified port. For example, if 8080 is specified the remote debugging URL
        /// will be http://localhost:8080. CEF can be remotely debugged from any CEF or
        /// Chrome browser window. Also configurable using the "remote-debugging-port"
        /// command-line switch.
        /// </summary>
        property int RemoteDebuggingPort
        {
            int get() { return _cefSettings->remote_debugging_port; }
            void set(int value) { _cefSettings->remote_debugging_port = value; }
        }

        /// <summary>
        /// The number of stack trace frames to capture for uncaught exceptions.
        /// Specify a positive value to enable the CefRenderProcessHandler::
        /// OnUncaughtException() callback. Specify 0 (default value) and
        /// OnUncaughtException() will not be called. Also configurable using the
        /// "uncaught-exception-stack-size" command-line switch.
        /// </summary>
        property int UncaughtExceptionStackSize
        {
            int get() { return _cefSettings->uncaught_exception_stack_size; }
            void set(int value) { _cefSettings->uncaught_exception_stack_size = value; }
        }

        /// <summary>
        /// Value that will be returned as the User-Agent HTTP header. If empty the
        /// default User-Agent string will be used. Also configurable using the
        /// "user-agent" command-line switch.
        /// </summary>
        property String^ UserAgent
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->user_agent); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->user_agent, value); }
        }

        /// <summary>
        /// Set to true (1) to enable windowless (off-screen) rendering support. Do not
        /// enable this value if the application does not use windowless rendering as
        /// it may reduce rendering performance on some systems.
        /// </summary>
        property bool WindowlessRenderingEnabled
        {
            bool get() { return _cefSettings->windowless_rendering_enabled == 1; }
            void set(bool value) { _cefSettings->windowless_rendering_enabled = value; }
        }

        /// <summary>
        /// To persist session cookies (cookies without an expiry date or validity
        /// interval) by default when using the global cookie manager set this value to
        /// true. Session cookies are generally intended to be transient and most
        /// Web browsers do not persist them. A CachePath value must also be
        /// specified to enable this feature. Also configurable using the
        /// "persist-session-cookies" command-line switch. Can be overridden for
        /// individual RequestContext instances via the
        /// RequestContextSettings.PersistSessionCookies value.
        /// </summary>
        property bool PersistSessionCookies
        {
            bool get() { return _cefSettings->persist_session_cookies == 1; }
            void set(bool value) { _cefSettings->persist_session_cookies = value; }
        }

        /// <summary>
        /// To persist user preferences as a JSON file in the cache path directory set
        /// this value to true. A CachePath value must also be specified
        /// to enable this feature. Also configurable using the
        /// "persist-user-preferences" command-line switch. Can be overridden for
        /// individual RequestContext instances via the
        /// RequestContextSettings.PersistUserPreferences value.
        /// </summary>
        property bool PersistUserPreferences
        {
            bool get() { return _cefSettings->persist_user_preferences == 1; }
            void set(bool value) { _cefSettings->persist_user_preferences = value; }
        }

        /// <summary>
        /// Comma delimited ordered list of language codes without any whitespace that
        /// will be used in the "Accept-Language" HTTP header. May be set globally
        /// using the CefSettings.AcceptLanguageList value. If both values are
        /// empty then "en-US,en" will be used.
        /// </summary>
        property String^ AcceptLanguageList
        {
            String^ get() { return StringUtils::ToClr(_cefSettings->accept_language_list); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_cefSettings->accept_language_list, value); }
        }

        /// <summary>
        /// Background color used for the browser before a document is loaded and when no document color is
        /// specified. The alpha component must be either fully opaque (0xFF) or fully transparent (0x00).
        /// If the alpha component is fully opaque then the RGB components will be used as the background
        /// color. If the alpha component is fully transparent for a WinForms browser then the default value
        /// of opaque white be used. If the alpha component is fully transparent for a windowless
        /// (WPF/OffScreen) browser then transparent painting will be enabled.
        /// </summary>
        virtual property uint32 BackgroundColor
        {
            uint32 get() { return _cefSettings->background_color; }
            void set(uint32 value) { _cefSettings->background_color = value; }
        }

        /// <summary>
        /// Registers a custom scheme using the provided settings.
        /// </summary>
        /// <param name="cefCustomScheme">The CefCustomScheme which provides the details about the scheme.</param>
        void RegisterScheme(CefCustomScheme^ cefCustomScheme)
        {
            //Scheme names are converted to lowercase
            cefCustomScheme->SchemeName = cefCustomScheme->SchemeName->ToLower();

            _cefCustomSchemes->Add(cefCustomScheme);
        }

        /// <summary>
        /// Register a new V8 extension with the specified JavaScript extension code
        /// </summary>
        /// <param name="extension">The V8Extension that contains the extension code.</param>
        void RegisterExtension(V8Extension^ extension)
        {
            if (_cefExtensions->Contains(extension))
            {
                throw gcnew ArgumentException("An extension with the same name is already registered.", "extension");
            }
            _cefExtensions->Add(extension);
        }

        /// <summary>
        /// Set command line argument to disable GPU Acceleration, this will disable WebGL.
        /// </summary>
        void DisableGpuAcceleration()
        {
            if (!_cefCommandLineArgs->ContainsKey("disable-gpu"))
            {
                _cefCommandLineArgs->Add("disable-gpu", "1");
            }
        }

        /// <summary>
        /// Set command line arguments for best OSR (Offscreen and WPF) Rendering performance
        /// This will disable WebGL, look at the source to determine which flags best suite
        /// your requirements.
        /// </summary>
        void SetOffScreenRenderingBestPerformanceArgs()
        {
            // Use software rendering and compositing (disable GPU) for increased FPS
            // and decreased CPU usage. This will also disable WebGL so remove these
            // switches if you need that capability.
            // See https://bitbucket.org/chromiumembedded/cef/issues/1257 for details.
            if (!_cefCommandLineArgs->ContainsKey("disable-gpu"))
            {
                _cefCommandLineArgs->Add("disable-gpu", "1");
            }

            if (!_cefCommandLineArgs->ContainsKey("disable-gpu-compositing"))
            {
                _cefCommandLineArgs->Add("disable-gpu-compositing", "1");
            }

            // Synchronize the frame rate between all processes. This results in
            // decreased CPU usage by avoiding the generation of extra frames that
            // would otherwise be discarded. The frame rate can be set at browser
            // creation time via CefBrowserSettings.windowless_frame_rate or changed
            // dynamically using CefBrowserHost::SetWindowlessFrameRate. In cefclient
            // it can be set via the command-line using `--off-screen-frame-rate=XX`.
            // See https://bitbucket.org/chromiumembedded/cef/issues/1368 for details.
            if (!_cefCommandLineArgs->ContainsKey("enable-begin-frame-scheduling"))
            {
                _cefCommandLineArgs->Add("enable-begin-frame-scheduling", "1");
            }
        }
    };
}
