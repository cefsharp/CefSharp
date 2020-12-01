// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace CefSharp
{
    public static class Initializer
    {
        public static string CefSharpCoreRuntimeLocation = "";

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            var assembly = LoadCefSharpCoreRuntime();

            CefSharpCoreRuntimeLocation = assembly.Location;
        }

        public static Assembly LoadCefSharpCoreRuntime()
        {
            //Load into the same context as CefSharp.Core, if user was to create their own context then
            //this should keep thing together.
            var currentCtx = AssemblyLoadContext.GetLoadContext(typeof(Initializer).Assembly);

            var browserSubprocessDllPath = Path.Combine(Path.GetDirectoryName(typeof(Initializer).Assembly.Location), "CefSharp.Core.Runtime.dll");
            return currentCtx.LoadFromAssemblyPath(browserSubprocessDllPath);
        }
    }
}
