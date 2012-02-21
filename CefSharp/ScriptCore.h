#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public class ScriptCore
    {
    private:
        HANDLE _event;

        bool _success;
        CefString _result;

        void UIT_Execute(CefRefPtr<CefFrame> frame, CefString script);
        void UIT_Evaluate(CefRefPtr<CefFrame> frame, CefString script);

    public:
        ScriptCore()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
        }

        void Execute(CefRefPtr<CefFrame> frame, String^ script);
        String^ Evaluate(CefRefPtr<CefFrame> frame, String^ script, TimeSpan timeout);

        IMPLEMENT_LOCKING(ScriptCore);
        IMPLEMENT_REFCOUNTING(ScriptCore);
    };
}