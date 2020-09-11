// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Definition of PermissionDescriptor defined in the Permissions API:
    public class PermissionDescriptor : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Name of permission.
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// For "midi" permission, may also specify sysex control.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sysex"), IsRequired = (false))]
        public bool? Sysex
        {
            get;
            set;
        }

        /// <summary>
        /// For "push" permission, may specify userVisibleOnly.
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("userVisibleOnly"), IsRequired = (false))]
        public bool? UserVisibleOnly
        {
            get;
            set;
        }

        /// <summary>
        /// For "wake-lock" permission, must specify type as either "screen" or "system".
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (false))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// For "clipboard" permission, may specify allowWithoutSanitization.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("allowWithoutSanitization"), IsRequired = (false))]
        public bool? AllowWithoutSanitization
        {
            get;
            set;
        }
    }
}