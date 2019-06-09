// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// ConcurrentMethodRunnerQueue - Async Javascript Binding methods are run
    /// on the ThreadPool in parallel, when a method returns a Task
    /// the we use ContinueWith to be notified of completion then
    /// raise the MethodInvocationComplete event
    /// </summary>
    public class ConcurrentMethodRunnerQueue : IMethodRunnerQueue
    {
        private readonly JavascriptObjectRepository repository;
        private readonly AutoResetEvent stopped = new AutoResetEvent(false);
        private readonly BlockingCollection<Func<MethodInvocationResult>> queue = new BlockingCollection<Func<MethodInvocationResult>>();
        private readonly object lockObject = new object();
        private volatile CancellationTokenSource cancellationTokenSource;
        private volatile bool running;

        public event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        public ConcurrentMethodRunnerQueue(JavascriptObjectRepository repository)
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
            queue.Add(() => ExecuteMethodInvocation(methodInvocation));
        }

        private void ConsumeTasks()
        {
            try
            {
                //Executes tasks on the ThreadPool
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var func = queue.Take(cancellationTokenSource.Token);

                    var task = new Task(() =>
                    {
                        var result = func();

                        //If the call failed or returned null then we'll fire the event immediately
                        if (!result.Success || result.Result == null)
                        {
                            OnMethodInvocationComplete(result);
                        }
                        else
                        {
                            var resultType = result.Result.GetType();

                            //If the returned type is Task then we'll ContinueWith and perform the processing then.
                            if (typeof(Task).IsAssignableFrom(resultType))
                            {
                                var resultTask = (Task)result.Result;

                                if (resultType.IsGenericType)
                                {
                                    resultTask.ContinueWith((t) =>
                                    {
                                        if (t.Status == TaskStatus.RanToCompletion)
                                        {
                                            //We use some reflection to get the Result
                                            //If someone has a better way of doing this then please submit a PR
                                            result.Result = resultType.GetProperty("Result").GetValue(resultTask);
                                        }
                                        else
                                        {
                                            result.Success = false;
                                            result.Result = null;
                                            var aggregateException = t.Exception;
                                            //TODO: Add support for passing a more complex message
                                            // to better represent the Exception
                                            if (aggregateException.InnerExceptions.Count == 1)
                                            {
                                                result.Message = aggregateException.InnerExceptions[0].ToString();
                                            }
                                            else
                                            {
                                                result.Message = t.Exception.ToString();
                                            }
                                        }

                                        OnMethodInvocationComplete(result);
                                    },
                                    cancellationTokenSource.Token, TaskContinuationOptions.None, TaskScheduler.Default);
                                }
                                else
                                {
                                    //If it's not a generic Task then it doesn't have a return object
                                    //So we'll just set the result to null and continue on
                                    result.Result = null;

                                    OnMethodInvocationComplete(result);
                                }
                            }
                            else
                            {
                                OnMethodInvocationComplete(result);
                            }
                        }

                    }, cancellationTokenSource.Token);

                    task.Start(TaskScheduler.Default);
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
            MethodInvocationComplete?.Invoke(this, new MethodInvocationCompleteArgs(e));
        }
    }
}
