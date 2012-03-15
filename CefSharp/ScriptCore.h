#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public class ScriptCore
    {
    private:
        HANDLE _event;

        gcroot<Object^> _result;
        gcroot<String^> _exception;

        void UIT_Execute(CefRefPtr<CefBrowser> browser, CefString script);
        void UIT_Evaluate(CefRefPtr<CefBrowser> browser, CefString script);

    public:
        ScriptCore()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
        }

        DECL void Execute(CefRefPtr<CefBrowser> browser, CefString script);
        DECL gcroot<Object^> Evaluate(CefRefPtr<CefBrowser> browser, CefString script, double timeout);

        IMPLEMENT_LOCKING(ScriptCore);
        IMPLEMENT_REFCOUNTING(ScriptCore);
    };
}