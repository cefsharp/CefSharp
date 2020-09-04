// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// GetVersionResponse
    /// </summary>
    public class GetVersionResponse
    {
        /// <summary>
        /// Protocol version.
        /// </summary>
        public string protocolVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Product name.
        /// </summary>
        public string product
        {
            get;
            set;
        }

        /// <summary>
        /// Product revision.
        /// </summary>
        public string revision
        {
            get;
            set;
        }

        /// <summary>
        /// User-Agent.
        /// </summary>
        public string userAgent
        {
            get;
            set;
        }

        /// <summary>
        /// V8 version.
        /// </summary>
        public string jsVersion
        {
            get;
            set;
        }
    }
}