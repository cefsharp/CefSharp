// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
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
        private readonly IJavascriptObjectRepositoryInternal repository;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        public ConcurrentMethodRunnerQueue(IJavascriptObjectRepositoryInternal repository)
        {
            this.repository = repository;
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
        }

        public void Enqueue(MethodInvocation methodInvocation)
        {
            var task = new Task(async () =>
            {
                var result = await ExecuteMethodInvocation(methodInvocation).ConfigureAwait(false);

                //If the call failed or returned null then we'll fire the event immediately
                if (!result.Success || result.Result == null)
                {
                    OnMethodInvocationComplete(result, cancellationTokenSource.Token);
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
                            //Discard the continuation as we rely on 
                            //OnMethodInvocationComplete to send the response
                            //to the render process.
                            _ = resultTask.ContinueWith((t) =>
                            {
                                if (t.Status == TaskStatus.RanToCompletion)
                                {
                                    //TODO: Use resultTask.GetAwaiter().GetResult() instead
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

                                OnMethodInvocationComplete(result, cancellationTokenSource.Token);
                            },
                            cancellationTokenSource.Token, TaskContinuationOptions.None, TaskScheduler.Default);
                        }
                        else
                        {
                            //If it's not a generic Task then it doesn't have a return object
                            //So we'll just set the result to null and continue on
                            result.Result = null;

                            OnMethodInvocationComplete(result, cancellationTokenSource.Token);
                        }
                    }
                    else
                    {
                        OnMethodInvocationComplete(result, cancellationTokenSource.Token);
                    }
                }

            }, cancellationTokenSource.Token);

            task.Start(TaskScheduler.Default);
        }

        private async Task<MethodInvocationResult> ExecuteMethodInvocation(MethodInvocation methodInvocation)
        {
            object returnValue = null;
            string exception;
            var success = false;
            var nameConverter = repository.NameConverter;

            //make sure we don't throw exceptions in the executor task
            try
            {
                var result = await repository.TryCallMethodAsync(methodInvocation.ObjectId, methodInvocation.MethodName, methodInvocation.Parameters.ToArray()).ConfigureAwait(false);

                success = result.Success;
                returnValue = result.ReturnValue;
                exception = result.Exception;
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
                Result = returnValue,
                Success = success,
                NameConverter = nameConverter
            };
        }

        private void OnMethodInvocationComplete(MethodInvocationResult e, CancellationToken token)
        {
            //If cancellation has been requested we don't need to continue.
            if (!token.IsCancellationRequested)
            {
                MethodInvocationComplete?.Invoke(this, new MethodInvocationCompleteArgs(e));
            }
        }
    }
}
