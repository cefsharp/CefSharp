#pragma once

using namespace System;

namespace CefSharp
{
    public interface class IJsDialogHandler
    {
        public:
            bool OnJSAlert(IWebBrowser^ browser, String^ url, String^ message);
            bool OnJSConfirm(IWebBrowser^ browser, String^ url, String^ message, bool& retval);
            bool OnJSPrompt(IWebBrowser^ browser, String^ url, String^ message, String^ defaultValue, bool& retval,  String^% result);
   };
}
