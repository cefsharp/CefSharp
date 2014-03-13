using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace CefSharp.Test
{
    public class TestWindow : Window
    {
        public TestWindow()
        {
            Content = WebView = new WebView();
        }

        public WebView WebView { get; set; }
    }

    public class TestWindowWrapper : ObjectBase
    {
        public TestWindow Window { get; set; }

        private Thread _thread;
        private DispatcherFrame _frame;


        public TestWindowWrapper()
        {
            using ( var evt = new ManualResetEvent(false) )
            {
                _thread = new Thread(() =>
                {
                    _frame = new DispatcherFrame(true);
                    Action set = () => evt.Set();
                    Dispatcher.CurrentDispatcher.BeginInvoke(set);

                    Dispatcher.PushFrame(_frame);
                });
                _thread.Name = "UI";
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();
                evt.WaitOne();
            }

            using ( var evt = new ManualResetEvent( false ) )
            {
                Action init = () =>
                {
                    Window = new TestWindow();
                    Window.WebView.IsVisibleChanged += ( o, e ) => 
                    {
                        if ( !evt.SafeWaitHandle.IsClosed && !evt.SafeWaitHandle.IsInvalid )
                        {
                            evt.Set();
                        }
                    }; 

                    Window.Show();

                    Window.ToString();
                };

                _frame.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
                _frame.Dispatcher.Invoke(init);  
                
                evt.WaitOne(); 
            }      
        }

        void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }

        protected override void DoDispose(bool isDisposing)
        {
            if ( _frame != null )
            {
                if ( Window != null )
                {
                    Action cleanup = () =>
                    {
                        Window.Close();
                        Window = null;
                    };

                    _frame.Dispatcher.Invoke(cleanup); 
                }
                _frame.Continue = false;
                _frame = null;
            }

            if ( _thread != null )
            {
                _thread.Join();
                _thread = null; 
            }

            base.DoDispose(isDisposing);
        }
    }
}
