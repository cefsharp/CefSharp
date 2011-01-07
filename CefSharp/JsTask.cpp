#include "stdafx.h"
#include "JsTask.h"
#include "Utils.h"

namespace CefSharp
{
    void JsTask::HandleSuccess(CefRefPtr<CefV8Value> result)
    {
        Object^ obj = convertFromCef(result);
        if(obj != nullptr)
        {
            _browser->SetJsResult(obj->ToString());
        }
        else
        {
            _browser->SetJsResult("");
        }
    }

    void JsTask::HandleError()
    {
        _browser->SetJsError();
    }

}