using System;
using NUnit.Framework;

namespace CefSharp.WinForms.Test
{
    [TestFixture]
    public class InitTest
    {
        private WebView web_view;

        [SetUp]
        public void SetUp()
        {
            var settings = new Settings();
            if (!CEF.Initialize(settings))
            {
                Assert.Fail();
            }

            web_view = new WebView();
        }

        [Test]
        public void Foo()
        {
            Assert.Pass();
        }
    }
}