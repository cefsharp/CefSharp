// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// TargetInfo
    /// </summary>
    public class TargetInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string TargetId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the target has an attached client.
        /// </summary>
        public bool Attached
        {
            get;
            set;
        }

        /// <summary>
        /// Opener target Id
        /// </summary>
        public string OpenerId
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the opened window has access to the originating window.
        /// </summary>
        public bool CanAccessOpener
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string BrowserContextId
        {
            get;
            set;
        }
    }
}