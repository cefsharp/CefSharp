// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using CefSharp.Internals;
using System;
using System.IO;

namespace CefSharp.BrowserSubprocess
{
    /// <summary>
    /// SelfHost allows your application executable to be used as the BrowserSubProcess
    /// with minimal effort.
    /// </summary>
    /// <example>
    /// //WinForms Example
    /// public class Program
    /// {
    ///	  [STAThread]
    ///   public static int Main(string[] args)
    ///   {
    ///     Cef.EnableHighDPISupport();
    ///
    ///     var exitCode = CefSharp.BrowserSubprocess.SelfHost.Main(args);
    ///
    ///     if (exitCode >= 0)
    ///     {
    ///       return exitCode;
    ///     }
    ///
    ///     var settings = new CefSettings();
    ///     //Absolute path to your applications executable
    ///     settings.BrowserSubprocessPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
    ///
    ///     Cef.Initialize(settings);
    ///
    ///     var browser = new BrowserForm(true);
    ///     Application.Run(browser);
    ///
    ///     return 0;
    ///   }
    /// }
    /// </example>
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


#if NETCOREAPP
            var browserSubprocessDllPath = Initializer.BrowserSubProcessCorePath;
            if (!File.Exists(browserSubprocessDllPath))
            {
                browserSubprocessDllPath = Path.Combine(Path.GetDirectoryName(typeof(CefSharp.Core.BrowserSettings).Assembly.Location), "CefSharp.BrowserSubprocess.Core.dll");
            }
            var browserSubprocessDll = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(browserSubprocessDllPath);
#else
            var browserSubprocessDllPath = Path.Combine(Path.GetDirectoryName(typeof(CefSharp.Core.BrowserSettings).Assembly.Location), "CefSharp.BrowserSubprocess.Core.dll");
            var browserSubprocessDll = System.Reflection.Assembly.LoadFrom(browserSubprocessDllPath);
            
#endif
            var browserSubprocessExecutableType = browserSubprocessDll.GetType("CefSharp.BrowserSubprocess.BrowserSubprocessExecutable");
            var browserSubprocessExecutable = Activator.CreateInstance(browserSubprocessExecutableType);

            var mainMethod = browserSubprocessExecutableType.GetMethod("MainSelfHost", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            var argCount = mainMethod.GetParameters();

            var methodArgs = new object[] { args };

            var exitCode = mainMethod.Invoke(null, methodArgs);

            return (int)exitCode;
        }
    }
}
