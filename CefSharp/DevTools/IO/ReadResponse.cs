// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IO
{
    /// <summary>
    /// ReadResponse
    /// </summary>
    public class ReadResponse
    {
        /// <summary>
        /// Set if the data is base64-encoded
        /// </summary>
        public bool? base64Encoded
        {
            get;
            set;
        }

        /// <summary>
        /// Data that were read.
        /// </summary>
        public string data
        {
            get;
            set;
        }

        /// <summary>
        /// Set if the end-of-file condition occured while reading.
        /// </summary>
        public bool eof
        {
            get;
            set;
        }
    }
}