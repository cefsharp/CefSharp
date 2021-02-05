// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// DependencyChecker provides a known list of Cef/CefSharp dependencies and 
    /// provides helper methods to check for their existance.
    /// </summary>
    public static class DependencyChecker
    {
        /// <summary>
        /// en-US Locales pak file location
        /// </summary>
        public const string LocalesPackFile = @"locales\en-US.pak";

        /// <summary>
        /// List of Cef Dependencies
        /// </summary>
        public static string[] CefDependencies =
        {
            // CEF core library
            "libcef.dll",
            // Unicode support
            "icudtl.dat",
            // V8 native mapping files, see
            // https://groups.google.com/a/chromium.org/forum/#!topic/chromium-packagers/75J9Y1vIc_E
            // http://www.magpcss.org/ceforum/viewtopic.php?f=6&t=12580
            // "natives_blob.bin" was removed
            // https://bugs.chromium.org/p/v8/issues/detail?id=7624#c60
            "snapshot_blob.bin",
            "v8_context_snapshot.bin"
        };

        /// <summary>
        /// List of Cef Resources (pack files)
        /// </summary>
        public static string[] CefResources =
        {
            // Pack Files
            // Note: Contains WebKit image and inspector resources.
            "devtools_resources.pak",
            "cef.pak",
            "cef_extensions.pak",
            "cef_100_percent.pak",
            "cef_200_percent.pak"
        };

        /// <summary>
        /// List of Optional CEF Dependencies
        /// </summary>
        public static string[] CefOptionalDependencies =
        {
            // Angle and Direct3D support
            // Note: Without these components HTML5 accelerated content like 2D canvas, 3D CSS and WebGL will not function.
            "libEGL.dll",
            "libGLESv2.dll",
            "d3dcompiler_47.dll",
            //Crashpad support
            "chrome_elf.dll"
        };

        /// <summary>
        /// List of CefSharp Dependencies
        /// </summary>
        public static string[] CefSharpDependencies =
        {
            "CefSharp.Core.Runtime.dll",
            "CefSharp.Core.dll",
            "CefSharp.dll"
        };

        /// <summary>
        /// List of CefSharp.BrowserSubprocess.exe dependencies.
        /// </summary>
        public static string[] BrowserSubprocessDependencies =
        {
            "CefSharp.BrowserSubprocess.Core.dll",
            "CefSharp.dll",
            "icudtl.dat",
            "libcef.dll"
        };

        /// <summary>
        /// CheckDependencies iterates through the list of Cef and CefSharp dependencines
        /// relative to the path provided and returns a list of missing ones
        /// </summary>
        /// <param name="checkOptional">check to see if optional dependencies are present</param>
        /// <param name="packLoadingDisabled">Is loading of pack files disabled?</param>
        /// <param name="path">path to check for dependencies</param>
        /// <param name="resourcesDirPath">The path to the resources directory, if empty the Executing Assembly path is used.</param>
        /// <param name="browserSubProcessPath">The path to a separate executable that will be launched for sub-processes.</param>
        /// <param name="localePackFile">The locale pack file e.g. <see cref="LocalesPackFile"/> </param>
        /// <returns>List of missing dependencies, if all present an empty List will be returned</returns>
        public static List<string> CheckDependencies(bool checkOptional, bool packLoadingDisabled, string path, string resourcesDirPath, string browserSubProcessPath, string localePackFile = LocalesPackFile)
        {
            var missingDependencies = new List<string>();

            missingDependencies.AddRange(CheckDependencyList(path, CefDependencies));

            if (!packLoadingDisabled)
            {
                missingDependencies.AddRange(CheckDependencyList(resourcesDirPath, CefResources));
            }

            if (checkOptional)
            {
                missingDependencies.AddRange(CheckDependencyList(path, CefOptionalDependencies));
            }

            missingDependencies.AddRange(CheckDependencyList(path, CefSharpDependencies));

            if (!File.Exists(browserSubProcessPath))
            {
                missingDependencies.Add(browserSubProcessPath);
            }

            var browserSubprocessDir = Path.GetDirectoryName(browserSubProcessPath);
            if (browserSubprocessDir == null)
            {
                missingDependencies.AddRange(BrowserSubprocessDependencies);
            }
            else
            {
                missingDependencies.AddRange(CheckDependencyList(browserSubprocessDir, BrowserSubprocessDependencies));
            }

            // If path is not rooted (doesn't start with a drive letter + folder)
            // then make it relative to the executing assembly.
            var localePath = Path.IsPathRooted(localePackFile) ? localePackFile : Path.Combine(path, localePackFile);

            if (!File.Exists(localePath))
            {
                missingDependencies.Add(localePackFile);
            }

            return missingDependencies;
        }

        /// <summary>
        /// Loop through dependencies and add to the returned missing dependency list if not found.
        /// </summary>
        /// <param name="dir">The directory of the dependencies, or the current directory if null.</param>
        /// <param name="files">The dependencies to check.</param>
        /// <returns>List of missing dependencies, if all present an empty List will be returned</returns>
        private static List<string> CheckDependencyList(string dir, IEnumerable<string> files)
        {
            var missingDependencies = new List<string>();

            foreach (var file in files)
            {
                var filePath = string.IsNullOrEmpty(dir) ? file : Path.Combine(dir, file);
                if (!File.Exists(filePath))
                {
                    missingDependencies.Add(filePath);
                }
            }

            return missingDependencies;
        }

        /// <summary>
        /// Checks if all Cef and CefSharp dependencies were found relative to the Executing Assembly.
        /// Shortcut method that calls <see cref="CheckDependencies"/>, throws an Exception if not files are missing.
        /// </summary>
        /// <param name="locale">The locale, if empty then en-US will be used.</param>
        /// <param name="localesDirPath">The path to the locales directory, if empty locales\ will be used.</param>
        /// <param name="resourcesDirPath">The path to the resources directory, if empty the Executing Assembly path is used.</param>
        /// <param name="packLoadingDisabled">Is loading of pack files disabled?</param>
        /// <param name="browserSubProcessPath">The path to a separate executable that will be launched for sub-processes.</param>
        /// <exception cref="Exception">Throw when not all dependencies are present</exception>
        public static void AssertAllDependenciesPresent(string locale = null, string localesDirPath = null, string resourcesDirPath = null, bool packLoadingDisabled = false, string browserSubProcessPath = "CefSharp.BrowserSubProcess.exe")
        {
            string path;

            Uri pathUri;
            if (Uri.TryCreate(browserSubProcessPath, UriKind.Absolute, out pathUri) && pathUri.IsAbsoluteUri)
            {
                path = Path.GetDirectoryName(browserSubProcessPath);
            }
            else
            {
                var executingAssembly = Assembly.GetExecutingAssembly();

                path = Path.GetDirectoryName(executingAssembly.Location);
            }

            if (string.IsNullOrEmpty(locale))
            {
                locale = "en-US";
            }

            if (string.IsNullOrEmpty(localesDirPath))
            {
                localesDirPath = @"locales";
            }

            if (string.IsNullOrEmpty(resourcesDirPath))
            {
                resourcesDirPath = path;
            }

            var missingDependencies = CheckDependencies(true, packLoadingDisabled, path, resourcesDirPath, browserSubProcessPath, Path.Combine(localesDirPath, locale + ".pak"));

            if (missingDependencies.Count > 0)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Unable to locate required Cef/CefSharp dependencies:");

                foreach (var missingDependency in missingDependencies)
                {
                    builder.AppendLine("Missing:" + missingDependency);
                }

                builder.AppendLine("Executing Assembly Path:" + path);

                throw new Exception(builder.ToString());
            }
        }
    }
}
