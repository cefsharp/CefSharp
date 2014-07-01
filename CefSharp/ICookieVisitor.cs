// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Net;

namespace CefSharp
{
    public interface ICookieVisitor
    {
        bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie);
    }
}
