// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CefSharp
{
    /// <summary>
    /// CefLibraryHandle is a SafeHandle that Loads libcef.dll and relesases it when disposed/finalized
    /// Calls LoadLibraryEx with LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH
    /// Make sure to set settings.BrowserSubprocessPath and settings.LocalesDirPath
    /// </summary>
    /// <remarks>Adapted from http://www.pinvoke.net/default.aspx/kernel32.loadlibraryex</remarks>
    public class CefLibraryHandle : SafeHandle
    {
        /// <summary>
        /// In general not a fan of having inline classes/enums
        /// In this case it's not something that I'd like to see exposed
        /// as it's just a helper and outside the scope of the project
        /// </summary>
        [Flags]
        private enum LoadLibraryFlags : uint
        {
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">libcef.dll full path.</param>
        public CefLibraryHandle(string path) : base(IntPtr.Zero, true)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("NotFound", path);
            }

            var handle = LoadLibraryEx(path, IntPtr.Zero, LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH);
            base.SetHandle(handle);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the handle value is invalid.
        /// </summary>
        /// <value>
        /// true if the handle value is invalid; otherwise, false.
        /// </value>
        public override bool IsInvalid
        {
            get { return this.handle == IntPtr.Zero; }
        }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it
        /// generates a releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            return FreeLibrary(this.handle);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);
    }
}
