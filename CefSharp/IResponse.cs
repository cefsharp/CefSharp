// Copyright © 2014 The CefSharp Authors. All rights reserved.
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
        /// Get/Set the response charset.
        /// </summary>
        string Charset { get; set; }

        /// <summary>
        /// MimeType
        /// </summary>
        string MimeType { get; set; }

        /// <summary>
        /// Response Headers
        /// </summary>
        NameValueCollection Headers { get; set; }

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

        /// <summary>
        /// Returns the first header value for name or an empty string if not found.
        /// Will not return the Referer value if any. Use <see cref="Headers"/> instead if name might have multiple values.
        /// </summary>
        /// <param name="name">header name</param>
        /// <returns>Returns the first header value for name or an empty string if not found.</returns>
        string GetHeaderByName(string name);

        /// <summary>
        /// Set the header name to value. The Referer value cannot be set using this method.
        /// Use <see cref="SetReferrer(string, ReferrerPolicy)"/> instead.
        /// </summary>
        /// <param name="name">header name</param>
        /// <param name="value">new header value</param>
        /// <param name="overwrite">If overwrite is true any existing values will be replaced with the new value. If overwrite is false any existing values will not be overwritten</param>
        void SetHeaderByName(string name, string value, bool overwrite);
    }
}
