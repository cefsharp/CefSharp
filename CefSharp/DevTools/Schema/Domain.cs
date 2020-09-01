// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Schema
{
    /// <summary>
    /// Description of the protocol domain.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// Domain name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Domain version.
        /// </summary>
        public string Version
        {
            get;
            set;
        }
    }
}