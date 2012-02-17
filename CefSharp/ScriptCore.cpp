#include "stdafx.h"
#include "cef_runnable.h"
#include "ScriptCore.h"
#include "ScriptException.h"

namespace CefSharp
{
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

    String^ ScriptCore::Evaluate(CefRefPtr<CefFrame> frame, String^ script)
    {
        AutoLock lock_scope(this);

        if (CefCurrentlyOn(TID_UI))
        {
            UIT_Evaluate(frame, toNative(script));
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(this, &ScriptCore::UIT_Evaluate,
                frame, toNative(script)));
            DWORD waitResult = WaitForSingleObject(_event, 2000);
        }

        String^ result = toClr(_result);

        if (!_success)
        {
            throw gcnew ScriptException(result);
        }
        else
        {
            return result;
        }
    }
}