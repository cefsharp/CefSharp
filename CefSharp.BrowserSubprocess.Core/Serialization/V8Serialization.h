#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
    namespace Internals
    {
        class JavascriptCallbackRegistry;

        namespace Serialization
        {
            template<typename TList, typename TIndex>
            void SerializeV8Object(CefRefPtr<CefV8Value> value, CefRefPtr<TList> list, TIndex index, CefRefPtr<JavascriptCallbackRegistry> callbackRegistry);

            template<typename TList, typename TIndex>
            CefRefPtr<CefV8Value> DeserializeV8Object(CefRefPtr<TList> list, TIndex index);
        }
    }
}