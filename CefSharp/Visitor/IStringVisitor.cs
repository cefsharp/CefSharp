// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to receive string values asynchronously.
    /// </summary>
    public interface IStringVisitor : IDisposable
    {
        /// <summary>
        ///  Method that will be executed.
        /// </summary>
        /// <param name="str">string (result of async execution)</param>
        void Visit(string str);
    }
}
