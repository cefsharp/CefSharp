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
        internal static IntPtr? LibCefHandle { get; private set; }
        internal static bool LibCefLoaded { get; private set; }
        internal static string LibCefPath { get; private set; }
        internal static string BrowserSubProcessPath { get; private set; }
        internal static string BrowserSubProcessCorePath { get; private set; }

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            string currentFolder;

            var executingAssembly = Assembly.GetEntryAssembly();
            //The GetEntryAssembly method can return null when a managed assembly has been loaded from an unmanaged application.
            //https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getentryassembly?view=net-5.0
            if (executingAssembly == null)
            {
                currentFolder = Path.GetDirectoryName(typeof(Initializer).Assembly.Location);
            }
            else
            {
                currentFolder = Path.GetDirectoryName(executingAssembly.Location);
            }

            //In .NET 5.0 and later versions, for bundled assemblies, Assembly.Location is an empty string.
            //https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.location?view=net-5.0
            //Which results in Path.GetDirectoryName returning null, in that case use use AppContext.BaseDirectory
            //otherwise Path.Combine will throw an exception.
            //In .NET 5.0 and later versions, for bundled assemblies, AppContext.BaseDirectory returns the containing directory of the host executable.
            //https://docs.microsoft.com/en-us/dotnet/api/system.appcontext.basedirectory?view=net-5.0
            var libCefPath = Path.Combine(currentFolder ?? AppContext.BaseDirectory, "libcef.dll");

            if (File.Exists(libCefPath))
            {
                //We didn't load CEF, it was already next to our calling assembly, the
                //framework should load it correctly on it's own
                LibCefPath = libCefPath;
                LibCefLoaded = false;
            }
            else
            { 
                //TODO: This will need changing if we support ARM64
                var arch = Environment.Is64BitProcess ? "x64" : "x86";
                var archFolder = $"runtimes\\win-{arch}\\native";
                libCefPath = Path.Combine(currentFolder, archFolder, "libcef.dll");
                if (File.Exists(libCefPath))
                {
                    LibCefLoaded = NativeLibrary.TryLoad(libCefPath, out IntPtr handle);

                    if (LibCefLoaded)
                    {
                        BrowserSubProcessPath = Path.Combine(currentFolder, archFolder, "CefSharp.BrowserSubprocess.exe");
                        BrowserSubProcessCorePath = Path.Combine(currentFolder, archFolder, "CefSharp.BrowserSubprocess.Core.dll");
                        
                        LibCefPath = libCefPath;
                        LibCefHandle = handle;
                    }
                }
                else
                {
                    LibCefPath = libCefPath;
                    LibCefLoaded = false;
                }
            }
        }
    }
}
