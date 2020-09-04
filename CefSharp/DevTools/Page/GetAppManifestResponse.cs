// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetAppManifestResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetAppManifestResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string url
        {
            get;
            set;
        }

        /// <summary>
        /// Manifest location.
        /// </summary>
        public string Url
        {
            get
            {
                return url;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<AppManifestError> errors
        {
            get;
            set;
        }

        /// <summary>
        /// errors
        /// </summary>
        public System.Collections.Generic.IList<AppManifestError> Errors
        {
            get
            {
                return errors;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string data
        {
            get;
            set;
        }

        /// <summary>
        /// Manifest content.
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal AppManifestParsedProperties parsed
        {
            get;
            set;
        }

        /// <summary>
        /// Parsed manifest properties
        /// </summary>
        public AppManifestParsedProperties Parsed
        {
            get
            {
                return parsed;
            }
        }
    }
}