using System.Windows;

namespace CefSharp.Wpf.Example.Handlers
{
    public class LifespanHandler : ILifeSpanHandler
    {
        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string windowTitle, ref int x, ref int y, ref int width, ref int height, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            ChromiumWebBrowser chromiumBrowser = null;

            var windowX = (x == int.MinValue) ? double.NaN : x;
            var windowY = (y == int.MinValue) ? double.NaN : y;
            var windowWidth = (width == int.MinValue) ? double.NaN : width;
            var windowHeight = (height == int.MinValue) ? double.NaN : height;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);
                chromiumBrowser = new ChromiumWebBrowser
                {
                    Address = targetUrl,
                };

                var popup = new Window
                {
                    Left = windowX,
                    Top = windowY,
                    Width = windowWidth,
                    Height = windowHeight,
                    Content = chromiumBrowser,
                    Owner = owner,
                    Title = windowTitle
                };

                popup.Closed += (o, e) => 
                {
                    var w = o as Window;
                    if (w != null && w.Content is IWebBrowser)
                    {
                        (w.Content as IWebBrowser).Dispose();
                        w.Content = null;
                    }
                };

                chromiumBrowser.LifeSpanHandler = new LifespanHandler();
            });

            newBrowser = chromiumBrowser;

            return false;
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser browser)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browser;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);

                if (owner != null && owner.Content == browser && !(owner is MainWindow))
                {
                    owner.Show();
                }
            });
        }

        void ILifeSpanHandler.OnBeforeClose(IWebBrowser browser)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browser;

            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);
                if (owner != null && owner.Content == browser)
                {
                    if (!(owner is MainWindow))
                    {
                        owner.Close();
                    }                        
                }
            });
        }
    }
}
