// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp
{
    /// <summary>
    /// Callback interface used for asynchronous continuation of <see cref="IExtensionHandler.GetExtensionResource"/>.
    /// </summary>
    public interface IGetExtensionResourceCallback : IDisposable
    {
        /// <summary>
        /// Continue the request. Read the resource contents from stream.
        /// </summary>
        /// <param name="stream">stream to be used as response.</param>
        void Continue(Stream stream);

        /// <summary>
        /// Continue the request
        /// </summary>
        /// <param name="data">data to be used as response</param>
        void Continue(byte[] data);

        /// <summary>
        /// Cancel the request.
        /// </summary>
        void Cancel();
    }
}
