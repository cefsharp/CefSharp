using Xunit;

namespace CefSharp.Test.Framework
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class HasHandlerTests
    {
        [Fact]
        public void ShouldWorkForOffScreen()
        {
            using(var browser = new CefSharp.OffScreen.ChromiumWebBrowser(automaticallyCreateBrowser:false))
            {
                browser.Paint += OffScreenBrowserPaint;

                Assert.Equal(1, browser.PaintEventHandlerCount());

                browser.Paint -= OffScreenBrowserPaint;

                Assert.Equal(0, browser.PaintEventHandlerCount());
            }
        }

        [WpfFact]
        public void ShouldWorkForWpf()
        {
            using (var browser = new CefSharp.Wpf.ChromiumWebBrowser())
            {
                browser.Paint += WpfBrowserPaint;

                Assert.Equal(1, browser.PaintEventHandlerCount());

                browser.Paint -= WpfBrowserPaint;

                Assert.Equal(0, browser.PaintEventHandlerCount());
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
