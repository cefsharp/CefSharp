/// https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/
/// 
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.OffScreen.Example
{
    public static class AsyncContext
    {
        public static void Run(Func<Task> func)
        {
            var prevCtx = SynchronizationContext.Current;

            try
            {
                var syncCtx = new SingleThreadSynchronizationContext();

                SynchronizationContext.SetSynchronizationContext(syncCtx);

                var t = func();

                t.ContinueWith(delegate
                {
                    syncCtx.Complete();
                }, TaskScheduler.Default);

                syncCtx.RunOnCurrentThread();

                t.GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevCtx);
            }
        }
    }
}
