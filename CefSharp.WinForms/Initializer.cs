// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.CompilerServices;

namespace CefSharp.WinForms
{
    public static class Initializer
    {
        public static string CefSharpCoreRuntimeLocation = "";

        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            var assembly = CefSharp.Initializer.LoadCefSharpCoreRuntime();

            CefSharpCoreRuntimeLocation = assembly.Location;
        }
    }
}
