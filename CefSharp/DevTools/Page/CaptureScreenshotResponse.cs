// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// CaptureScreenshotResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CaptureScreenshotResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string data
        {
            get;
            set;
        }

        /// <summary>
        /// Base64-encoded image data.
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }
        }
    }
}