// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CefSharp
{
    /// <summary>
    /// TODO: Expand to support optional dependencies
    /// </summary>
    public static class DependencyChecker
    {
        private const string LocalPackPath = @"locales\en-US.pak";

        /// <summary>
        /// List of Cef Dependencies - currently contains all possabilties
        /// </summary>
        public static string[] CefDependencies =
        {
            "libcef.dll",
            "libEGL.dll",
            "libGLESv2.dll",
            "pdf.dll",
            "icudtl.dat",
            "ffmpegsumo.dll",
            "d3dcompiler_43.dll",
            "d3dcompiler_47.dll",
            "devtools_resources.pak",
            "cef.pak",
            "cef_100_percent.pak",
            "cef_200_percent.pak"
        };

        /// <summary>
        /// List of CefSharp Dependencies
        /// </summary>
        public static string[] CefSharpDependencies =
        {
            "CefSharp.Core.dll",
            "CefSharp.dll",
            "CefSharp.BrowserSubprocess.Core.dll",
            "CefSharp.BrowserSubprocess.exe"
        };

        /// <summary>
        /// Check Dependencies relative to the executing assembly
        /// </summary>
        /// <param name="localePackFile">The locale pack file, if empty then locales\en-US.pak will be used</param>
        /// <returns>List of missing dependencies, if all present an empty List will be returned</returns>
        public static List<string> CheckDependencies(string localePackFile = LocalPackPath)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            var path = Path.GetDirectoryName(executingAssembly.Location);

            return CheckDependencies(path, localePackFile);
        }

        /// <summary>
        /// CheckDependencies iterates through the list of Cef and CefSharp dependencines
        /// relative to the path provided and returns a list of missing ones
        /// </summary>
        /// <param name="path">path to check for dependencies</param>
        /// <param name="localePackFile">The locale pack file e.g. locales\en-US.pak</param>
        /// <returns>List of missing dependencies, if all present an empty List will be returned</returns>
        public static List<string> CheckDependencies(string path, string localePackFile)
        {
            var missingDependencies = new List<string>();

            //Loop through Cef dependencies and add to list if not found
            foreach (var cefDependency in CefDependencies)
            {
                var dependencyPath = Path.Combine(path, cefDependency);

                if (!File.Exists(dependencyPath))
                {
                    missingDependencies.Add(cefDependency);
                }
            }

            // Loop through CefSharp dependencies and add to list if not found
            foreach (var cefSharpDependency in CefSharpDependencies)
            {
                var dependencyPath = Path.Combine(path, cefSharpDependency);

                if (!File.Exists(dependencyPath))
                {
                    missingDependencies.Add(cefSharpDependency);
                }
            }

            //If path path is not rooted (doesn't start with a drive letter + folder)
            //then make it relative to the executing assembly.
            var localePath = Path.IsPathRooted(localePackFile) ? localePackFile : Path.Combine(path, localePackFile);

            if (!File.Exists(localePath))
            {
                missingDependencies.Add(localePackFile);
            }

            return missingDependencies;
        }

        /// <summary>
        /// Shortcut method that calls <see cref="CheckDependencies(string)"/>
        /// </summary>
        /// <param name="localePackPath">The locale pack file, if empty then locales\en-US.pak will be used</param>
        /// <returns>Returns true of missing dependency count is 0</returns>
        public static bool AreAllDependenciesPresent(string localePackPath = LocalPackPath)
        {
            var missingDependencies = CheckDependencies(localePackPath);

            return missingDependencies.Count == 0;
        }
    }
}
