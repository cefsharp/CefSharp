// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Internals.Tasks;

namespace CefSharp.Internals
{
    public sealed class MethodRunnerQueue : IMethodRunnerQueue
    {
        //Limit to 1 task per methodRunnerQueue
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/d0bcb415-fb1e-42e4-90f8-c43a088537fb/aborting-a-long-running-task-in-tpl?forum=parallelextensions
        private readonly LimitedConcurrencyLevelTaskScheduler taskScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
        private readonly IJavascriptObjectRepositoryInternal repository;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        public MethodRunnerQueue(IJavascriptObjectRepositoryInternal repository)
        {
            this.repository = repository;
        }

        public void Dispose()
        {
            //Cancel all tasks associated with this MethoRunnerQueue
            cancellationTokenSource.Cancel();
        }

        public void Enqueue(MethodInvocation methodInvocation)
        {
            Task.Factory.StartNew(() =>
            {
                var result = ExecuteMethodInvocation(methodInvocation);

                var handler = MethodInvocationComplete;
                if (!cancellationTokenSource.Token.IsCancellationRequested && handler != null)
                {
                    handler(this, new MethodInvocationCompleteArgs(result));
                }
            }, cancellationTokenSource.Token, TaskCreationOptions.HideScheduler, taskScheduler);
        }

        private MethodInvocationResult ExecuteMethodInvocation(MethodInvocation methodInvocation)
        {
            object returnValue = null;
            string exception;
            var success = false;
            var nameConverter = repository.NameConverter;

            //make sure we don't throw exceptions in the executor task
            try
            {
                var result = repository.TryCallMethod(methodInvocation.ObjectId, methodInvocation.MethodName, methodInvocation.Parameters.ToArray());
                success = result.Success;
                returnValue = result.ReturnValue;
                exception = result.Exception;

                //We don't support Tasks by default
                if (success && returnValue != null && (typeof(Task).IsAssignableFrom(returnValue.GetType())))
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
                Result = returnValue,
                Success = success,
                NameConverter = nameConverter
            };
        }
    }
}
