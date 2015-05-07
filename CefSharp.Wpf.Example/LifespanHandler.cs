using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CefSharp.Wpf.Example
{
    public class LifespanHandler : ILifeSpanHandler
    {

        MainWindow _owner;

        public LifespanHandler(MainWindow owner)
        {
            _owner = owner;
        }

        // The list of browsers created via popup
        List<IWebBrowser> _popupBrowsersPending = new List<IWebBrowser>();
        List<IWebBrowser> _popupBrowsersCreated = new List<IWebBrowser>();

        public bool OnBeforePopup(IWebBrowser browser, IFrame frame, string targetUrl, ref int x, ref int y, ref int width, ref int height, ref bool noJavascriptAccess, ref IWebBrowser newBrowser)
        {
            ChromiumWebBrowser newChromiumBrowser = null;

            _owner.Dispatcher.Invoke(() => 
                {
                    newChromiumBrowser = _owner.BrowserFactory.CreateWebBrowser(targetUrl);
                });

            _popupBrowsersPending.Add(newChromiumBrowser);
            
            newBrowser = newChromiumBrowser;
            return false;
        }


        public void OnAfterCreated(IWebBrowser browser)
        {
            if (_popupBrowsersPending.Contains(browser))
            {
                _popupBrowsersPending.Remove(browser);

                _owner.Dispatcher.Invoke(() =>
                {
                    BrowserTabViewModel vm = new BrowserTabViewModel(browser.Address)
                    {
                        ShowSidebar = false,
                    };

                    vm.WebBrowser = browser as ChromiumWebBrowser;
                    _owner.BrowserTabs.Add(vm);
                    _owner.TabControl.SelectedIndex = _owner.BrowserTabs.Count - 1;
                });

                _popupBrowsersCreated.Add(browser);

            }
  
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
            if (_popupBrowsersCreated.Contains(browser))
            {
                _owner.Dispatcher.Invoke(() =>
                {
                    BrowserTabViewModel toClose =
                        _owner.BrowserTabs
                            .Where(vm => object.ReferenceEquals(vm.WebBrowser, browser))
                            .FirstOrDefault();
                    if (toClose != null)
                        _owner.BrowserTabs.Remove(toClose);
                    browser.Dispose();
                });

                _popupBrowsersCreated.Remove(browser);
            }
        }
    }
}
