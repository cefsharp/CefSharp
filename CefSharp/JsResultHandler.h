#include "stdafx.h"

#pragma once

namespace CefSharp 
{

    class JsResultHandler : public CefThreadSafeBase<CefV8Handler>
    {
        const static char* DONE_FUNC_NAME;
        const static char* ERR_FUNC_NAME;

        gcroot<BrowserControl^> _browserControl;
        
        JsResultHandler(BrowserControl^ browserControl) : _browserControl(browserControl) {}

    public:
        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments,
                            CefRefPtr<CefV8Value>& retval, CefString& exception)
        {
            if(name == DONE_FUNC_NAME || ERR_FUNC_NAME)
            {
                if(arguments.size() != 1) 
                {
                    exception = "argument count mismatch, expected 1";
                    return false;
                }

                CefRefPtr<CefV8Value> arg = arguments[0];
                CefString result = "";
                if(arg != NULL)
                {
                    result = arg->GetStringValue();                    
                }

                if(name == DONE_FUNC_NAME) 
                {
                    _browserControl->SetJsResult(result);                    
                }
                else 
                {
                    _browserControl->SetJsError(result);
                }
                return true;
            }
            else 
            {
                exception = "JsResultHandler: unrecognised function name";
                return false;
            }
        }
        
        static void Bind(BrowserControl^ browserControl, CefRefPtr<CefV8Value> domWindow)
        {
            CefRefPtr<JsResultHandler> handler = new JsResultHandler(browserControl);
         
            CefRefPtr<CefV8Value> doneFunction = CefV8Value::CreateFunction(DONE_FUNC_NAME, static_cast<CefRefPtr<CefV8Handler>>(handler));
            domWindow->SetValue(DONE_FUNC_NAME, doneFunction);

            CefRefPtr<CefV8Value> errFunction = CefV8Value::CreateFunction(ERR_FUNC_NAME, static_cast<CefRefPtr<CefV8Handler>>(handler));
            domWindow->SetValue(ERR_FUNC_NAME, errFunction);
        }
    };

    const char* JsResultHandler::DONE_FUNC_NAME = "__js_run_done";
    const char* JsResultHandler::ERR_FUNC_NAME = "__js_run_err";
}