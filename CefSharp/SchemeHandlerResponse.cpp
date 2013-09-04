#include "Stdafx.h"
#include "SchemeHandlerWrapper.h"

namespace CefSharp
{
    void SchemeHandlerResponse::OnRequestCompleted()
    {
        _schemeHandlerWrapper->ProcessRequestCallback(this);
    }
}