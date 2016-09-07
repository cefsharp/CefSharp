// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Example.Handlers;
using System.Timers;
using System.Windows.Threading;

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
            timer = new Timer { Interval = MaxTimerDelay, AutoReset = true };
            timer.Start();
            timer.Elapsed += TimerTick;

            this.dispatcher = dispatcher;
            this.dispatcher.ShutdownStarted += DispatcherShutdownStarted;
        }

        private void DispatcherShutdownStarted(object sender, EventArgs e)
        {
            //If the dispatcher is shutting down then we stop the timer
            if(timer != null)
            {
                timer.Stop();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            //Reset the timer interval to maximum 
            timer.Interval = MaxTimerDelay;

            //Execute DoMessageLoopWork on UI thread
            dispatcher.BeginInvoke((Action)(() => Cef.DoMessageLoopWork()));
        }

        protected override void OnScheduleMessagePumpWork(int delay)
        {
            //Basically execute Cef.DoMessageLoopWork 30 times per second,
            //when delay <= 0 set the timer interval to really small so it's executed
            // relatively quickly.
            if(delay <= 0)
            {
                //Update the timer to execute almost immediately
                timer.Interval = 1;
            }
            else
            {
                timer.Interval = delay;
            }
        }

        public override void Dispose()
        {
            if(dispatcher != null)
            {
                dispatcher.ShutdownStarted -= DispatcherShutdownStarted;
                dispatcher = null;
            }

            if(timer != null)
            { 
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }
    }
}