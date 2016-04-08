// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to receive string values asynchronously.
    /// </summary>
    public interface IStringVisitor
    {
        void Visit(string str);
    }
}
