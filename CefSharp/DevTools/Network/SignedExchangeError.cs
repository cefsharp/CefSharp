// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange response.
    /// </summary>
    public class SignedExchangeError
    {
        /// <summary>
        /// Error message.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// The index of the signature which caused the error.
        /// </summary>
        public int SignatureIndex
        {
            get;
            set;
        }

        /// <summary>
        /// The field which caused the error.
        /// </summary>
        public string ErrorField
        {
            get;
            set;
        }
    }
}