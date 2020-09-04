// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// GetPropertiesResponse
    /// </summary>
    public class GetPropertiesResponse
    {
        /// <summary>
        /// Object properties.
        /// </summary>
        public System.Collections.Generic.IList<PropertyDescriptor> result
        {
            get;
            set;
        }

        /// <summary>
        /// Internal object properties (only of the element itself).
        /// </summary>
        public System.Collections.Generic.IList<InternalPropertyDescriptor> internalProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Object private properties.
        /// </summary>
        public System.Collections.Generic.IList<PrivatePropertyDescriptor> privateProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public ExceptionDetails exceptionDetails
        {
            get;
            set;
        }
    }
}