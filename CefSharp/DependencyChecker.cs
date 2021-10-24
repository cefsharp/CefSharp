// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
#if NETCOREAPP
using System.Linq;
#endif
using System.Reflection;
#if NETCOREAPP
using System.Runtime.InteropServices;
#endif
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
        /// File name of the Direct3D Compiler DLL.
        /// </summary>
        private const string D3DCompilerDll = "d3dcompiler_47.dll";

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
            "resources.pak",
            "chrome_100_percent.pak",
            "chrome_200_percent.pak"
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
            // The D3D Compiler isn't included in the win-arm64 redist; we remove it in the static constructor.
            D3DCompilerDll,
            //Crashpad support
            "chrome_elf.dll"
        };

        /// <summary>
        /// List of CefSharp Managed Dependencies (Those that are AnyCPU written in c#)
        /// </summary>
        public static string[] CefSharpManagedDependencies =
        {
            "CefSharp.Core.dll",
            "CefSharp.dll"
        };

        /// <summary>
        /// List of CefSharp Arch Specific Dependencies
        /// Those that are arch specific,
        /// distributed as x86, x64 and ARM64 (coming soon for .Net 5.0 only)
        /// </summary>
        public static string[] CefSharpArchSpecificDependencies =
        {
            "CefSharp.Core.Runtime.dll"
        };

        /// <summary>
        /// List of CefSharp.BrowserSubprocess.exe dependencies.
        /// </summary>
        public static string[] BrowserSubprocessDependencies =
        {
            "CefSharp.BrowserSubprocess.Core.dll",
            "CefSharp.dll",
            "icudtl.dat",
            "libcef.dll",
#if NETCOREAPP
            //C++/CLI Loader, required by CefSharp.BrowserSubprocess.Core.dll and CefSharp.Core.Runtime.dll
            "Ijwhost.dll"
#endif
        };

#if NETCOREAPP
        static DependencyChecker()
        {
            // win-arm64 doesn't ship with a copy of the D3D Compiler, it's included with the OS.
            if (RuntimeInformation.ProcessArchitecture is Architecture.Arm64)
            {
                CefOptionalDependencies = CefOptionalDependencies.Where(x => x != D3DCompilerDll).ToArray();
            }
        }
#endif

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
            return CheckDependencies(checkOptional, packLoadingDisabled, path, path, resourcesDirPath, browserSubProcessPath, localePackFile);
        }

        /// <summary>
        /// CheckDependencies iterates through the list of Cef and CefSharp dependencines
        /// relative to the path provided and returns a list of missing ones
        /// </summary>
        /// <param name="checkOptional">check to see if optional dependencies are present</param>
        /// <param name="packLoadingDisabled">Is loading of pack files disabled?</param>
        /// <param name="managedLibPath">path to check for mangaed dependencies</param>
        /// <param name="nativeLibPath">path to check for native (unmanged) dependencies</param>
        /// <param name="resourcesDirPath">The path to the resources directory, if empty the Executing Assembly path is used.</param>
        /// <param name="browserSubProcessPath">The path to a separate executable that will be launched for sub-processes.</param>
        /// <param name="localePackFile">The locale pack file e.g. <see cref="LocalesPackFile"/> </param>
        /// <returns>List of missing dependencies, if all present an empty List will be returned</returns>
        public static List<string> CheckDependencies(bool checkOptional, bool packLoadingDisabled, string managedLibPath, string nativeLibPath, string resourcesDirPath, string browserSubProcessPath, string localePackFile = LocalesPackFile)
        {
            var missingDependencies = new List<string>();

            missingDependencies.AddRange(CheckDependencyList(nativeLibPath, CefDependencies));

            if (!packLoadingDisabled)
            {
                missingDependencies.AddRange(CheckDependencyList(resourcesDirPath, CefResources));
            }

            if (checkOptional)
            {
                missingDependencies.AddRange(CheckDependencyList(nativeLibPath, CefOptionalDependencies));
            }

#if NETCOREAPP
            missingDependencies.AddRange(CheckDependencyList(managedLibPath, CefSharpArchSpecificDependencies));
#else
            missingDependencies.AddRange(CheckDependencyList(nativeLibPath, CefSharpArchSpecificDependencies));
#endif
            missingDependencies.AddRange(CheckDependencyList(managedLibPath, CefSharpManagedDependencies));

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
            var localePath = Path.IsPathRooted(localePackFile) ? localePackFile : Path.Combine(nativeLibPath, localePackFile);

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
        /// Shortcut method that calls <see cref="CheckDependencies(bool, bool, string, string, string, string, string)"/>, throws an Exception if not files are missing.
        /// </summary>
        /// <param name="locale">The locale, if empty then en-US will be used.</param>
        /// <param name="localesDirPath">The path to the locales directory, if empty locales\ will be used.</param>
        /// <param name="resourcesDirPath">The path to the resources directory, if empty the Executing Assembly path is used.</param>
        /// <param name="packLoadingDisabled">Is loading of pack files disabled?</param>
        /// <param name="browserSubProcessPath">The path to a separate executable that will be launched for sub-processes.</param>
        /// <exception cref="Exception">Throw when not all dependencies are present</exception>
        public static void AssertAllDependenciesPresent(string locale = null, string localesDirPath = null, string resourcesDirPath = null, bool packLoadingDisabled = false, string browserSubProcessPath = "CefSharp.BrowserSubProcess.exe")
        {
            string nativeLibPath;
            string managedLibPath = Path.GetDirectoryName(typeof(DependencyChecker).Assembly.Location);

            Uri pathUri;
            if (Uri.TryCreate(browserSubProcessPath, UriKind.Absolute, out pathUri) && pathUri.IsAbsoluteUri)
            {
                nativeLibPath = Path.GetDirectoryName(browserSubProcessPath);
            }
            else
            {
                var executingAssembly = Assembly.GetExecutingAssembly();

                nativeLibPath = Path.GetDirectoryName(executingAssembly.Location);
            }

            if (string.IsNullOrEmpty(locale))
            {
                locale = "en-US";
            }

            if (string.IsNullOrEmpty(localesDirPath))
            {
                localesDirPath = Path.Combine(nativeLibPath, "locales");
            }

            if (string.IsNullOrEmpty(resourcesDirPath))
            {
                resourcesDirPath = nativeLibPath;
            }

            var missingDependencies = CheckDependencies(true, packLoadingDisabled, managedLibPath, nativeLibPath, resourcesDirPath, browserSubProcessPath, Path.Combine(localesDirPath, locale + ".pak"));

            if (missingDependencies.Count > 0)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Unable to locate required Cef/CefSharp dependencies:");

                foreach (var missingDependency in missingDependencies)
                {
                    builder.AppendLine("Missing:" + missingDependency);
                }

                builder.AppendLine("Executing Assembly Path:" + nativeLibPath);

                throw new Exception(builder.ToString());
            }
        }
    }
}
