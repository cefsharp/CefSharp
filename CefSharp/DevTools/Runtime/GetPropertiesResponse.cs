// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// GetPropertiesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetPropertiesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Runtime.PropertyDescriptor> result
        {
            get;
            set;
        }

        /// <summary>
        /// Object properties.
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Runtime.PropertyDescriptor> Result
        {
            get
            {
                return result;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Runtime.InternalPropertyDescriptor> internalProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Internal object properties (only of the element itself).
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Runtime.InternalPropertyDescriptor> InternalProperties
        {
            get
            {
                return internalProperties;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Runtime.PrivatePropertyDescriptor> privateProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Object private properties.
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Runtime.PrivatePropertyDescriptor> PrivateProperties
        {
            get
            {
                return privateProperties;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Runtime.ExceptionDetails exceptionDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public CefSharp.DevTools.Runtime.ExceptionDetails ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
        }
    }
}