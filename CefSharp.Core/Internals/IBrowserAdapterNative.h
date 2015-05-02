#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace Internals
    {
        public interface struct IBrowserAdapterNative : IBrowserAdapter
        {
            virtual Task<JavascriptResponse^>^ EvaluateScriptAsync(const CefRefPtr<CefFrame>& frame, String^ script, Nullable<TimeSpan> timeout);
        };
    }
}
