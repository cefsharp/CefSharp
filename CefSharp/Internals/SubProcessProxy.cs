using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class SubProcessProxy : ObjectBase, ISubProcessProxy
    {
        public ISubProcessCallback callback { get; private set; }

        public void Initialize()
        {
            CefSubprocessBase.Instance.ServiceHost.Service = this;
            callback = OperationContext.Current.GetCallbackChannel<ISubProcessCallback>();
        }

        public IAsyncResult BeginEvaluateScript(long frameId, string script, AsyncCallback callback, object state )
        {
            var task = CefSubprocessBase.Instance.Browser.EvaluateScript(frameId, script);

            return ToBegin( task, callback, state );
        }

        public object EndEvaluateScript( IAsyncResult r )
        {
            return ToEnd<object>( r ); 
        }

        public void Terminate()
        {
            CefSubprocessBase.Instance.ServiceHost.Service = null;
            CefSubprocessBase.Instance.Dispose();
        }

        /// <summary>
        /// Wraps a <see cref="Task{TResult}"/> into the Begin method of an APM pattern.
        /// </summary>
        /// <param name="task">The task to wrap. May not be <c>null</c>.</param>
        /// <param name="callback">The callback method passed into the Begin method of the APM pattern.</param>
        /// <param name="state">The state passed into the Begin method of the APM pattern.</param>
        /// <returns>The asynchronous operation, to be returned by the Begin method of the APM pattern.</returns>
        private static IAsyncResult ToBegin<TResult>(Task<TResult> task, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<TResult>(state);
            task.ContinueWith(t =>
            {
                tcs.TryCompleteFromCompletedTask(t);

                if (callback != null)
                    callback(tcs.Task);
            }, TaskScheduler.Default);

            return tcs.Task;
        }

        /// <summary>
        /// Wraps a <see cref="Task{TResult}"/> into the End method of an APM pattern.
        /// </summary>
        /// <param name="asyncResult">The asynchronous operation returned by the matching Begin method of this APM pattern.</param>
        /// <returns>The result of the asynchronous operation, to be returned by the End method of the APM pattern.</returns>
        private static TResult ToEnd<TResult>(IAsyncResult asyncResult)
        {
            return ((Task<TResult>)asyncResult).Result;
        }

    }

    public static class TaskExtensions
    {
        /// <summary>
        /// Attempts to complete a <see cref="TaskCompletionSource{TResult}"/>, propagating the completion of <paramref name="task"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the target asynchronous operation.</typeparam>
        /// <typeparam name="TSourceResult">The type of the result of the source asynchronous operation.</typeparam>
        /// <param name="this">The task completion source. May not be <c>null</c>.</param>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        /// <returns><c>true</c> if this method completed the task completion source; <c>false</c> if it was already completed.</returns>
        public static bool TryCompleteFromCompletedTask<TResult, TSourceResult>(this TaskCompletionSource<TResult> @this, Task<TSourceResult> task) where TSourceResult : TResult
        {
            if (task.IsFaulted)
                return @this.TrySetException(task.Exception.InnerExceptions);
            if (task.IsCanceled)
                return @this.TrySetCanceled();
            return @this.TrySetResult(task.Result);
        }


    }
}
