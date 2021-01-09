// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

using namespace CefSharp::JavascriptBinding;

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            //Functions to serialize/deserialize data returned/sent from/to the render process.

            //Serializes data into a given position in a CefListValue or CefDictionaryValue
            template<typename TList, typename TIndex>
            void SerializeV8Object(const CefRefPtr<TList>& list, const TIndex& index, Object^ obj, IJavascriptNameConverter^ nameConverter);

            template<typename TList, typename TIndex>
            void SerializeV8SimpleObject(const CefRefPtr<TList>& list, const TIndex& index, Object^ obj, HashSet<Object^>^ seen, IJavascriptNameConverter^ nameConverter);

            template void SerializeV8Object(const CefRefPtr<CefListValue>& list, const int& index, Object^ obj, IJavascriptNameConverter^ nameConverter);
            template void SerializeV8Object(const CefRefPtr<CefListValue>& list, const size_t& index, Object^ obj, IJavascriptNameConverter^ nameConverter);
            template void SerializeV8Object(const CefRefPtr<CefDictionaryValue>& list, const CefString& index, Object^ obj, IJavascriptNameConverter^ nameConverter);
        }
    }
}