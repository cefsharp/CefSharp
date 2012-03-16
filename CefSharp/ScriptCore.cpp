#include "stdafx.h"
#include "include/cef_runnable.h"
#include "ScriptCore.h"
#include "ScriptException.h"

namespace CefSharp
{
    void ScriptCore::UIT_Execute(CefRefPtr<CefBrowser> browser, CefString script)
    {
        browser->GetMainFrame()->ExecuteJavaScript(script, "about:blank", 0);
    }

    void ScriptCore::UIT_Evaluate(CefRefPtr<CefBrowser> browser, CefString script)
    {
        CefRefPtr<CefV8Context> context = browser->GetMainFrame()->GetV8Context();

        if (context.get() &&
            context->Enter())
        {
            CefRefPtr<CefV8Value> global = context->GetGlobal();
            CefRefPtr<CefV8Value> eval = global->GetValue("eval");
            CefRefPtr<CefV8Value> arg = CefV8Value::CreateString(script);
            CefRefPtr<CefV8Value> result;
            CefRefPtr<CefV8Exception> exception;

            CefV8ValueList args;
            args.push_back(arg);

            if (eval->ExecuteFunctionWithContext(context, global, args,
                result, exception, false))
            {
                if (exception)
                {
                    CefString message = exception->GetMessage();
                    _exception = toClr(message);
                }
                else
                {
                    try
                    {
                        _result = convertFromCef(result);
                    }
                    catch (Exception^ ex)
                    {
                        _exception = ex->Message;
                    }
                }
            }

            context->Exit();
        }

        SetEvent(_event);
    }

    void ScriptCore::Execute(CefRefPtr<CefBrowser> browser, CefString script)
    {
        if (CefCurrentlyOn(TID_UI))
        {
            UIT_Execute(browser, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(this, &ScriptCore::UIT_Execute,
                browser, script));
        }
    }

    gcroot<Object^> ScriptCore::Evaluate(CefRefPtr<CefBrowser> browser, CefString script, double timeout)
    {
        AutoLock lock_scope(this);
        _result = nullptr;
        _exception = nullptr;

        if (CefCurrentlyOn(TID_UI))
        {
            UIT_Evaluate(browser, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(this, &ScriptCore::UIT_Evaluate,
                browser, script));

            switch (WaitForSingleObject(_event, timeout))
            {
            case WAIT_TIMEOUT:
                throw gcnew ScriptException("Script timed out");
            case WAIT_ABANDONED:
            case WAIT_FAILED:
                throw gcnew ScriptException("Script error");
            }
        }

        if (_exception)
        {
            throw gcnew ScriptException(_exception);
        }
        else
        {
            return _result;
        }
    }
}