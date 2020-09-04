// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetAppManifestResponse
    /// </summary>
    public class GetAppManifestResponse
    {
        /// <summary>
        /// Manifest location.
        /// </summary>
        public string url
        {
            get;
            set;
        }

        /// <summary>
        /// errors
        /// </summary>
        public System.Collections.Generic.IList<AppManifestError> errors
        {
            get;
            set;
        }

        /// <summary>
        /// Manifest content.
        /// </summary>
        public string data
        {
            get;
            set;
        }

        /// <summary>
        /// Parsed manifest properties
        /// </summary>
        public AppManifestParsedProperties parsed
        {
            get;
            set;
        }
    }
}