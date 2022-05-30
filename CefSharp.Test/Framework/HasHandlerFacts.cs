using Xunit;

namespace CefSharp.Test.Framework
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class HasHandlerFacts
    {
        [Fact]
        public void ShouldWorkForOffScreen()
        {
            using(var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser:false))
            {
                browser.Paint += OffScreenBrowserPaint;

                Assert.True(browser.HasPaintEventHandler());

                browser.Paint -= OffScreenBrowserPaint;

                Assert.False(browser.HasPaintEventHandler());
            }
        }

        [WpfFact]
        public void ShouldWorkForWpf()
        {
            using (var browser = new CefSharp.Wpf.ChromiumWebBrowser())
            {
                browser.Paint += WpfBrowserPaint;

                Assert.True(browser.HasPaintEventHandler());

                browser.Paint -= WpfBrowserPaint;

                Assert.False(browser.HasPaintEventHandler());
            }
        }

        private void WpfBrowserPaint(object sender, CefSharp.Wpf.PaintEventArgs e)
        {
            
        }

        private void OffScreenBrowserPaint(object sender, CefSharp.OffScreen.OnPaintEventArgs e)
        {
            
        }
    }
}
