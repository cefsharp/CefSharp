// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Reflection;

namespace CefSharp
{
    /// <summary>
    /// Used for AnyCPU Support to resolve Assemblies and Dependencies
    /// </summary>
    public static class CefLoader
    {
        /// <summary>
        /// RegisterCefSharpAssemblyResolver
        /// </summary>
        public static void RegisterCefSharpAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
        }
        
        /// <summary>
        /// UnRegisterCefSharpAssemblyResolver
        /// </summary>
        public static void UnRegisterCefSharpAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= Resolver;
        }

        /// <summary>
        /// Helper method to map the CefSharp.BrowserSubProcess.exe file
        /// based on current process bitness (x64 or x86)
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserSubProcessPath()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");

            if(File.Exists(path))
            {
                throw new FileNotFoundException("Unable to locate", path);
            }

            return path;
        }

        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath) ? Assembly.LoadFile(archSpecificPath) : null;
            }

            return null;
        }
    }
}
