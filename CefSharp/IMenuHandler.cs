using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public interface IMenuHandler
    {
        bool OnBeforeContextMenu(IWebBrowser browser);
    }
}
