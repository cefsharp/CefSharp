using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if CEFSHARP_WPF_HWNDHOST
using CefSharp.Wpf.HwndHost;
#endif
namespace CefSharp.Wpf.Example.Controls
{
    public class ChromiumWebBrowser :
#if CEFSHARP_WPF_HWNDHOST
        CefSharp.Wpf.HwndHost.ChromiumWebBrowser
#else
        CefSharp.Wpf.ChromiumWebBrowser
#endif
    {
        public ChromiumWebBrowser(string initialAddress) : base(initialAddress){ }
        public ChromiumWebBrowser() { }
    }
}
