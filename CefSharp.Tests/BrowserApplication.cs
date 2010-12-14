namespace CefSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using System.Windows.Forms;
    using System.Threading;

    [SetUpFixture]
    public class BrowserApplication
    {
        public static BrowserApplication Instance { get; private set; }
        public static Form MainForm { get; private set; }
        public static BrowserControl WebBrowser { get; private set; }

        private ManualResetEvent waitCreated;

        [SetUp]
        public void SetUp()
        {
            Instance = this;
            waitCreated = new ManualResetEvent(false);

            Settings settings = new Settings();
            BrowserSettings browserSettings = new BrowserSettings();

            if(!CEF.Initialize(settings, browserSettings))
            {
                Assert.Fail("Couldn't initialise CEF.");
                return;
            }

            // CEF.RegisterScheme("test", new TestSchemeHandlerFactory());
            CEF.RegisterJsObject("bound", new BoundObject());

            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);

            var uiThread = new Thread(UIThread);
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
            waitCreated.WaitOne();
        }

        private void UIThread()
        {
            MainForm = new Form();
            MainForm.Shown += new EventHandler(MainForm_Shown);

            Application.Run(MainForm);
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            WebBrowser = new BrowserControl("http://google.com");
            WebBrowser.Parent = MainForm;
            WebBrowser.Dock = DockStyle.Fill;
            WebBrowser.WaitForLoadCompletion();

            waitCreated.Set();
        }

        [TearDown]
        public void TearDown()
        {
            waitCreated.WaitOne();
            MainForm.Close();
            Application.Exit();
        }
    }
}
