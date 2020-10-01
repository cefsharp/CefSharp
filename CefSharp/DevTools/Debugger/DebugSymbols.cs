// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Debug symbols available for a wasm script.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DebugSymbols : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Type of the debug symbols.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the external symbol source.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("externalURL"), IsRequired = (false))]
        public string ExternalURL
        {
            get;
            set;
        }
    }
}