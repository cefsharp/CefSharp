// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using System.Timers;
using CefSharp.Example.Handlers;

namespace CefSharp.WinForms.Example.Handlers
{
    /// <summary>
    /// Integreates CEF into the WinForms message loop, 
    /// This implementation is very simplistic, the timer fires roughly <see cref="BrowserProcessHandler.ThirtyTimesPerSecond"/>
    /// times per second calling Cef.DoMessageLoopWork on the WinForms UI Thread. When OnScheduleMessagePumpWork
    /// is called with a delay of less than or equal to 0 then Cef.DoMessageLoopWork is called as CEF has signaled
    /// that it needs to perform work.
    /// See the following link for the CEF reference implementation that containes a more complex example that maybe
    /// required in some circumstances.
    /// https://bitbucket.org/chromiumembedded/cef/commits/1ff26aa02a656b3bc9f0712591c92849c5909e04?at=2785
    /// </summary>
    public class ScheduleMessagePumpBrowserProcessHandler : BrowserProcessHandler
    {
        private Timer timer;
        private TaskFactory factory;

        public ScheduleMessagePumpBrowserProcessHandler(TaskScheduler scheduler)
        {
            factory = new TaskFactory(scheduler);
            timer = new Timer { Interval = ThirtyTimesPerSecond, AutoReset = true };
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
            if (delay <= 0)
            {
                //Update the timer to execute almost immediately
                factory.StartNew(() => Cef.DoMessageLoopWork());
            }
        }

        public override void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }
    }
}
