// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetAppManifestResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetAppManifestResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string url
        {
            get;
            set;
        }

        /// <summary>
        /// url
        /// </summary>
        public string Url
        {
            get
            {
                return url;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Page.AppManifestError> errors
        {
            get;
            set;
        }

        /// <summary>
        /// errors
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Page.AppManifestError> Errors
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
        /// data
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Page.AppManifestParsedProperties parsed
        {
            get;
            set;
        }

        /// <summary>
        /// parsed
        /// </summary>
        public CefSharp.DevTools.Page.AppManifestParsedProperties Parsed
        {
            get
            {
                return parsed;
            }
        }
    }
}