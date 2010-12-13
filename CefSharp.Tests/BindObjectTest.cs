namespace CefSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using NUnit.Framework;

    [TestFixture]
    public class BindObjectTest
    {
        private BrowserControl WebBrowser { get { return BrowserApplication.WebBrowser; } }

        [Test]
        public void Test1()
        {
            WebBrowser.Load("http://google.com/");
            WebBrowser.WaitForLoadCompletion();
        }

        [Test]
        public void Test2()
        {
            WebBrowser.Load("http://ya.ru/");
            WebBrowser.WaitForLoadCompletion();
        }

        [Test]
        public void Test3()
        {
            WebBrowser.Load("http://microsoft.com/");
            WebBrowser.WaitForLoadCompletion();
        }

    }
}
