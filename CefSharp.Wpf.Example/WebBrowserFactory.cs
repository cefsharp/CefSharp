using CefSharp.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharp.Wpf.Example
{
    public class WebBrowserFactory
    {
        FrameworkElement _parent;
        ILifeSpanHandler _lifespanHandler;

        public WebBrowserFactory(FrameworkElement parent, ILifeSpanHandler lifespanHandler)
        {
            _parent = parent;
            _lifespanHandler = lifespanHandler;
        }

        public ChromiumWebBrowser CreateWebBrowser(string url)
        {

            DataTemplate browserTemplate = _parent.FindResource("WebBrowserTemplate") as DataTemplate;
            ChromiumWebBrowser browser = browserTemplate.LoadContent() as ChromiumWebBrowser;

            browser.Address = url;
            browser.RequestHandler = new RequestHandler();
            browser.RegisterJsObject("bound", new BoundObject());

            browser.MenuHandler = new Handlers.MenuHandler();
            browser.GeolocationHandler = new Handlers.GeolocationHandler();
            browser.DownloadHandler = new DownloadHandler();
            browser.LifeSpanHandler = _lifespanHandler;
            browser.PreviewTextInput += (o, e) =>
            {
                foreach (var character in e.Text)
                {
                    browser.SendKeyEvent((int)WM.CHAR, character, 0);
                }

                e.Handled = true;
            };

            CefExample.RegisterTestResources(browser);

            return browser;
        }

    }
}
