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

        void UIT_Evaluate(CefRefPtr<CefFrame> frame, CefString script);

    public:
        ScriptCore()
        {
            _event = CreateEvent(NULL, FALSE, FALSE, NULL);
        }

        String^ Evaluate(CefRefPtr<CefFrame> frame, String^ script);

        IMPLEMENT_LOCKING(ScriptCore);
        IMPLEMENT_REFCOUNTING(ScriptCore);
    };
}