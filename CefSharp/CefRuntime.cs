// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Reflection;

namespace CefSharp
{
    /// <summary>
    /// CefRuntime - Used to simplify loading of the CefSharp architecture specific resources.
    /// Typical use case would be when you are targeting AnyCPU
    /// </summary>
    public static class CefRuntime
    {
        private static ResolveEventHandler currentDomainAssemblyResolveHandler;

        /// <summary>
        /// When using AnyCPU the architecture specific version of CefSharp.Core.Runtime.dll
        /// needs to be loaded (x64/x86).
        /// This method subscribes to the <see cref="AppDomain.AssemblyResolve"/> event
        /// for <see cref="AppDomain.CurrentDomain"/> and loads the CefSharp.Core.Runtime.dll
        /// based on <see cref="Environment.Is64BitProcess"/>.
        /// This method MUST be called before you call Cef.Initialize, create your first ChromiumWebBrowser instance, basically
        /// before anything CefSharp related happens. This method is part of CefSharp.dll which is an AnyCPU library and
        /// doesn't have any references to the CefSharp.Core.Runtime.dll so it's safe to use.
        /// </summary>
        /// <param name="basePath">
        /// The path containing the x64/x86 folders which contain the CefSharp/CEF resources.
        /// If null then AppDomain.CurrentDomain.SetupInformation.ApplicationBase will be used as the path.
        /// (</param>
        public static void SubscribeAnyCpuAssemblyResolver(string basePath = null)
        {
            if(currentDomainAssemblyResolveHandler != null)
            {
                throw new Exception("UseAnyCpuAssemblyResolver has already been called, call ");
            }

            if(basePath == null)
            {
                basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }

            currentDomainAssemblyResolveHandler = (sender, args) =>
            {
                if (args.Name.StartsWith("CefSharp.Core.Runtime"))
                {
                    string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                    string archSpecificPath = Path.Combine(basePath,
                                                           Environment.Is64BitProcess ? "x64" : "x86",
                                                           assemblyName);

                    return File.Exists(archSpecificPath)
                               ? System.Reflection.Assembly.LoadFile(archSpecificPath)
                               : null;
                }

                return null;
            };

            AppDomain.CurrentDomain.AssemblyResolve += currentDomainAssemblyResolveHandler;
        }

        /// <summary>
        /// Unsubscribe  from the <see cref="AppDomain.AssemblyResolve"/> event
        /// for <see cref="AppDomain.CurrentDomain"/> that was added in <see cref="UseAnyCpuAssemblyResolver"/>
        /// </summary>
        public static void UnsubscribeAnyCpuAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= currentDomainAssemblyResolveHandler;

            currentDomainAssemblyResolveHandler = null;
        }

        /// <summary>
        /// When using AnyCPU the architecture specific version of CefSharp.Core.Runtime.dll
        /// needs to be loaded (x64/x86).
        /// This method calls <see cref="Assembly.LoadFile(string)"/> to immediately load CefSharp.Core.Runtime.dll
        /// based on <see cref="Environment.Is64BitProcess"/>.
        /// This method MUST be called before you call Cef.Initialize, create your first ChromiumWebBrowser instance, basically
        /// before anything CefSharp related happens. This method is part of CefSharp.dll which is an AnyCPU library and
        /// doesn't have any references to the CefSharp.Core.Runtime.dll so it's safe to use.
        /// </summary>
        /// <param name="basePath">
        /// The path containing the x64/x86 folders which contain the CefSharp/CEF resources.
        /// If null then AppDomain.CurrentDomain.SetupInformation.ApplicationBase will be used as the path.
        /// (</param>
        public static void LoadCefSharpCoreRuntimeAnyCpu(string basePath = null)
        {
            const string assemblyName = "CefSharp.Core.Runtime.dll";

            if (basePath == null)
            {
                basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }

            var env = Environment.Is64BitProcess ? "x64" : "x86";
            string archSpecificPath = Path.Combine(basePath,
                                                   env,
                                                   assemblyName);

            if (File.Exists(archSpecificPath))
            {
                Assembly.LoadFile(archSpecificPath);
            }
            else
            {
                throw new FileNotFoundException("Unable to load for arch " + env, archSpecificPath);
            }
        }
    }
}
