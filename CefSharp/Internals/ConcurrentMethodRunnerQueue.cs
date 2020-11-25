// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
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
                    if (result.Result is Task)
                    {
                        
                        var builder = new System.Text.StringBuilder();
                        builder.AppendLine("Your method returned a Task which can cause deadlocks.");
                        builder.AppendLine("Please attach a interceptor marked as async and properly await your results.");
                        result.Success = false;
                        result.Result = null;
                        result.Message = builder.ToString();
                        OnMethodInvocationComplete(result, cancellationTokenSource.Token);
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
            object result = null;
            string exception;
            var success = false;
            var nameConverter = repository.NameConverter;

            //make sure we don't throw exceptions in the executor task
            try
            {
                var value = await repository.TryCallMethod(methodInvocation.ObjectId, methodInvocation.MethodName, methodInvocation.Parameters.ToArray()).ConfigureAwait(false);

                success = value.Item1;
                result = value.Item2;
                exception = value.Item3;
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
