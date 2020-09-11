// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Definition of PermissionDescriptor defined in the Permissions API:
    /// https://w3c.github.io/permissions/#dictdef-permissiondescriptor.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PermissionDescriptor : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Name of permission.
        /// See https://cs.chromium.org/chromium/src/third_party/blink/renderer/modules/permissions/permission_descriptor.idl for valid permission names.
        /// </summary>
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
        /// Note that userVisibleOnly = true is the only currently supported type.
        /// </summary>
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