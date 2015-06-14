#pragma

#include "include/cef_app.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            JavascriptRootObject^ DeserializeJsRootObject(CefRefPtr<CefListValue> list);
            JavascriptObject^ DeserializeJsObject(CefRefPtr<CefListValue> list, int index);
        }
    }
}