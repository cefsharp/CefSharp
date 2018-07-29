using CefSharp.Structs;
using System;
using System.Windows;

namespace CefSharp.Wpf.Example.Views
{
    internal class CustomChromiumWebBrowser : ChromiumWebBrowser
    {
        private Window _parent;
        public CustomChromiumWebBrowser()
        {
            Loaded += HandleLoaded;
            Unloaded += HandleUnloaded;
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            if(_parent != null)
            {
                _parent.LocationChanged -= NotifyScreenInfoChanged;
            }
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            _parent = Window.GetWindow(this);
            _parent.LocationChanged += NotifyScreenInfoChanged;
        }

        private void NotifyScreenInfoChanged(object sender, EventArgs args)
        {
            this.GetBrowserHost().NotifyScreenInfoChanged();
        }

        protected override ScreenInfo? GetScreenInfo()
        {
            var siNullable = base.GetScreenInfo();
            if(!siNullable.HasValue)
            {
                return null;
            }

            var screenInfo = siNullable.Value;

            Dispatcher.Invoke(() =>
            {
                MonitorInfo.RectStruct monitor;
                MonitorInfo.RectStruct available;
                MonitorInfo.GetMonitorInfoForVisual(this, out monitor, out available);

                screenInfo.AvailableTop = available.Top;
                screenInfo.AvailableLeft = available.Left;
                screenInfo.AvailableHeight = available.Bottom - available.Top;
                screenInfo.AvailableWidth = available.Right - available.Left;

                screenInfo.Height = monitor.Bottom - monitor.Top;
                screenInfo.Width = monitor.Right - monitor.Bottom;
            });

            return screenInfo;
        }
    }
}
