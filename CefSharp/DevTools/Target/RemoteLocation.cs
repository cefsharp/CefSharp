// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// RemoteLocation
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RemoteLocation : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Host
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("host"), IsRequired = (true))]
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// Port
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("port"), IsRequired = (true))]
        public int Port
        {
            get;
            set;
        }
    }
}