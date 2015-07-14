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
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly BlockingCollection<Task<MethodInvocationResult>> queue = new BlockingCollection<Task<MethodInvocationResult>>();

        public event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        public MethodRunnerQueue(JavascriptObjectRepository repository)
        {
            this.repository = repository;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var task = queue.Take(cancellationTokenSource.Token);
                    task.RunSynchronously(TaskScheduler.Current);
                    OnMethodInvocationComplete(task.Result);
                }
                stopped.Set();
            }, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            stopped.WaitOne();
        }

        public void Enqueue(MethodInvocation methodInvocation)
        {
            var task = new Task<MethodInvocationResult>(() =>
            {
                object result;
                string exception;
                var success = repository.TryCallMethod(methodInvocation.ObjectId, methodInvocation.MethodName, methodInvocation.Parameters.ToArray(), out result, out exception);
                return new MethodInvocationResult
                {
                    CallbackId = methodInvocation.CallbackId,
                    Message = exception,
                    Result = result,
                    Success = success
                };
            });
            queue.Add(task);
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
