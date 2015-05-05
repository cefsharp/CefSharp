using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharp.Wpf.Example
{
    public class LifespanHandler : ILifeSpanHandler
    {
        BrowserTabView _tabView;

        public LifespanHandler(BrowserTabView tabView)
        {
            _tabView = tabView;
        }

        public bool OnBeforePopup(IWebBrowser browser, IFrame frame, string targetUrl, ref int x, ref int y, ref int width, ref int height, ref IWebBrowser newBrowser)
        {
            BrowserTabViewModel newVm = null;
            IWebBrowser wb = null;

            _tabView.Dispatcher.Invoke(() =>
                {
                    Window w = new Window();
                    wb = new ChromiumWebBrowser()
                    {
                        Address = targetUrl
                    };
                    w.Content = wb;
                    w.Show();

                });

                    /*
                    BrowserTabViewModel vm = _tabView.DataContext as BrowserTabViewModel;
                    if (vm != null)
                    {
                        newVm = vm.RaisePopupRequest(targetUrl);
                    }
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);


            if (newVm != null)
            {
                _tabView.Dispatcher.Invoke(() => wb = newVm.WebBrowser);
            }
                    */


            if (wb != null)
                newBrowser = wb;
            return false;
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
        }
    }
}
