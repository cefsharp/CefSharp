// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;

namespace CefSharp.Internals
{
    /// <summary>
    /// Browser Ref counter
    /// Used internally to keep track of open browser instances
    /// The ref count is incremented when a browser is created,
    /// and decremented when the browser has successfully closed.
    /// </summary>
    public interface IBrowserRefCounter : IDisposable
    {
        /// <summary>
        /// Increment browser count
        /// </summary>
        /// <param name="type">Browser type, used for logging internally</param>
        void Increment(Type type);

        /// <summary>
        /// Decrement browser count
        /// </summary>
        /// <param name="type">Browser type, used for logging internally</param>
        /// <returns>returns true if the count is 0, otherwise false</returns>
        bool Decrement(Type type);

        /// <summary>
        /// Gets the number of CefBrowser instances currently open (this includes popups)
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }

        /// <summary>
        /// Enable logging
        /// </summary>
        void EnableLogging();

        /// <summary>
        /// Gets the log (empty if not enabled).
        /// </summary>
        /// <returns>string</returns>
        string GetLog();

        /// <summary>
        /// Blocks until the CefBrowser count has reached 0 or the timeout has been reached
        /// </summary>
        /// <param name="timeoutInMiliseconds">(Optional) The timeout in miliseconds.</param>
        void WaitForBrowsersToClose(int timeoutInMiliseconds = 500);

        /// <summary>
        /// Blocks until the CefBrowser count has reached 0 or the timeout has been reached
        /// </summary>
        /// <param name="timeoutInMiliseconds">(Optional) The timeout in miliseconds.</param>
        /// <param name="cancellationToken">(Optional) The cancellation token.</param>
        void WaitForBrowsersToClose(int timeoutInMiliseconds, CancellationToken cancellationToken);
    }
}
