// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Interface to implement for visiting cookie values. 
    /// The methods of this class will always be called on the IO thread.
    /// </summary>
    public interface ICookieVisitor
    {
        bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie);
    }
}
