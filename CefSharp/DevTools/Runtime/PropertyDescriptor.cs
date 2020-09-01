// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object property descriptor.
    /// </summary>
    public class PropertyDescriptor
    {
        /// <summary>
        /// Property name or symbol description.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The value associated with the property.
        /// </summary>
        public RemoteObject Value
        {
            get;
            set;
        }

        /// <summary>
        /// True if the value associated with the property may be changed (data descriptors only).
        /// </summary>
        public bool Writable
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a getter for the property, or `undefined` if there is no getter
        public RemoteObject Get
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a setter for the property, or `undefined` if there is no setter
        public RemoteObject Set
        {
            get;
            set;
        }

        /// <summary>
        /// True if the type of this property descriptor may be changed and if the property may be
        public bool Configurable
        {
            get;
            set;
        }

        /// <summary>
        /// True if this property shows up during enumeration of the properties on the corresponding
        public bool Enumerable
        {
            get;
            set;
        }

        /// <summary>
        /// True if the result was thrown during the evaluation.
        /// </summary>
        public bool WasThrown
        {
            get;
            set;
        }

        /// <summary>
        /// True if the property is owned for the object.
        /// </summary>
        public bool IsOwn
        {
            get;
            set;
        }

        /// <summary>
        /// Property symbol object, if the property is of the `symbol` type.
        /// </summary>
        public RemoteObject Symbol
        {
            get;
            set;
        }
    }
}