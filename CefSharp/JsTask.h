#include "stdafx.h"
#pragma once

#include "CefWebBrowser.h"

namespace CefSharp
{

    class JsTask : public CefThreadSafeBase<CefV8Task>
    {
        gcroot<CefWebBrowser^> _browser;
        CefString _script;
        CefString _scriptName;
        int _lineNo;
    public:
        JsTask(CefWebBrowser^ browser, CefString script, CefString scriptName, int lineNo)
            : _browser(browser), _script(script), _scriptName(scriptName), _lineNo(lineNo)
        {
        }

        virtual CefString GetScript() { return _script; };
        virtual CefString GetScriptName() { return _scriptName; };
        virtual int GetStartLine() { return _lineNo; };
        virtual void HandleSuccess(CefRefPtr<CefV8Value> result);
        virtual void HandleError();
    };

}