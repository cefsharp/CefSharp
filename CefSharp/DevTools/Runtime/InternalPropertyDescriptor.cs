// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object internal property descriptor. This property isn't normally visible in JavaScript code.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class InternalPropertyDescriptor : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Conventional property name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The value associated with the property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Value
        {
            get;
            set;
        }
    }
}