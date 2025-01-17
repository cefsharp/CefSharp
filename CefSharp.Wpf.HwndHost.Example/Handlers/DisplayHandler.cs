using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CefSharp.Wpf.HwndHost.Example.Handlers
{
    public class DisplayHandler : CefSharp.Handler.DisplayHandler
    {
        private Border parent;
        private Window fullScreenWindow;

        protected override void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            _ = webBrowser.Dispatcher.InvokeAsync(() =>
            {
                if (fullscreen)
                {
                    //In this example the parent is a Border, if your parent is a different type
                    //of control then update this code accordingly.
                    parent = (Border)VisualTreeHelper.GetParent(webBrowser);

                    //NOTE: If the ChromiumWebBrowser instance doesn't have a direct reference to
                    //the DataContext in this case the BrowserTabViewModel then your bindings won't
                    //be updated/might cause issues like the browser reloads the Url when exiting
                    //fullscreen.
                    parent.Child = null;

                    fullScreenWindow = new Window
                    {
                        WindowStyle = WindowStyle.None,
                        WindowState = WindowState.Maximized,
                        Content = webBrowser
                    };
                    fullScreenWindow.Loaded += (_,_) => webBrowser.Focus();

                    fullScreenWindow.ShowDialog();
                }
                else
                {
                    fullScreenWindow.Content = null;

                    parent.Child = webBrowser;

                    fullScreenWindow.Close();
                    fullScreenWindow = null;
                    parent = null;
                }
            });
        }
    }
}
