// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Windows;
using CefSharp.Wpf.HwndHost.Handler;

namespace CefSharp.Wpf.HwndHost.Example
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if !NETCOREAPP3_1_OR_GREATER
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            //Example of setting a command line argument
            //Enables WebRTC
            // - CEF Doesn't currently support permissions on a per browser basis see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access
            // - CEF Doesn't currently support displaying a UI for media access permissions
            //
            //NOTE: WebRTC Device Id's aren't persisted as they are in Chrome see https://bitbucket.org/chromiumembedded/cef/issues/2064/persist-webrtc-deviceids-across-restart
            settings.CefCommandLineArgs.Add("enable-media-stream");
            //https://peter.sh/experiments/chromium-command-line-switches/#use-fake-ui-for-media-stream
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            //For screen sharing add (see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access#comment-58677180)
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

            //See https://github.com/cefsharp/CefSharp/wiki/General-Usage#multithreadedmessageloop
            //The default is true
            const bool multiThreadedMessageLoop = true;

            IBrowserProcessHandler browserProcessHandler = null;

            if(!multiThreadedMessageLoop)
            {
                settings.MultiThreadedMessageLoop = false;
                browserProcessHandler = new IntegratedMessageLoopBrowserProcessHandler(Dispatcher);
            }

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: browserProcessHandler);

            base.OnStartup(e);
        }
    }
}
