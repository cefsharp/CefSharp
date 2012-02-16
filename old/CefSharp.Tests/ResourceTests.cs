namespace CefSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using System.IO;

    [TestFixture]
    public class ResourceTests
    {
        private CefFormsWebBrowser WebBrowser { get { return BrowserApplication.WebBrowser; } }

        [Test]
        public void StreamClosingWithBeforeResourceLoad()
        {
            WebBrowser.Load(BrowserApplication.GetTestPagesUrl("TestPage.html"));
            WebBrowser.WaitForLoadCompletion();
            WebBrowser.Load("about:blank");
            WebBrowser.WaitForLoadCompletion();
            
            // Uncomment this to pass test.
            // GC.Collect();

            // now we will try open TestPage.html file for writing
            try
            {
                var file = File.Open(BrowserApplication.GetTestPagesFilePath("TestPage.html"), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                file.Close();
            }
            catch (IOException ioex)
            {
                Assert.Fail("File couldn't be opened. So underlying stream for file is not closed.\nException: {0}", ioex.ToString());
            }
            catch
            {
                throw;
            }
        }
    }
}
