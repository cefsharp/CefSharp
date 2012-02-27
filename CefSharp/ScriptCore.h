#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public class ScriptCore
    {
    private:
        HANDLE _event;

        gcroot<Object^> _result;
        gcroot<Exception^> _exception;

        void UIT_Execute(CefRefPtr<CefFrame> frame, CefString script);
        void UIT_Evaluate(CefRefPtr<CefFrame> frame, CefString script);

    public:
        ScriptCore()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
        }

        DECL void Execute(CefRefPtr<CefFrame> frame, CefString script);
        DECL gcroot<Object^> Evaluate(CefRefPtr<CefFrame> frame, CefString script, double timeout);

        IMPLEMENT_LOCKING(ScriptCore);
        IMPLEMENT_REFCOUNTING(ScriptCore);
    };
}