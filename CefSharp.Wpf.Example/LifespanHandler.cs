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

        MainWindow m_owner;

        public LifespanHandler(MainWindow owner)
        {
            m_owner = owner;
        }

        // The list of browsers created via popup
        List<IWebBrowser> _popupBrowsersPending = new List<IWebBrowser>();
        List<IWebBrowser> _popupBrowsersCreated = new List<IWebBrowser>();

        public bool OnBeforePopup(IWebBrowser browser, IFrame frame, string targetUrl, ref int x, ref int y, ref int width, ref int height, ref bool noJavascriptAccess, ref IWebBrowser newBrowser)
        {
            ChromiumWebBrowser newChromiumBrowser = null;

            m_owner.Dispatcher.Invoke(() => 
                {
                    newChromiumBrowser = m_owner.BrowserFactory.CreateWebBrowser(targetUrl);
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

                m_owner.Dispatcher.Invoke(() =>
                {
                    BrowserTabViewModel vm = new BrowserTabViewModel(browser.Address)
                    {
                        ShowSidebar = false,
                        WebBrowser = browser as ChromiumWebBrowser
                    };

                    m_owner.BrowserTabs.Add(vm);
                    m_owner.TabControl.SelectedIndex = m_owner.BrowserTabs.Count - 1;
                });

                _popupBrowsersCreated.Add(browser);

            }
  
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
            if (_popupBrowsersCreated.Contains(browser))
            {
                m_owner.Dispatcher.Invoke(() =>
                {
                    BrowserTabViewModel toClose =
                        m_owner.BrowserTabs
                            .Where(vm => object.ReferenceEquals(vm.WebBrowser, browser))
                            .FirstOrDefault();
                    if (toClose != null)
                        m_owner.BrowserTabs.Remove(toClose);
                    browser.Dispose();
                });

                _popupBrowsersCreated.Remove(browser);
            }
        }
    }
}
