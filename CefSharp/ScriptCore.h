#include "Stdafx.h"
#pragma once

namespace CefSharp
{
    class ScriptCore : public AppDomainSafeCefBase
    {
    private:
        static void _UIT_Evaluate(ScriptCore* const _this, CefRefPtr<CefBrowser> browser, CefString script);
        static gcroot<Object^> _Evaluate(ScriptCore* const _this, CefRefPtr<CefBrowser> browser, CefString script, double timeout);
        static void _Execute(ScriptCore* const _this, CefRefPtr<CefBrowser> browser, CefString script);

    private:
        HANDLE _event;

        gcroot<Object^> _result;
        gcroot<String^> _exceptionMessage;

        static bool TryGetMainFrame(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame>& frame);
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
        IMPLEMENT_SAFE_REFCOUNTING(ScriptCore);
    };
}