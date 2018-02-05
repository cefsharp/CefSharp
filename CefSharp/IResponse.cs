// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Specialized;

namespace CefSharp
{
    /// <summary>
    /// Class used to represent a web response. The methods of this class may be called on any thread. 
    /// </summary>
    public interface IResponse : IDisposable
    {
        /// <summary>
        /// MimeType
        /// </summary>
        string MimeType { get; set; }

        /// <summary>
        /// Response Headers
        /// </summary>
        NameValueCollection ResponseHeaders { get; set; }

        /// <summary>
        /// Returns true if this object is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Get/set the response error code.
        /// </summary>
        CefErrorCode ErrorCode { get; set; }

        /// <summary>
        /// The status code of the response. Unless set, the default value used is 200
        /// (corresponding to HTTP status OK).
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Status Text
        /// </summary>
        string StatusText { get; set; }
    }
}
