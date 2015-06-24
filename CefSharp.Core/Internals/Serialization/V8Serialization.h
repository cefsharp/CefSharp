#pragma once

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            template<typename TList, typename TIndex>
			Object^ DeserializeV8Object(CefRefPtr<TList> list, TIndex index, IJavascriptCallbackFactory^ callbackFactory);

            template<typename TList, typename TIndex>
            void SerializeV8Object(Object^ obj, CefRefPtr<TList> list, TIndex index);

            DateTime ConvertCefTimeToDateTime(CefTime time);
            CefTime ConvertDateTimeToCefTime(DateTime dateTime);
        }
    }
}