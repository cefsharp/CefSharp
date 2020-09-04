// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// HTTP request data.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Request URL (without fragment).
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Fragment of the requested URL starting with hash, if present.
        /// </summary>
        public string UrlFragment
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request method.
        /// </summary>
        public string Method
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers.
        /// </summary>
        public Headers Headers
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP POST request data.
        /// </summary>
        public string PostData
        {
            get;
            set;
        }

        /// <summary>
        /// True when the request has POST data. Note that postData might still be omitted when this flag is true when the data is too long.
        /// </summary>
        public bool? HasPostData
        {
            get;
            set;
        }

        /// <summary>
        /// Request body elements. This will be converted from base64 to binary
        /// </summary>
        public System.Collections.Generic.IList<PostDataEntry> PostDataEntries
        {
            get;
            set;
        }

        /// <summary>
        /// The mixed content type of the request.
        /// </summary>
        public string MixedContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Priority of the resource request at the time request is sent.
        /// </summary>
        public string InitialPriority
        {
            get;
            set;
        }

        /// <summary>
        /// The referrer policy of the request, as defined in https://www.w3.org/TR/referrer-policy/
        /// </summary>
        public string ReferrerPolicy
        {
            get;
            set;
        }

        /// <summary>
        /// Whether is loaded via link preload.
        /// </summary>
        public bool? IsLinkPreload
        {
            get;
            set;
        }
    }
}