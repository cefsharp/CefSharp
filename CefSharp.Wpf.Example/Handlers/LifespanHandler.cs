using System.Windows;

namespace CefSharp.Wpf.Example.Handlers
{
    public class LifespanHandler : ILifeSpanHandler
    {
        Window owner;

        public LifespanHandler(Window owner)
        {
            this.owner = owner;
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string windowTitle, ref int x, ref int y, ref int width, ref int height, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            ChromiumWebBrowser chromiumBrowser = null;

            double windowX = (x == int.MinValue) ? double.NaN : (double)x;
            double windowY = (y == int.MinValue) ? double.NaN : (double)y;
            double windowWidth = (width == int.MinValue) ? double.NaN : (double)width;
            double windowHeight = (height == int.MinValue) ? double.NaN : (double)height;

            owner.Dispatcher.Invoke(() =>
                {
                    chromiumBrowser = new ChromiumWebBrowser()
                    {
                        Address = targetUrl,
                    };

                    Window popup = new Window()
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
                        Window w = o as Window;
                        if (w != null && w.Content is IWebBrowser)
                        {
                            (w.Content as IWebBrowser).Dispose();
                            w.Content = null;
                        }
                    };

                    chromiumBrowser.LifeSpanHandler = new LifespanHandler(popup);
                });

           

            newBrowser = chromiumBrowser;

            

            return false;
        }

        public void OnAfterCreated(IWebBrowser browser)
        {
            owner.Dispatcher.Invoke(() =>
                {
                    if (owner != null && owner.Content == browser && !(owner is MainWindow))
                    {
                        owner.Show();
                    }
                });
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
            owner.Dispatcher.Invoke(() =>
                {

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
