using System;
using NUnit.Framework;

namespace CefSharp.Test
{
    [TestFixture]
    public class BrowserCoreTest
    {
        private BrowserCore browser_core;

        [SetUp]
        public void SetUp()
        {
            browser_core = new BrowserCore(String.Empty);
        }

        [Test]
        public void Test()
        {
            int event_count = 0;
            browser_core.PropertyChanged += (sender, e) => event_count++;

            browser_core.TooltipText = "foo";
            browser_core.TooltipText = null;
            browser_core.TooltipText = null;

            Assert.AreEqual(2, event_count);
        }
    }
}
