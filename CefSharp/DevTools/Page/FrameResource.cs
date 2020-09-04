// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Information about the Resource on the page.
    /// </summary>
    public class FrameResource
    {
        /// <summary>
        /// Resource URL.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Type of this resource.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Resource mimeType as determined by the browser.
        /// </summary>
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// last-modified timestamp as reported by server.
        /// </summary>
        public long? LastModified
        {
            get;
            set;
        }

        /// <summary>
        /// Resource content size.
        /// </summary>
        public long? ContentSize
        {
            get;
            set;
        }

        /// <summary>
        /// True if the resource failed to load.
        /// </summary>
        public bool? Failed
        {
            get;
            set;
        }

        /// <summary>
        /// True if the resource was canceled during loading.
        /// </summary>
        public bool? Canceled
        {
            get;
            set;
        }
    }
}