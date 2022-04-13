// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Reflection;
using System.Runtime.CompilerServices;

namespace CefSharp
{
    /// <summary>
    /// When targeting NETFRAMEWORK this Module Initializer
    /// will call <see cref="CefRuntime.SubscribeAnyCpuAssemblyResolver(string)"/>
    /// when the <see cref="Assembly.GetEntryAssembly"/> has a <see cref="ProcessorArchitecture.MSIL"/>.
    /// </summary>
    public static class CefRuntimeAnyCpuInitializer
    {
        /// <summary>
        /// True if the AnyCpu resolver was attached via this Module Initializer
        /// </summary>
        public static bool AssemblyResolverAttached { get; private set; }

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            var executingAssembly = Assembly.GetEntryAssembly();
            //The GetEntryAssembly method can return null when a managed assembly has been loaded from an unmanaged application.
            //https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getentryassembly
            if (executingAssembly != null && CefRuntime.SubscribeToAnyCpuResolverInModuleInitializer)
            {
                var name = executingAssembly.GetName();
                if (name.ProcessorArchitecture == ProcessorArchitecture.MSIL)
                {
                    if (!CefRuntime.IsAnyCpuAssemblyResolverAttached)
                    {
                        AssemblyResolverAttached = true;

                        CefRuntime.SubscribeAnyCpuAssemblyResolver();
                    }
                }
            }
        }
    }
}
