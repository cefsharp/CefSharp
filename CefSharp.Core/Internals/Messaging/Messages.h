#pragma once

#include "include/cef_base.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            const CefString kJsRootObject = "JavascriptRootObject";
            const CefString kEvaluateJavascript = "EvaluateJavascript";
            const CefString kEvaluateJavascriptDone = "EvaluateJavascriptDone";
        }
    }
}