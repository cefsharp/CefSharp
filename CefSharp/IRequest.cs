// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Specialized;

namespace CefSharp
{
    public interface IRequest : IDisposable
    {
        /// <summary>
        /// Request Url
        /// </summary>
        string Url { get; set; }

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
        /// Get the referrer policy.
        /// </summary>
        ReferrerPolicy ReferrerPolicy { get; }

        /// <summary>
        /// Header Collection
        /// NOTE: This collection is a copy of the underlying type, to make changes, take a reference to the collection,
        /// make your changes, then reassign the collection. At some point this will be replaced with a proper wrapper.
        /// </summary>
        NameValueCollection Headers { get; set; }

        /// <summary>
        /// Post data
        /// </summary>
        IPostData PostData { get; }
        
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
        /// Initialize a new instance of <see cref="IPostData"/>.
        /// Make sure to check if the <see cref="PostData"/> is null
        /// before calling otherwise the existing data will be overridden. 
        /// </summary>
        void InitializePostData();
    }
}
