// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Callback
{
    /// <summary>
    /// Provides a callback implementation of <see cref="ICompletionCallback"/>
    /// that does nothing with complete.
    /// Added to workaround a CEF bug as per https://github.com/cefsharp/CefSharp/issues/2957#issuecomment-555285400
    /// </summary>
    public sealed class NoOpCompletionCallback : ICompletionCallback
    {
        /// <inheritdoc/>
        void ICompletionCallback.OnComplete()
        {

        }

        /// <inheritdoc/>
        bool ICompletionCallback.IsDisposed
        {
            //For now we return false
            get { return false; }
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {

        }
    }
}
