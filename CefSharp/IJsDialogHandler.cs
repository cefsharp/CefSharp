using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public interface IJsDialogHandler
    {
        bool OnJSAlert(IWebBrowser browser, string url, string message);
        bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval);
        bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result);
    }
}
