#pragma once

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            template<typename TList, typename TIndex>
            Object^ DeserializeV8Object(CefRefPtr<TList> list, TIndex index);

            DateTime ConvertCefTimeToDateTime(CefTime time);
        }
    }
}