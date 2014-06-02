using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CefSharp.Test
{
    public class DispatcherThread : DisposableResource
    {
        public DispatcherThread()
        {
            startedEvent = new ManualResetEvent(false);
            thread = new Thread(Run);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "DispatcherThread";
            thread.Start();

            startedEvent.WaitOne();
            startedEvent.Dispose();
            startedEvent = null;
        }

        private void Run()
        {
            frame = new DispatcherFrame(true);

            dispatcher = Dispatcher.CurrentDispatcher;

            Action action = () =>
            {
                TaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                startedEvent.Set();
            };

            dispatcher.BeginInvoke(action);

            Dispatcher.PushFrame(frame);
        }

        protected override void DoDispose(bool isDisposing)
        {
            if (frame != null)
            {
                frame.Continue = false;
                frame = null;
            }

            dispatcher.BeginInvokeShutdown( DispatcherPriority.Normal );

            if (!thread.Join(TimeSpan.FromSeconds(5)))
            {
                thread.Abort();

                thread.Interrupt();
            }

            thread = null;

            base.DoDispose(isDisposing);
        }

        private DispatcherFrame frame;
        private ManualResetEvent startedEvent;
        private Thread thread;
        private Dispatcher dispatcher;

        public TaskFactory TaskFactory { get; private set; }
    }
}
