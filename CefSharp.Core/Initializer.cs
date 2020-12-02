// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CefSharp
{
    /// <summary>
    /// CLR Module Initializer
    /// Used to load libcef.dll if required
    /// </summary>
    public static class Initializer
    {
        //TODO: Internal debugging only for now, needs improving if users are going to
        //get meaningful data from this.
        public static (bool Loaded, string Path, string BrowserSubProcessPath, IntPtr LibraryHandle) LibCefLoadStatus;

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            var currentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var libCefPath = Path.Combine(currentFolder, "libcef.dll");

            if (File.Exists(libCefPath))
            {
                //We didn't load CEF, it was already next to our calling assembly, the
                //framework should load it correctly on it's own
                LibCefLoadStatus = (false, libCefPath, null, IntPtr.Zero);
            }
            else
            { 
                //TODO: This will need changing if we support ARM64
                var arch = Environment.Is64BitProcess ? "x64" : "x86";
                var archFolder = $"runtimes\\win-{arch}\\native";
                libCefPath = Path.Combine(currentFolder, archFolder, "libcef.dll");
                if (File.Exists(libCefPath))
                {
                    if (NativeLibrary.TryLoad(libCefPath, out IntPtr handle))
                    {
                        var browserSubProcessPath = Path.Combine(currentFolder, archFolder, "CefSharp.BrowserSubprocess.exe");
                        LibCefLoadStatus = (true, libCefPath, browserSubProcessPath, handle);
                    }
                }
                else
                {
                    LibCefLoadStatus = (false, libCefPath, null, IntPtr.Zero);
                }
            }
            //var assembly = LoadCefSharpCoreRuntime();

            //NativeLibrary.SetDllImportResolver(typeof(CefSharp.Core.CefSettingsBase).Assembly, LibCefImportResolver);

            //CefSharpCoreRuntimeLocation = assembly.Location;
        }

        //private static IntPtr LibCefImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        //{
        //    return IntPtr.Zero;
        //}

        //public static Assembly LoadCefSharpCoreRuntime()
        //{
        //    //Load into the same context as CefSharp.Core, if user was to create their own context then
        //    //this should keep thing together.
        //    var currentCtx = AssemblyLoadContext.GetLoadContext(typeof(Initializer).Assembly);

        //    var browserSubprocessDllPath = Path.Combine(Path.GetDirectoryName(typeof(Initializer).Assembly.Location), "CefSharp.Core.Runtime.dll");
        //    return currentCtx.LoadFromAssemblyPath(browserSubprocessDllPath);
        //}
    }
}
