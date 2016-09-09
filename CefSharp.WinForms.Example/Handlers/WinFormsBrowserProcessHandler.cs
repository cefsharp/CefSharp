// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Example.Handlers;
using System.Timers;

namespace CefSharp.WinForms.Example.Handlers
{
    /// <summary>
    /// EXPERIMENTAL - this implementation is very simplistic and not ready for production use
    /// See the following link for the CEF reference implementation.
    /// https://bitbucket.org/chromiumembedded/cef/commits/1ff26aa02a656b3bc9f0712591c92849c5909e04?at=2785
    /// </summary>
    public class WinFormsBrowserProcessHandler : BrowserProcessHandler
    {
        private Timer timer;
        private TaskFactory factory;

        public WinFormsBrowserProcessHandler(TaskScheduler scheduler)
        {
            factory = new TaskFactory(scheduler);
            timer = new Timer { Interval = MaxTimerDelay, AutoReset = true };
            timer.Start();
            timer.Elapsed += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            //Basically execute Cef.DoMessageLoopWork 30 times per second
            //Execute DoMessageLoopWork on UI thread
            factory.StartNew(() => Cef.DoMessageLoopWork());
        }

        protected override void OnScheduleMessagePumpWork(int delay)
        {
            //when delay <= 0 queue the Task up for execution on the UI thread.
            if(delay <= 0)
            {
                //Update the timer to execute almost immediately
                factory.StartNew(() => Cef.DoMessageLoopWork());
            }
        }

        public override void Dispose()
        {
            if(timer != null)
            { 
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }
    }
}
