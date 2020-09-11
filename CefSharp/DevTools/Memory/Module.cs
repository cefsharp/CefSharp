// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Memory
{
    /// <summary>
    /// Executable module information
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Module : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Name of the module.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// UUID of the module.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("uuid"), IsRequired = (true))]
        public string Uuid
        {
            get;
            set;
        }

        /// <summary>
        /// Base address where the module is loaded into memory. Encoded as a decimal
        /// or hexadecimal (0x prefixed) string.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("baseAddress"), IsRequired = (true))]
        public string BaseAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Size of the module in bytes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("size"), IsRequired = (true))]
        public long Size
        {
            get;
            set;
        }
    }
}