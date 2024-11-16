// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Timers;
using System.Windows.Threading;
using CefSharp.Handler;

namespace CefSharp.Wpf.HwndHost.Handler
{
    /// <summary>
    /// A <see cref="IBrowserProcessHandler"/> implementation that can be used to
    /// integreate CEF into the WPF message loop (Dispatcher).
    /// Currently it's a very basic implementation.
    /// See the following link for the CEF reference implementation.
    /// https://bitbucket.org/chromiumembedded/cef/commits/1ff26aa02a656b3bc9f0712591c92849c5909e04?at=2785
    /// </summary>
    public class IntegratedMessageLoopBrowserProcessHandler
        : BrowserProcessHandler
    {
        /// <summary>
        /// Sixty Times per second
        /// </summary>
        public const int SixtyTimesPerSecond = 1000 / 60;  // 60fps
        /// <summary>
        /// Thirty Times per second
        /// </summary>
        public const int ThirtyTimesPerSecond = 1000 / 30;  //30fps

        private DispatcherTimer dispatcherTimer;
        private Dispatcher dispatcher;
        private readonly DispatcherPriority dispatcherPriority;
        private int interval;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dispatcher">WPF Dispatcher</param>
        /// <param name="dispatcherPriority">Priority at which <see cref="Cef.DoMessageLoopWork"/> is called using the Dispatcher</param>
        /// <param name="interval">the <see cref="Timer.Interval"/> in miliseconds (frame rate), for 30/60 times per second use
        /// <see cref="ThirtyTimesPerSecond"/>/<see cref="SixtyTimesPerSecond"/> respectively.</param>
        public IntegratedMessageLoopBrowserProcessHandler(Dispatcher dispatcher, DispatcherPriority dispatcherPriority = DispatcherPriority.Render, int interval = ThirtyTimesPerSecond)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            if (interval < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), "Argument less than 10. ");
            }

            dispatcherTimer = new DispatcherTimer(dispatcherPriority, dispatcher)
            {
                Interval = TimeSpan.FromMilliseconds(interval)
            };

            dispatcherTimer.Tick += OnDispatcherTimerTick;
            dispatcherTimer.Start();

            this.dispatcher = dispatcher;
            this.dispatcher.ShutdownStarted += DispatcherShutdownStarted;
            this.dispatcherPriority = dispatcherPriority;
            this.interval = interval;

            Cef.ShutdownStarted += OnCefShutdownStarted;
        }

        private void OnCefShutdownStarted(object sender, EventArgs e)
        {
            InternalDispose();
        }

        private void DispatcherShutdownStarted(object sender, EventArgs e)
        {
            //If the dispatcher is shutting down then we will cleanup
            InternalDispose();
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            // Execute Cef.DoMessageLoopWork on the UI Thread
            // Typically this would happen 30/60 times per second (frame rate)
            Cef.DoMessageLoopWork();
        }

        /// <inheritdoc/>
        protected override void OnScheduleMessagePumpWork(long delay)
        {
            // If the delay is greater than the Maximum then use ThirtyTimesPerSecond
            // instead - we do this to achieve a minimum number of FPS
            if (delay > interval)
            {
                delay = interval;
            }

            // When delay <= 0 we'll execute Cef.DoMessageLoopWork immediately
            // if it's greater than we'll just let the Timer which fires 30 times per second
            // care of the call
            if (delay <= 0)
            {
                dispatcher?.InvokeAsync(() => Cef.DoMessageLoopWork(), dispatcherPriority);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InternalDispose();
            }

            base.Dispose(disposing);
        }

        private void InternalDispose()
        {
            if (dispatcher != null)
            {
                dispatcher.ShutdownStarted -= DispatcherShutdownStarted;
                dispatcher = null;
            }

            dispatcherTimer?.Stop();
            dispatcherTimer = null;
        }
    }
}
