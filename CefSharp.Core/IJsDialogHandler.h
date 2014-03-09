// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

using namespace System;

namespace CefSharp
{
    public interface class IJsDialogHandler
    {
        bool OnJSAlert(IWebBrowser^ browser, String^ url, String^ message);
        bool OnJSConfirm(IWebBrowser^ browser, String^ url, String^ message, bool& retval);
        bool OnJSPrompt(IWebBrowser^ browser, String^ url, String^ message, String^ defaultValue, bool& retval,  String^% result);
    };
}
