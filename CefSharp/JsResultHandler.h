#include "stdafx.h"
#pragma once

namespace CefSharp 
{
    ref class CefWebBrowser;

    class JsResultHandler : public CefThreadSafeBase<CefV8Handler>
    {
        const static char* DONE_FUNC_NAME;
        const static char* ERR_FUNC_NAME;

        gcroot<CefWebBrowser^> _browserControl;
        
        JsResultHandler(CefWebBrowser^ browserControl) : _browserControl(browserControl) {}

    public:
        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception);
        
        static void Bind(CefWebBrowser^ browserControl, CefRefPtr<CefV8Value> domWindow);
    };
}