// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;
using System.Runtime.CompilerServices;

namespace CefSharp.WinForms
{
    public static class Initializer
    {
        public static string InitValue = "";

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            InitValue = "CefSharp WinForms Loaded!!!";
            var browserSubprocessDllPath = Path.Combine(Path.GetDirectoryName(typeof(Initializer).Assembly.Location), "CefSharp.Core.Runtime.dll");
            System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(browserSubprocessDllPath);
        }
    }
}
