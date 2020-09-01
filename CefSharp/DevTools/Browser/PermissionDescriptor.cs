// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Definition of PermissionDescriptor defined in the Permissions API:
    public class PermissionDescriptor
    {
        /// <summary>
        /// Name of permission.
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// For "midi" permission, may also specify sysex control.
        /// </summary>
        public bool Sysex
        {
            get;
            set;
        }

        /// <summary>
        /// For "push" permission, may specify userVisibleOnly.
        public bool UserVisibleOnly
        {
            get;
            set;
        }

        /// <summary>
        /// For "clipboard" permission, may specify allowWithoutSanitization.
        /// </summary>
        public bool AllowWithoutSanitization
        {
            get;
            set;
        }
    }
}