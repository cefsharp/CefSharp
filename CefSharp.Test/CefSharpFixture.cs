// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test
{
    public class CefSharpFixture : IAsyncLifetime, IDisposable
    {
        private readonly BlockingCollection<Action> queue;
        private readonly Thread thread;
        private readonly CancellationTokenSource cts;

        public CefSharpFixture()
        {
            queue = new BlockingCollection<Action>();
            cts = new CancellationTokenSource();

            using (var resetEvent = new ManualResetEventSlim())
            {
                thread = new Thread(Run);
                thread.Start(resetEvent);

                resetEvent.Wait();
            }
        }

        private void CefInitialize()
        {
            if (!Cef.IsInitialized)
            {
                var isDefault = AppDomain.CurrentDomain.IsDefaultAppDomain();
                if (!isDefault)
                {
                    throw new Exception(@"Add <add key=""xunit.appDomain"" value=""denied""/> to your app.config to disable appdomains");
                }

                var settings = new CefSettings();

                //The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
                //HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
                settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\Cache");

                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
        }

        private void CefShutdown()
        {
            if (Cef.IsInitialized)
            {
                Cef.Shutdown();
            }
        }

        public Task InitializeAsync()
        {
            return Post(CefInitialize);
        }

        public Task DisposeAsync()
        {
            return Post(CefShutdown);
        }

        public void Dispose()
        {
            queue.CompleteAdding();

            cts.Cancel();
            
            if (!thread.Join(TimeSpan.FromSeconds(5)))
            {
                thread.Abort();
            }

            queue.Dispose();
        }

        private void Run(object state)
        {
            ((ManualResetEventSlim)state).Set();

            try
            {
                foreach (var action in queue.GetConsumingEnumerable(cts.Token))
                {
                    action();
                }
            }
            catch (OperationCanceledException) { }
        }

        private Task Post(Action action)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!queue.TryAdd(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }))
            {
                tcs.SetCanceled();
            }

            return tcs.Task;
        }
    }
}
