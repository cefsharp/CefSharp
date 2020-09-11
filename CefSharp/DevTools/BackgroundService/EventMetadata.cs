// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.BackgroundService
{
    /// <summary>
    /// A key-value pair for additional event information to pass along.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class EventMetadata : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Key
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("key"), IsRequired = (true))]
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Value
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public string Value
        {
            get;
            set;
        }
    }
}