// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Specialized;

namespace CefSharp
{
    /// <summary>
    /// Class used to represent a web request. The methods of this class may be called on any thread. 
    /// </summary>
    public interface IRequest : IDisposable
    {
        ///// <summary>
        ///// Get/Set the Url to the first party for cookies used in combination with CefURLRequest.
        ///// </summary>
        ///// Note: If we every implment CefURLRequest then this will need to be added
        ////string FirstPartyForCookies { get; set; }

        /// <summary>
        /// Get/Set request flags, can be used to control caching policy
        /// </summary>
        UrlRequestFlags Flags { get; set; }

        /// <summary>
        /// Request Url
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Returns the globally unique identifier for this request or 0 if not specified.
        /// Can be used by <see cref="IRequestHandler"/> implementations in the browser process to track a
        /// single request across multiple callbacks.
        /// </summary>
        ulong Identifier { get; }

        /// <summary>
        /// Request Method GET/POST etc
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// Set the referrer URL and policy. If non-empty the referrer URL must be
        /// fully qualified with an HTTP or HTTPS scheme component. Any username,
        /// password or ref component will be removed.
        /// </summary>
        /// <param name="referrerUrl">the referrer url</param>
        /// <param name="policy">referrer policy</param>
        void SetReferrer(string referrerUrl, ReferrerPolicy policy);

        /// <summary>
        /// Get the referrer URL.
        /// </summary>
        string ReferrerUrl { get; }

        /// <summary>
        /// Get the resource type for this request.
        /// </summary>
        ResourceType ResourceType { get; }

        /// <summary>
        /// Get the referrer policy.
        /// </summary>
        ReferrerPolicy ReferrerPolicy { get; }

        /// <summary>
        /// Header Collection - If dealing with headers that only contain a single value then
        /// it's easier to use <see cref="SetHeaderByName(string, string, bool)"/> or <see cref="GetHeaderByName(string)"/>.
        /// You cannot modify the referrer using headers, use <see cref="SetReferrer(string, ReferrerPolicy)"/>.
        /// NOTE: This collection is a copy of the underlying type, to make changes, take a reference to the collection,
        /// make your changes, then reassign the collection.
        /// </summary>
        /// <example> 
        /// This example shows how to modify headers, make sure you reassign the collection
        /// once it's been modified.
        /// <code>
        /// var headers = request.Headers;
        /// var userAgent = headers["User-Agent"];
        /// headers["User-Agent"] = userAgent + " CefSharp";
        /// request.Headers = headers;
        /// </code>
        /// </example>
        NameValueCollection Headers { get; set; }

        /// <summary>
        /// Post data
        /// </summary>
        IPostData PostData { get; set; }

        /// <summary>
        /// Get the transition type for this request.
        /// Applies to requests that represent a main frame or sub-frame navigation.
        /// </summary>
        TransitionType TransitionType { get; }

        /// <summary>
        /// Gets a value indicating whether the request has been disposed of.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Returns true if this object is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="IPostData"/>.
        /// Make sure to check if the <see cref="PostData"/> is null
        /// before calling otherwise the existing data will be overridden. 
        /// </summary>
        void InitializePostData();

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
