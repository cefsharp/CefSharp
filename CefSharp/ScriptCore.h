// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    private class ScriptCore
    {
    private:
        HANDLE _event;

        gcroot<Object^> _result;
        gcroot<String^> _exceptionMessage;

        bool TryGetMainFrame(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame>& frame);
        void UIT_Execute(CefRefPtr<CefBrowser> browser, CefString script);

    public:
        ScriptCore()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
        }

        DECL void Execute(CefRefPtr<CefBrowser> browser, CefString script);

        IMPLEMENT_LOCKING(ScriptCore);
        IMPLEMENT_REFCOUNTING(ScriptCore);
    };
}