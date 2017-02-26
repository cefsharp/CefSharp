// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
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

        public CefLibraryHandle(string filename) : base(IntPtr.Zero, true)
        {
            var handle = LoadLibraryEx(filename, IntPtr.Zero, LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH);
            base.SetHandle(handle);
        }

        public override bool IsInvalid
        {
           get { return this.handle == IntPtr.Zero; }
        }

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
