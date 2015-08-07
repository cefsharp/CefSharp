// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
            void SetInt64(const int64 &value, const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            int64 GetInt64(const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsInt64(const CefRefPtr<TList>& list, TIndex index);

            template<typename TList, typename TIndex>
            void SetCefTime(const CefTime &value, const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            CefTime GetCefTime(const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsCefTime(const CefRefPtr<TList>& list, TIndex index);
            
            template<typename TList, typename TIndex>
            void SetJsCallback(JavascriptCallback^ value, const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(const CefRefPtr<TList>& list, TIndex index);
            template<typename TList, typename TIndex>
            bool IsJsCallback(const CefRefPtr<TList>& list, TIndex index);
        }
    }
}