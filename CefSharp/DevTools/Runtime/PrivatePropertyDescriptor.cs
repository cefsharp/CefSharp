// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object private field descriptor.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PrivatePropertyDescriptor : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Private property name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The value associated with the private property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Value
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a getter for the private property,
        /// or `undefined` if there is no getter (accessor descriptors only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("get"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Get
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a setter for the private property,
        /// or `undefined` if there is no setter (accessor descriptors only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("set"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Set
        {
            get;
            set;
        }
    }
}