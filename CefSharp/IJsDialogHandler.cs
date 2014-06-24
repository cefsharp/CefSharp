// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IJsDialogHandler
    {
        bool OnJSAlert(IWebBrowser browser, string url, string message);
        bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval);
        bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result);
    }
}
