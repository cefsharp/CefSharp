// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object property descriptor.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PropertyDescriptor : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Property name or symbol description.
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

        /// <summary>
        /// True if the value associated with the property may be changed (data descriptors only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("writable"), IsRequired = (false))]
        public bool? Writable
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a getter for the property, or `undefined` if there is no getter
        /// (accessor descriptors only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("get"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Get
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a setter for the property, or `undefined` if there is no setter
        /// (accessor descriptors only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("set"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Set
        {
            get;
            set;
        }

        /// <summary>
        /// True if the type of this property descriptor may be changed and if the property may be
        /// deleted from the corresponding object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("configurable"), IsRequired = (true))]
        public bool Configurable
        {
            get;
            set;
        }

        /// <summary>
        /// True if this property shows up during enumeration of the properties on the corresponding
        /// object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("enumerable"), IsRequired = (true))]
        public bool Enumerable
        {
            get;
            set;
        }

        /// <summary>
        /// True if the result was thrown during the evaluation.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("wasThrown"), IsRequired = (false))]
        public bool? WasThrown
        {
            get;
            set;
        }

        /// <summary>
        /// True if the property is owned for the object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isOwn"), IsRequired = (false))]
        public bool? IsOwn
        {
            get;
            set;
        }

        /// <summary>
        /// Property symbol object, if the property is of the `symbol` type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("symbol"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Symbol
        {
            get;
            set;
        }
    }
}