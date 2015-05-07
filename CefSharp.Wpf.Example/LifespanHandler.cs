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
        DependencyObject _tabView;
        Window _w;

        public LifespanHandler(DependencyObject tabView)
        {
            _tabView = tabView;
        }

        public void OnAfterCreated(IWebBrowser browser)
        {
            _tabView.Dispatcher.Invoke(() =>
            {
                ChromiumWebBrowser wb = browser as ChromiumWebBrowser;
                if (wb != null && wb.Parent == null)
                {
                    _w = new Window();
                    _w.Content = wb;
                    _w.Show();
                }
            });
        }

        public bool OnBeforePopup(IWebBrowser browser, IFrame frame, string targetUrl, ref int x, ref int y, ref int width, ref int height, ref IWebBrowser newBrowser)
        {
            BrowserTabViewModel newVm = null;
            IWebBrowser wb = null;

            _tabView.Dispatcher.Invoke(() =>
                {
                    
                    wb = new ChromiumWebBrowser()
                    {
                        Address = targetUrl
                    };
                    wb.LifeSpanHandler = new LifespanHandler(_tabView);
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
            if (_w != null)
            {
                _w.Dispatcher.Invoke(() =>
                    {
                        (_w.Content as ChromiumWebBrowser).Dispose();
                        _w.Close();
                    });
            }
        }
    }
}
