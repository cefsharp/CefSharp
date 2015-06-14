#pragma once

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            template<typename TList, typename TIndex>
            void SetInt64(const int64 &value, CefRefPtr<TList> list, TIndex index);
            template<typename TList, typename TIndex>
            int64 GetInt64(CefRefPtr<TList> list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsInt64(CefRefPtr<TList> list, TIndex index);

            template<typename TList, typename TIndex>
            void SetCefTime(const CefTime &value, CefRefPtr<TList> list, TIndex index);
            template<typename TList, typename TIndex>
            CefTime GetCefTime(CefRefPtr<TList> list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsCefTime(CefRefPtr<TList> list, TIndex index);

            template<typename TList, typename TIndex>
            void SetJsCallback(JavascriptCallback^ value, CefRefPtr<TList> list, TIndex index);
            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(CefRefPtr<TList> list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsJsCallback(CefRefPtr<TList> list, TIndex index);
        }
    }
}