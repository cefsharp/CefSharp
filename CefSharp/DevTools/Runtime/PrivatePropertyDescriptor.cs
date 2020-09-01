// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object private field descriptor.
    /// </summary>
    public class PrivatePropertyDescriptor
    {
        /// <summary>
        /// Private property name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The value associated with the private property.
        /// </summary>
        public RemoteObject Value
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a getter for the private property,
        public RemoteObject Get
        {
            get;
            set;
        }

        /// <summary>
        /// A function which serves as a setter for the private property,
        public RemoteObject Set
        {
            get;
            set;
        }
    }
}