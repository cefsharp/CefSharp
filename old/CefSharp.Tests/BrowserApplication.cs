namespace CefSharp.Tests
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using NUnit.Framework;
    using System.IO;
    using System.Text;

    [SetUpFixture]
    public class BrowserApplication : IBeforeResourceLoad
    {
        public static BrowserApplication Instance { get; private set; }
        public static Form MainForm { get; private set; }
        public static CefFormsWebBrowser WebBrowser { get; private set; }

        private ManualResetEvent waitCreated;

        [SetUp]
        public void SetUp()
        {
            Instance = this;
            waitCreated = new ManualResetEvent(false);

            Settings settings = new Settings();

            if(!CEF.Initialize(settings))
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
            WebBrowser = new CefFormsWebBrowser("http://google.com", new BrowserSettings());
            WebBrowser.Parent = MainForm;
            WebBrowser.Dock = DockStyle.Fill;
            WebBrowser.WaitForLoadCompletion();
            WebBrowser.BeforeResourceLoadHandler = this;

            waitCreated.Set();
        }

        [TearDown]
        public void TearDown()
        {
            waitCreated.WaitOne();
            MainForm.Close();
            Application.Exit();
        }

        #region IBeforeResourceLoad Members
        private const string testPagesBaseUrl = "http://testpages/";

        public static string GetTestPagesUrl(string url)
        {
            return testPagesBaseUrl + "/" + url;
        }

        public static string GetTestPagesFilePath(string url)
        {
            if (url.StartsWith("/") || url.StartsWith("\\")) url = "." + url;
            var path = Path.Combine(".", url);
            return path;
        }

        private Stream GetStream(string url)
        {
            var path = GetTestPagesFilePath(url);

            Stream result = null;
            try
            {
                result = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                result = new MemoryStream(Encoding.UTF8.GetBytes(
                    string.Format("<h1>ERROR!</h1><p>Error getting of shell resource page {0}!</p><p>{1}</p>", url, ex.ToString())
                    ));
            }

            return result;
        }

        public void HandleBeforeResourceLoad(ICefWebBrowser browserControl, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            if (request.Url.StartsWith(testPagesBaseUrl))
            {
                var url = request.Url.Substring(testPagesBaseUrl.Length);
                var stream = GetStream(url);
                requestResponse.RespondWith(stream, "text/html");
            }
        }
        #endregion
    }
}
