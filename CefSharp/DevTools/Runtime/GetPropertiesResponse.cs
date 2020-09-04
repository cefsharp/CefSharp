// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// GetPropertiesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetPropertiesResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<PropertyDescriptor> result
        {
            get;
            set;
        }

        /// <summary>
        /// Object properties.
        /// </summary>
        public System.Collections.Generic.IList<PropertyDescriptor> Result
        {
            get
            {
                return result;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<InternalPropertyDescriptor> internalProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Internal object properties (only of the element itself).
        /// </summary>
        public System.Collections.Generic.IList<InternalPropertyDescriptor> InternalProperties
        {
            get
            {
                return internalProperties;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<PrivatePropertyDescriptor> privateProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Object private properties.
        /// </summary>
        public System.Collections.Generic.IList<PrivatePropertyDescriptor> PrivateProperties
        {
            get
            {
                return privateProperties;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal ExceptionDetails exceptionDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public ExceptionDetails ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
        }
    }
}