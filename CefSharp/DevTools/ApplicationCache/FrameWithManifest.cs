// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ApplicationCache
{
    /// <summary>
    /// Frame identifier - manifest URL pair.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FrameWithManifest : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Frame identifier.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frameId"), IsRequired = (true))]
        public string FrameId
        {
            get;
            set;
        }

        /// <summary>
        /// Manifest URL.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("manifestURL"), IsRequired = (true))]
        public string ManifestURL
        {
            get;
            set;
        }

        /// <summary>
        /// Application cache status.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("status"), IsRequired = (true))]
        public int Status
        {
            get;
            set;
        }
    }
}