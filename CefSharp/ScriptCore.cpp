#include "stdafx.h"
#include "cef_runnable.h"
#include "ScriptCore.h"
#include "ScriptException.h"

namespace CefSharp
{
    void ScriptCore::UIT_Execute(CefRefPtr<CefFrame> frame, CefString script)
    {
        frame->ExecuteJavaScript(script, "about:blank", 0);
    }

    void ScriptCore::UIT_Evaluate(CefRefPtr<CefFrame> frame, CefString script)
    {
        CefRefPtr<CefV8Context> context = frame->GetV8Context();

        if (context.get() &&
            context->Enter())
        {
            CefRefPtr<CefV8Value> global = context->GetGlobal();
            CefRefPtr<CefV8Value> eval = global->GetValue("eval");
            CefRefPtr<CefV8Value> arg = CefV8Value::CreateString(script);

            CefV8ValueList args;
            args.push_back(arg);

            CefRefPtr<CefV8Value> result;
            CefRefPtr<CefV8Exception> exception;

            if (eval->ExecuteFunctionWithContext(context, global, args, result, exception, false))
            {
                if (exception.get())
                {
                    _success = false;
                    _result = exception->GetMessage();
                }
                else if (result.get())
                {
                    _success = true;
                    _result = result->GetStringValue();
                }
            }

            context->Exit();
        }

        SetEvent(_event);
    }

    void ScriptCore::Execute(CefRefPtr<CefFrame> frame, CefString script)
    {
        if (CefCurrentlyOn(TID_UI))
        {
            UIT_Execute(frame, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(this, &ScriptCore::UIT_Execute,
                frame, script));
        }
    }

    CefString ScriptCore::Evaluate(CefRefPtr<CefFrame> frame, CefString script, int timeout)
    {
        AutoLock lock_scope(this);

        if (CefCurrentlyOn(TID_UI))
        {
            UIT_Evaluate(frame, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(this, &ScriptCore::UIT_Evaluate,
                frame, script));

            if (WaitForSingleObject(_event, timeout))
            {
                throw gcnew ScriptException("Script timed out");
            }
        }

        if (!_success)
        {
            throw gcnew ScriptException(toClr(_result));
        }
        else
        {
            return _result;
        }
    }
}