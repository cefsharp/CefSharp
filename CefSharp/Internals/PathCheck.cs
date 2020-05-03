// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Internals
{
    /// <summary>
    /// Path Validation utility class
    /// </summary>
    //TODO: Better name
    public static class PathCheck
    {
        /// <summary>
        /// Allow user to disable the assert.
        /// As a temporary measure we'll allow users to disable the assert
        /// as the check may not yet be 100% bulletproof.
        /// </summary>
        [Obsolete("This will be removed after further testing.")]
        public static bool EnableAssert = true;

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
                if (IsAbsolute(path))
                {
                    throw new Exception(settingName + " now requires an absolute path, the path provided is non-absolute. You can use System.IO.Path.GetFullPath to obtain an absolute path. Current value:" + path);
                }
            }
        }

        /// <summary>
        /// Throw exception if the path provided is non-asbolute
        /// CEF now explicitly requires absolute paths
        /// https://bitbucket.org/chromiumembedded/cef/issues/2916/not-persisting-in-local-stoage-when-using
        /// Empty paths are ignored
        /// </summary>
        /// <param name="path">path</param>
        public static bool IsAbsolute(string path)
        {
            const string directorySeperator = "\\";

            //We don't have access to Path.IsPathFullyQualified as it's not in .Net 4.5.2 so we have to implement our own check
            //If this isn't sufficent then we can import the code for .Net Core
            //https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/IO/Path.cs#L290

            //IsPathRooted will return true for paths that start with a single slash, e.g. \programfiles
            //Based on https://stackoverflow.com/a/35046453/4583726
            if (!Path.IsPathRooted(path) || Path.GetPathRoot(path).Equals(directorySeperator, StringComparison.Ordinal))
            {
                return false;
            }

            return true;
        }
    }
}
