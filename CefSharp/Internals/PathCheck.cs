// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace CefSharp.Internals
{
    /// <summary>
    /// Path Validation utility class
    /// </summary>
    //TODO: Better name
    public static class PathCheck
    {
        internal const char DirectorySeparatorChar = '\\';
        internal const char AltDirectorySeparatorChar = '/';

        /// <summary>
        /// Allow user to disable the assert.
        /// As a temporary measure we'll allow users to disable the assert
        /// as the check may not yet be 100% bulletproof.
        /// </summary>
        [Obsolete("This will be removed after further testing.")]
        public static bool EnableAssert = true;

        internal static int FindDriveLetter(string path)
        {
            if (path.Length >= 2 && path[1] == ':' &&
                ((path[0] >= 'A' && path[0] <= 'Z') || (path[0] >= 'a' && path[0] <= 'z')))
            {
                return 1;
            }
            return -1;
        }

        /// <summary>
        /// True if the given character is a directory separator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsDirectorySeparator(char c)
        {
            return c == DirectorySeparatorChar || c == AltDirectorySeparatorChar;
        }

        /// <summary>
        /// Throw exception if the path provided is non-asbolute
        /// CEF now explicitly requires absolute paths
        /// https://bitbucket.org/chromiumembedded/cef/issues/2916/not-persisting-in-local-stoage-when-using
        /// Empty paths are ignored
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="settingName">string to appear at the start of
        /// the exception, e.g. CefSettings.BrowserSubProcessPath</param>
        public static void AssertAbsolute(string path, string settingName)
        {
            //Don't validate empty paths
            if (!string.IsNullOrEmpty(path) && EnableAssert)
            {
                //IsPathRooted will return true for paths that start with a single slash, e.g. \programfiles
                if (!IsAbsolute(path))
                {
                    throw new Exception(settingName + " now requires an absolute path, the path provided is non-absolute. You can use System.IO.Path.GetFullPath to obtain an absolute path. Current value:" + path);
                }
            }
        }

        /// <summary>
        /// Valid path is absolute, based on Chromium implementation.
        /// </summary>
        /// <param name="path">path</param>
        public static bool IsAbsolute(string path)
        {
            //Based on Chromium FilePath::IsAbsolute
            //https://source.chromium.org/chromium/chromium/src/+/master:base/files/file_path.cc;drc=1c097f5f790782b2ad0b897cd9e2921ce9713585;l=97?q=IsAbsolute&ss=chromium&originalUrl=https:%2F%2Fcs.chromium.org%2F
            var pos = FindDriveLetter(path);
            if (pos != -1)
            {
                // Look for a separator right after the drive specification.
                return path.Length > (pos + 1) && IsDirectorySeparator(path[pos + 1]);
            }

            // Look for a pair of leading separators.
            return path.Length > 1 && IsDirectorySeparator(path[0]) && IsDirectorySeparator(path[1]);
        }
    }
}
