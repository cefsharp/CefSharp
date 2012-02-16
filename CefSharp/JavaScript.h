#include "stdafx.h"
#pragma once

#include "cef_runnable.h"

namespace CefSharp
{
    HANDLE evaluateEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
    CefString evaluateResult;

    static void UIT_EvaluateScript(CefRefPtr<CefFrame> frame, CefString script)
    {
        CefRefPtr<CefV8Context> context = frame->GetV8Context();
        CefString url = frame->GetURL();

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

            if (eval->ExecuteFunctionWithContext(context, global, args, result, exception, false) &&
                result.get())
            {
                CefSharp::evaluateResult = result->GetStringValue();
            }

            context->Exit();
        }

        SetEvent(CefSharp::evaluateEvent);
    }

    static String^ EvaluateScript(CefRefPtr<CefFrame> frame, String^ script)
    {
        if (CefCurrentlyOn(TID_UI))
        {
            CefSharp::UIT_EvaluateScript(frame, toNative(script));
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableFunction(&CefSharp::UIT_EvaluateScript,
                frame, toNative(script)));

            DWORD waitResult = WaitForSingleObject(CefSharp::evaluateEvent, INFINITE);
        }

        return toClr(CefSharp::evaluateResult);
    }
}