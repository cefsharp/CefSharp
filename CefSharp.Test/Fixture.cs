// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CefSharp.Wpf;

namespace CefSharp.Test
{
    public class Fixture : DisposableResource
    {
        public ChromiumWebBrowser Browser { get; set; }
        public Window Window { get; set; }
        public DispatcherThread DispatcherThread { get; set; }


        public Fixture()
        {
            DispatcherThread = new DispatcherThread();
        }

        public Task Initialize()
        {
            return DoInUi(() =>
            {
                Window = new Window();

                var cefsettings = new CefSettings
                {
                    BrowserSubprocessPath = Path.Combine(Environment.CurrentDirectory, "CefSharp.BrowserSubprocess.exe"),
                    LogSeverity = LogSeverity.Verbose,
                    LocalesDirPath = Path.Combine(Environment.CurrentDirectory, "locales"),
                    PackLoadingDisabled = true
                };

                if (!Cef.Initialize(cefsettings))
                {
                    
                }

                Window.Content = Browser = new ChromiumWebBrowser();

                Window.Show();
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    throw t.Exception;
                }
                using (var evt = new ManualResetEvent(false))
                {
                    Browser.IsBrowserInitializedChanged += (o, e) => evt.Set();

                    DoInUi(() =>
                    {
                        if (Browser.IsBrowserInitialized)
                        {
                            evt.Set();
                        }
                    });

                    evt.WaitOne();
                }
            }, DispatcherThread.TaskFactory.Scheduler);
        }

        public Task DoInUi(Action action)
        {
            return DispatcherThread.TaskFactory.StartNew(action);
        }

        public Task<T> DoInUi<T>(Func<T> action)
        {
            return DispatcherThread.TaskFactory.StartNew(action);
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            if (DispatcherThread != null)
            {
                DispatcherThread.Dispose();
                DispatcherThread = null;
            }
            Browser = null;
            base.DoDispose(isDisposing);
        }
    }
}
