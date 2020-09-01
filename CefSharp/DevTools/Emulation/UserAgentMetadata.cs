// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
    /// </summary>
    public class UserAgentMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public System.Collections.Generic.IList<UserAgentBrandVersion> Brands
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FullVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Platform
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PlatformVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Architecture
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Model
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Mobile
        {
            get;
            set;
        }
    }
}