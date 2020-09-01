// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
    /// </summary>
    public class UserAgentBrandVersion
    {
        /// <summary>
        /// 
        /// </summary>
        public string Brand
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Version
        {
            get;
            set;
        }
    }
}