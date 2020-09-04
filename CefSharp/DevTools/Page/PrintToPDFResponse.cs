// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// PrintToPDFResponse
    /// </summary>
    public class PrintToPDFResponse
    {
        /// <summary>
        /// Base64-encoded pdf data. Empty if |returnAsStream| is specified.
        /// </summary>
        public string data
        {
            get;
            set;
        }

        /// <summary>
        /// A handle of the stream that holds resulting PDF data.
        /// </summary>
        public string stream
        {
            get;
            set;
        }
    }
}