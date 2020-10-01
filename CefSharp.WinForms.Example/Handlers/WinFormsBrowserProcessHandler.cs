// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using CefSharp.Example.Handlers;

namespace CefSharp.WinForms.Example.Handlers
{
    /// <summary>
    /// Minimal integration of CEF into existing message loop
    /// The timer fires roughly <see cref="BrowserProcessHandler.SixtyTimesPerSecond"/>
    /// times per second calling  Cef.DoMessageLoopWork on the WinForms UI Thread.
    /// See the following link for the CEF reference implementation.
    /// https://bitbucket.org/chromiumembedded/cef/commits/1ff26aa02a656b3bc9f0712591c92849c5909e04?at=2785
    /// </summary>
    public class WinFormsBrowserProcessHandler : BrowserProcessHandler
    {
        private Timer timer;

        public WinFormsBrowserProcessHandler(IContainer components)
        {
            timer = new Timer(components) { Interval = SixtyTimesPerSecond };
            timer.Start();
            timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            //Basically execute Cef.DoMessageLoopWork 30 times per second
            Cef.DoMessageLoopWork();
        }

        protected override void OnScheduleMessagePumpWork(int delay)
        {
            //NOOP - Only enabled when CefSettings.ExternalMessagePump
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
