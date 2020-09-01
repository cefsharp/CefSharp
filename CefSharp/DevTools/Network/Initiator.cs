// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about the request initiator.
    /// </summary>
    public class Initiator
    {
        /// <summary>
        /// Type of this initiator.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Initiator JavaScript stack trace, set for Script only.
        /// </summary>
        public Runtime.StackTrace Stack
        {
            get;
            set;
        }

        /// <summary>
        /// Initiator URL, set for Parser type or for Script type (when script is importing module) or for SignedExchange type.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Initiator line number, set for Parser type or for Script type (when script is importing
        public long LineNumber
        {
            get;
            set;
        }
    }
}