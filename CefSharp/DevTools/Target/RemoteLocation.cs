// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// RemoteLocation
    /// </summary>
    public class RemoteLocation : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("host"), IsRequired = (true))]
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("port"), IsRequired = (true))]
        public int Port
        {
            get;
            set;
        }
    }
}