// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    /// <summary>
    /// Interface to convert a JavascriptCallback dto to a callable implementation.
    /// </summary>
    public interface IJavascriptCallbackFactory
    {
        IJavascriptCallback Create(JavascriptCallback callback);
    }
}
