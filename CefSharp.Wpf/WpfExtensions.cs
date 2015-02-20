// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CefSharp.Wpf
{
    public static class WpfExtensions
    {
        public static void DelayUiThreadRunAsync(this Control control, Action action, int milliseconds = 2000, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            var timer = new DispatcherTimer(priority, control.Dispatcher) { Interval = TimeSpan.FromMilliseconds(milliseconds) };
            
            timer.Tick += (sender, args) =>
            {
                timer.Stop();

                action();
            };

            timer.Start();
        }

        public static void UiThreadRunAsync(this Control control, Action action, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            var dispatcher = control.Dispatcher;

            if (dispatcher.CheckAccess())
            {
                action();
            }
            else if (!dispatcher.HasShutdownStarted)
            {
                dispatcher.BeginInvoke(action, priority);
            }
        }
    }
}
