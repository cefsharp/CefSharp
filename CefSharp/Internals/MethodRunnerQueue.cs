// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public sealed class MethodRunnerQueue
    {
        private readonly JavascriptObjectRepository repository;
        private readonly AutoResetEvent stopped = new AutoResetEvent(false);
        private readonly BlockingCollection<Task<MethodInvocationResult>> queue = new BlockingCollection<Task<MethodInvocationResult>>();
        private readonly object lockObject = new object();
        private volatile CancellationTokenSource cancellationTokenSource;
        private volatile bool running;

        public event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        public MethodRunnerQueue(JavascriptObjectRepository repository)
        {
            this.repository = repository;
        }

        public void Start()
        {
            lock (lockObject)
            {
                if (!running)
                {
                    cancellationTokenSource = new CancellationTokenSource();
                    Task.Factory.StartNew(ConsumeTasks, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    running = true;
                }
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                if (running)
                {
                    cancellationTokenSource.Cancel();
                    stopped.WaitOne();
                    //clear the queue
                    while (queue.Count > 0)
                    {
                        queue.Take();
                    }
                    cancellationTokenSource = null;
                    running = false;
                }
            }
        }

        public void Enqueue(MethodInvocation methodInvocation)
        {
            var task = new Task<MethodInvocationResult>(() => ExecuteMethodInvocation(methodInvocation));
            queue.Add(task);
        }

        private void ConsumeTasks()
        {
            try
            {

                if (CefSharpSettings.ConcurrentTaskExecution)
                {
                    //New experimental behaviour that Starts the Tasks on TaskScheduler.Default 
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        var task = queue.Take(cancellationTokenSource.Token);
                        task.ContinueWith((t) =>
                        {
                            OnMethodInvocationComplete(t.Result);
                        }, cancellationTokenSource.Token);
                        task.Start(TaskScheduler.Default);
                    }
                }
                else
                {
                    //Old behaviour, runs Tasks in sequential order on the current Thread.
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        var task = queue.Take(cancellationTokenSource.Token);
                        task.RunSynchronously();
                        OnMethodInvocationComplete(task.Result);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Note: Task has been cancelled
            }
            finally
            {
                stopped.Set();
            }
        }

        private MethodInvocationResult ExecuteMethodInvocation(MethodInvocation methodInvocation)
        {
            object result = null;
            string exception;
            var success = false;

            //make sure we don't throw exceptions in the executor task
            try
            {
                success = repository.TryCallMethod(methodInvocation.ObjectId, methodInvocation.MethodName, methodInvocation.Parameters.ToArray(), out result, out exception);
            }
            catch (Exception e)
            {
                exception = e.Message;
            }

            return new MethodInvocationResult
            {
                BrowserId = methodInvocation.BrowserId,
                CallbackId = methodInvocation.CallbackId,
                FrameId = methodInvocation.FrameId,
                Message = exception,
                Result = result,
                Success = success
            };
        }

        private void OnMethodInvocationComplete(MethodInvocationResult e)
        {
            var handler = MethodInvocationComplete;
            if (handler != null)
            {
                handler(this, new MethodInvocationCompleteArgs(e));
            }
        }
    }
}
