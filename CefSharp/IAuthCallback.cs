﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for asynchronous continuation of authentication requests.
    /// </summary>
    public interface IAuthCallback : IDisposable
    {
        /// <summary>
        /// Continue the authentication request.
        /// </summary>
        /// <param name="username">requested username</param>
        /// <param name="password">requested password</param>
        void Continue(string username, string password);

        /// <summary>
        /// Cancel the authentication request.
        /// </summary>
        void Cancel();
    }
}
