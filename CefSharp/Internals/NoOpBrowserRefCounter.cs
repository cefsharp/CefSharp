// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;

namespace CefSharp.Internals
{
    /// <inheritdoc/>
    public sealed class NoOpBrowserRefCounter : IBrowserRefCounter
    {
        /// <inheritdoc/>
        int IBrowserRefCounter.Count
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        void IBrowserRefCounter.EnableLogging()
        {
            
        }

        /// <inheritdoc/>
        string IBrowserRefCounter.GetLog()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        bool IBrowserRefCounter.Decrement(Type type)
        {
            return false;
        }

        /// <inheritdoc/>
        void IBrowserRefCounter.Increment(Type type)
        {

        }

        /// <inheritdoc/>
        void IBrowserRefCounter.WaitForBrowsersToClose(int timeoutInMiliseconds)
        {

        }

        /// <inheritdoc/>
        void IBrowserRefCounter.WaitForBrowsersToClose(int timeoutInMiliseconds, CancellationToken cancellationToken)
        {

        }
    }
}
