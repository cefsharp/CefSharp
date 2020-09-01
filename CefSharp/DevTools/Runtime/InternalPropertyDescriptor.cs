// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object internal property descriptor. This property isn't normally visible in JavaScript code.
    /// </summary>
    public class InternalPropertyDescriptor
    {
        /// <summary>
        /// Conventional property name.
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
    }
}