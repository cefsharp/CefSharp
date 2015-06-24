#pragma 

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            void SerializeJsRootObject(JavascriptRootObject ^value, CefRefPtr<CefListValue> list);
            void SerializeJsObject(JavascriptObject ^value, CefRefPtr<CefListValue> list, int index);
        }
    }
}