// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "include/cef_runnable.h"
#include "ScriptCore.h"
#include "ScriptException.h"

namespace CefSharp
{
    bool ScriptCore::TryGetMainFrame(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame>& frame)
    {
        if (browser != nullptr)
        {
            frame = browser->GetMainFrame();
            return frame != nullptr;
        }
        else
        {
            return false;
        }
    }

    void ScriptCore::UIT_Execute(CefRefPtr<CefBrowser> browser, CefString script)
    {
        CefRefPtr<CefFrame> mainFrame;
        if (TryGetMainFrame(browser, mainFrame))
        {
            mainFrame->ExecuteJavaScript(script, "about:blank", 0);
        }
    }

    void ScriptCore::Execute(CefRefPtr<CefBrowser> browser, CefString script)
    {
        if (CefCurrentlyOn(TID_UI))
        {
            UIT_Execute(browser, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(this, &ScriptCore::UIT_Execute, browser, script));
        }
    }
}