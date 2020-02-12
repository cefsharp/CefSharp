// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
// 
using System.Threading;

namespace CefSharp.Internals
{
    /// <summary>
    /// Tracks the number of browser instances currently open
    /// The cound will be incrmented and decremented each time a CefBrowser is created/closed.
    /// This includes CefBrowser popup instances.
    /// </summary>
    /// <remarks>
    /// Roughtly based on <see cref="CountdownEvent"/>, unforeunately <see cref="CountdownEvent.AddCount"/>
    /// doesn't reset the internal <see cref="ManualResetEventSlim"/> when Count is aleady 0.
    /// In our case it's valid to increase the number of browsers and reset the event.
    /// </remarks>
    public class BrowserRefCounter
    {
        private volatile int count = 0;
        private ManualResetEventSlim manualResetEvent = new ManualResetEventSlim();

        /// TODO: Refactor this so it's not static.
        public static BrowserRefCounter Instance = new BrowserRefCounter();

        /// <summary>
        /// Increment browser count
        /// </summary>
        public void Increment()
        {
            var newCount = Interlocked.Increment(ref count);

            if (newCount > 0)
            {
                manualResetEvent.Reset();
            }
        }

        /// <summary>
        /// Decrement browser count
        /// </summary>
        public bool Decrement()
        {
            var newCount = Interlocked.Decrement(ref count);
            if (newCount == 0)
            {
                manualResetEvent.Set();
                return true;
            }

            if (newCount < 0)
            {
                //If we went below 0 then reset to 0
                // TODO: something went wrong with our tracking
                Interlocked.Exchange(ref count, 0);

                manualResetEvent.Set();
            }

            return false;
        }

        /// <summary>
        /// Gets the number of CefBrowser instances currently open (this includes popups)
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                int observedCount = count;
                return observedCount < 0 ? 0 : observedCount;
            }
        }

        /// <summary>
        /// Blocks until the CefBrowser count has reached 0 or the timeout has been reached
        /// </summary>
        /// <param name="timeoutInMiliseconds">(Optional) The timeout in miliseconds.</param>
        public void WaitForBrowsersToClose(int timeoutInMiliseconds = 500)
        {
            if (!manualResetEvent.IsSet)
            {
                manualResetEvent.Wait(timeoutInMiliseconds);
            }
        }

        /// <summary>
        /// Blocks until the CefBrowser count has reached 0 or the timeout has been reached
        /// </summary>
        /// <param name="timeoutInMiliseconds">(Optional) The timeout in miliseconds.</param>
        /// <param name="cancellationToken">(Optional) The cancellation token.</param>
        public void WaitForBrowsersToClose(int timeoutInMiliseconds, CancellationToken cancellationToken)
        {
            if (!manualResetEvent.IsSet)
            {
                manualResetEvent.Wait(timeoutInMiliseconds, cancellationToken);
            }
        }
    }
}
