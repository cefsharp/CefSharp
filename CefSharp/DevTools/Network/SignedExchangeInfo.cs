// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange response.
    /// </summary>
    public class SignedExchangeInfo
    {
        /// <summary>
        /// The outer response of signed HTTP exchange which was received from network.
        /// </summary>
        public Response OuterResponse
        {
            get;
            set;
        }

        /// <summary>
        /// Information about the signed exchange header.
        /// </summary>
        public SignedExchangeHeader Header
        {
            get;
            set;
        }

        /// <summary>
        /// Security details for the signed exchange header.
        /// </summary>
        public SecurityDetails SecurityDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Errors occurred while handling the signed exchagne.
        /// </summary>
        public System.Collections.Generic.IList<SignedExchangeError> Errors
        {
            get;
            set;
        }
    }
}