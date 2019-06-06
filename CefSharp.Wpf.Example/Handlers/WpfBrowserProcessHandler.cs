// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Timers;
using System.Windows.Threading;
using CefSharp.Example.Handlers;

namespace CefSharp.Wpf.Example.Handlers
{
    /// <summary>
    /// EXPERIMENTAL - this implementation is very simplistic and not ready for production use
    /// See the following link for the CEF reference implementation.
    /// https://bitbucket.org/chromiumembedded/cef/commits/1ff26aa02a656b3bc9f0712591c92849c5909e04?at=2785
    /// </summary>
    public class WpfBrowserProcessHandler : BrowserProcessHandler
    {
        private Timer timer;
        private Dispatcher dispatcher;

        public WpfBrowserProcessHandler(Dispatcher dispatcher)
        {
            timer = new Timer { Interval = ThirtyTimesPerSecond, AutoReset = true };
            timer.Start();
            timer.Elapsed += TimerTick;

            this.dispatcher = dispatcher;
            this.dispatcher.ShutdownStarted += DispatcherShutdownStarted;
        }

        private void DispatcherShutdownStarted(object sender, EventArgs e)
        {
            //If the dispatcher is shutting down then we stop the timer
            if (timer != null)
            {
                timer.Stop();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            //Basically execute Cef.DoMessageLoopWork 30 times per second, on the UI Thread
            dispatcher.BeginInvoke((Action)(() => Cef.DoMessageLoopWork()), DispatcherPriority.Render);
        }

        protected override void OnScheduleMessagePumpWork(int delay)
        {
            //When delay <= 0 we'll execute Cef.DoMessageLoopWork immediately
            // if it's greater than we'll just let the Timer which fires 30 times per second
            // care of the call
            if (delay <= 0)
            {
                dispatcher.BeginInvoke((Action)(() => Cef.DoMessageLoopWork()), DispatcherPriority.Normal);
            }
        }

        public override void Dispose()
        {
            if (dispatcher != null)
            {
                dispatcher.ShutdownStarted -= DispatcherShutdownStarted;
                dispatcher = null;
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }
    }
}
