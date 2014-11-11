// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Specialized;

namespace CefSharp
{
    public interface IRequest
    {
        string Url { get; set; }
        string Method { get; }
        string Body { get; }
        NameValueCollection Headers { get; set; }
        
        /// <summary>
        /// Get the transition type for this request.
        /// Applies to requests that represent a main frame or sub-frame navigation.
        /// </summary>
        TransitionType TransitionType { get; }
    }
}
