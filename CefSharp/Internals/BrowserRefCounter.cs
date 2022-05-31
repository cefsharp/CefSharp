// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
// 
using System;
using System.Text;
using System.Threading;

namespace CefSharp.Internals
{
    /// <summary>
    /// Tracks the number of browser instances currently open
    /// The cound will be incrmented and decremented each time a CefBrowser is created/closed.
    /// This includes CefBrowser popup instances.
    /// </summary>
    /// <remarks>
    /// Roughtly based on <see cref="CountdownEvent"/>, unforeunately <see cref="CountdownEvent.AddCount(int)"/>
    /// doesn't reset the internal <see cref="ManualResetEventSlim"/> when Count is aleady 0.
    /// In our case it's valid to increase the number of browsers and reset the event.
    /// </remarks>
    public sealed class BrowserRefCounter : IBrowserRefCounter
    {
        private volatile int count = 0;
        private ManualResetEventSlim manualResetEvent = new ManualResetEventSlim();
        private bool loggingEnabled = false;
        private StringBuilder logger = new StringBuilder();

        /// TODO: Refactor this so it's not static.
        public static IBrowserRefCounter Instance = new NoOpBrowserRefCounter();

        /// <inheritdoc/>
        void IBrowserRefCounter.Increment(Type type)
        {
            if(loggingEnabled)
            {
                logger.AppendLine($"Incremented - {type}");
            }

            var newCount = Interlocked.Increment(ref count);

            if (newCount > 0)
            {
                manualResetEvent.Reset();

                if (loggingEnabled)
                {
                    logger.AppendLine("Incremented - ManualResetEvent was reset");
                }
            }
        }

        /// <inheritdoc/>
        bool IBrowserRefCounter.Decrement(Type type)
        {
            if (loggingEnabled)
            {
                logger.AppendLine($"Decremented - {type}");
            }

            var newCount = Interlocked.Decrement(ref count);

            if (loggingEnabled)
            {
                logger.AppendLine($"Decremented - Current Count {newCount}");
            }

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

        /// <inheritdoc/>
        int IBrowserRefCounter.Count
        {
            get
            {
                int observedCount = count;
                return observedCount < 0 ? 0 : observedCount;
            }
        }

        /// <inheritdoc/>
        void IBrowserRefCounter.WaitForBrowsersToClose(int timeoutInMiliseconds)
        {
            if (loggingEnabled)
            {
                logger.AppendLine($"WaitForBrowsersToClose - Current Count {count}");
            }

            if (!manualResetEvent.IsSet)
            {
                manualResetEvent.Wait(timeoutInMiliseconds);
            }

            if (loggingEnabled)
            {
                logger.AppendLine($"WaitForBrowsersToClose - Updated Count {count}");
            }
        }

        /// <inheritdoc/>
        void IBrowserRefCounter.WaitForBrowsersToClose(int timeoutInMiliseconds, CancellationToken cancellationToken)
        {
            if (loggingEnabled)
            {
                logger.AppendLine($"WaitForBrowsersToClose - Current Count {count}");
            }

            if (!manualResetEvent.IsSet)
            {
                manualResetEvent.Wait(timeoutInMiliseconds, cancellationToken);
            }

            if (loggingEnabled)
            {
                logger.AppendLine($"WaitForBrowsersToClose - Updated Count {count}");
            }
        }

        /// <inheritdoc/>
        void IBrowserRefCounter.EnableLogging()
        {
            loggingEnabled = true;
        }

        /// <inheritdoc/>
        string IBrowserRefCounter.GetLog()
        {
            return logger.ToString();
        }

        public void Dispose()
        {
            manualResetEvent.Dispose();
        }
    }
}
