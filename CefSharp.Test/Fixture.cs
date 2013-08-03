using System.Threading;
using NUnit.Framework;

namespace CefSharp.Test
{
    [SetUpFixture]
    public class Fixture
    {
        //public static WebView Browser { get; private set; }

        private ManualResetEvent createdEvent = new ManualResetEvent(false);

        [SetUp]
        public void SetUp()
        {
            //var settings = new Settings();

            //var cef = new Cef();
            //cef.Initialize(settings);

            //    var thread = new Thread(() =>
            //    {
            //        var form = new Form();
            //        form.Shown += (sender, e) =>
            //        {
            //            Browser = new WebView
            //            {
            //                Parent = form,
            //                Dock = DockStyle.Fill,
            //            };

            //            createdEvent.Set();
            //        };

            //        Application.Run(form);
            //    });
            //    thread.SetApartmentState(ApartmentState.STA);
            //    thread.Start();

            //    createdEvent.WaitOne();
        }

        [TearDown]
        public void TearDown()
        {
            //createdEvent.WaitOne();
            //CEF.Shutdown();
            //Application.Exit();
        }
    }
}
