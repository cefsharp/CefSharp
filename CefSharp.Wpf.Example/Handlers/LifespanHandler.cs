using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CefSharp.Wpf.Example.Handlers
{
    public class LifespanHandler : ILifeSpanHandler
    {

        Window owner;

        public LifespanHandler(Window owner)
        {
            this.owner = owner;
        }

 
        public bool OnBeforePopup(IWebBrowser browser, IFrame frame, string targetUrl, string popupTitle,
            ref int x, ref int y, ref int width, ref int height, ref bool noJavascriptAccess, ref IWebBrowser newBrowser)
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
                        Title = popupTitle

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
