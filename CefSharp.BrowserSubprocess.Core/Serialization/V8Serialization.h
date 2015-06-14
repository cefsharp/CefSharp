#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
    namespace Internals
    {
        ref class JavascriptCallbackRegistry;

        namespace Serialization
        {
            template<typename TList, typename TIndex>
            void SerializeV8Object(CefRefPtr<CefV8Value> value, CefRefPtr<TList> list, TIndex index, JavascriptCallbackRegistry^ callbackRegistry);
        }
    }
}