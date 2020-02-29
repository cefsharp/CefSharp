// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Reflection;

namespace CefSharp.Test
{
    /// <summary>
    /// CefSharp requires a default AppDomain which means that xunit is not able
    /// to provide the correct binding redirects defined in the app.config
    /// so we have to provide them manually via <see cref="AppDomain.CurrentDomain.AssemblyResolve"/>
    /// </summary>
    internal class BindingRedirectAssemblyResolver : IDisposable
    {
        internal BindingRedirectAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;

        }
        void IDisposable.Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolve;
        }

        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var asemblyName = new AssemblyName(args.Name);
            var path = Path.Combine(Environment.CurrentDirectory, asemblyName.Name + ".dll");

            if (File.Exists(path))
            {
                return Assembly.LoadFrom(path);
            }

            return null;
        }
    }
}
