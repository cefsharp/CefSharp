using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public interface ILifeSpanHandler
    {
        bool OnBeforePopup(IWebBrowser browser, string url, ref int x, ref int y, ref int width, ref int height);
        void OnBeforeClose(IWebBrowser browser);
    }
}
