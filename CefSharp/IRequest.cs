// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Specialized;

namespace CefSharp
{
    public interface IRequest : IDisposable
    {
        string Url { get; set; }
        string Method { get; }
        NameValueCollection Headers { get; set; }
        IPostData PostData { get; }
        
        /// <summary>
        /// Get the transition type for this request.
        /// Applies to requests that represent a main frame or sub-frame navigation.
        /// </summary>
        TransitionType TransitionType { get; }
    }
}
