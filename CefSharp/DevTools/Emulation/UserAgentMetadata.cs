// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
    /// </summary>
    public class UserAgentMetadata : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("brands"), IsRequired = (true))]
        public System.Collections.Generic.IList<UserAgentBrandVersion> Brands
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fullVersion"), IsRequired = (true))]
        public string FullVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("platform"), IsRequired = (true))]
        public string Platform
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("platformVersion"), IsRequired = (true))]
        public string PlatformVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("architecture"), IsRequired = (true))]
        public string Architecture
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("model"), IsRequired = (true))]
        public string Model
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mobile"), IsRequired = (true))]
        public bool Mobile
        {
            get;
            set;
        }
    }
}