// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            //Functions to serialize/deserialize specific types into CefBinaryValue

            template<typename TList, typename TIndex>
            void SetInt64(const CefRefPtr<TList>& list, TIndex index, const int64 &value);
            template<typename TList, typename TIndex>
            int64 GetInt64(const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsInt64(const CefRefPtr<TList>& list, TIndex index);

            template<typename TList, typename TIndex>
            void SetCefTime(const CefRefPtr<TList>& list, TIndex index, const CefTime &value);
            template<typename TList, typename TIndex>
            CefTime GetCefTime(const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsCefTime(const CefRefPtr<TList>& list, TIndex index);

            template<typename TList, typename TIndex>
            void SetJsCallback(const CefRefPtr<TList>& list, TIndex index, JavascriptCallback^ value);
            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsJsCallback(const CefRefPtr<TList>& list, TIndex index);

            template void SetInt64(const CefRefPtr<CefListValue>& list, int index, const int64 &value);
            template void SetInt64(const CefRefPtr<CefListValue>& list, size_t index, const int64 &value);
            template void SetInt64(const CefRefPtr<CefDictionaryValue>& list, CefString index, const int64 &value);
            template int64 GetInt64(const CefRefPtr<CefListValue>& list, int index);
            template int64 GetInt64(const CefRefPtr<CefListValue>& list, size_t index);
            template int64 GetInt64(const CefRefPtr<CefDictionaryValue>& list, CefString index);
            template bool IsInt64(const CefRefPtr<CefListValue>& list, int index);
            template bool IsInt64(const CefRefPtr<CefListValue>& list, size_t index);
            template bool IsInt64(const CefRefPtr<CefDictionaryValue>& list, CefString index);

            template void SetCefTime(const CefRefPtr<CefListValue>& list, int index, const CefTime &value);
            template void SetCefTime(const CefRefPtr<CefListValue>& list, size_t index, const CefTime &value);
            template void SetCefTime(const CefRefPtr<CefDictionaryValue>& list, CefString index, const CefTime &value);
            template CefTime GetCefTime(const CefRefPtr<CefListValue>& list, int index);
            template CefTime GetCefTime(const CefRefPtr<CefListValue>& list, size_t index);
            template CefTime GetCefTime(const CefRefPtr<CefDictionaryValue>& list, CefString index);
            template bool IsCefTime(const CefRefPtr<CefListValue>& list, size_t index);
            template bool IsCefTime(const CefRefPtr<CefListValue>& list, int index);
            template bool IsCefTime(const CefRefPtr<CefDictionaryValue>& list, CefString index);

            template void SetJsCallback(const CefRefPtr<CefListValue>& list, int index, JavascriptCallback^ value);
            template void SetJsCallback(const CefRefPtr<CefListValue>& list, size_t index, JavascriptCallback^ value);
            template void SetJsCallback(const CefRefPtr<CefDictionaryValue>& list, CefString index, JavascriptCallback^ value);
            template JavascriptCallback^ GetJsCallback(const CefRefPtr<CefListValue>& list, int index);
            template JavascriptCallback^ GetJsCallback(const CefRefPtr<CefListValue>& list, size_t index);
            template JavascriptCallback^ GetJsCallback(const CefRefPtr<CefDictionaryValue>& list, CefString index);
            template bool IsJsCallback(const CefRefPtr<CefListValue>& list, int index);
            template bool IsJsCallback(const CefRefPtr<CefListValue>& list, size_t index);
            template bool IsJsCallback(const CefRefPtr<CefDictionaryValue>& list, CefString index);
        }
    }
}