// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public sealed class MethodRunnerQueue : IMethodRunnerQueue
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
                //Tasks are run in sequential order on the current Thread.
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var task = queue.Take(cancellationTokenSource.Token);
                    task.RunSynchronously();
                    OnMethodInvocationComplete(task.Result);
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

                //We don't support Tasks by default
                if (success && result != null && (typeof(Task).IsAssignableFrom(result.GetType())))
                {
                    //Use StringBuilder to improve the formatting/readability of the error message
                    //I'm sure there's a better way I just cannot remember of the top of my head so going
                    //with this for now, as it's only for error scenaiors I'm not concerned about performance.
                    var builder = new System.Text.StringBuilder();
                    builder.AppendLine("Your method returned a Task which is not supported by default you must set CefSharpSettings.ConcurrentTaskExecution = true; before creating your first ChromiumWebBrowser instance.");
                    builder.AppendLine("This will likely change to the default at some point in the near future, subscribe to the issue link below to be notified of any changes.");
                    builder.AppendLine("See https://github.com/cefsharp/CefSharp/issues/2758 for more details, please report any issues you have there, make sure you have an example ready that reproduces your problem.");

                    success = false;
                    result = null;
                    exception = builder.ToString();
                }

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
