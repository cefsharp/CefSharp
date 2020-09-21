// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ApplicationCache
{
    /// <summary>
    /// Detailed application cache resource information.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ApplicationCacheResource : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Resource url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Resource size.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("size"), IsRequired = (true))]
        public int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Resource type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }
    }
}