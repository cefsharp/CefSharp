// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "V8Serialization.h"
#include "JavascriptCallbackRegistry.h"
#include "../CefSharp.Core.Runtime/Internals/Serialization/Primitives.h"

#include <deque>

using namespace std;

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            typedef deque<CefRefPtr<CefV8Value>> value_deque;

            template<typename TList, typename TIndex>
            void SerializeV8Object(const CefRefPtr<CefV8Value> &obj, const CefRefPtr<TList>& list, const TIndex& index, JavascriptCallbackRegistry^ callbackRegistry, value_deque &seen)
            {
                for (value_deque::const_iterator it = seen.begin(); it != seen.end(); ++it)
                {
                    if (obj->IsSame(*it))
                    {
                        throw exception("Cycle found");
                    }
                }
                seen.push_back(obj);

                if (obj->IsNull() || obj->IsUndefined())
                {
                    list->SetNull(index);
                }
                else if (obj->IsBool())
                {
                    list->SetBool(index, obj->GetBoolValue());
                }
                else if (obj->IsInt())
                {
                    list->SetInt(index, obj->GetIntValue());
                }
                else if (obj->IsDouble())
                {
                    list->SetDouble(index, obj->GetDoubleValue());
                }
                else if (obj->IsString())
                {
                    list->SetString(index, obj->GetStringValue());
                }
                else if (obj->IsDate())
                {
                    SetCefTime(list, index, obj->GetDateValue());
                }
                else if (obj->IsArray())
                {
                    int arrLength = obj->GetArrayLength();
                    std::vector<CefString> keys;
                    auto array = CefListValue::Create();
                    if (arrLength > 0 && obj->GetKeys(keys))
                    {
                        for (int i = 0; i < arrLength; i++)
                        {
                            SerializeV8Object(obj->GetValue(keys[i]), array, i, callbackRegistry, seen);
                        }

                    }

                    list->SetList(index, array);
                }
                else if (obj->IsFunction())
                {
                    auto context = CefV8Context::GetCurrentContext();
                    auto jsCallback = callbackRegistry->Register(context, obj);
                    SetJsCallback(list, index, jsCallback);
                }
                else if (obj->IsObject())
                {
                    std::vector<CefString> keys;
                    if (obj->GetKeys(keys) && keys.size() > 0)
                    {
                        auto result = CefDictionaryValue::Create();
                        for (size_t i = 0; i < keys.size(); i++)
                        {
                            auto p_keyStr = StringUtils::ToClr(keys[i].ToString());
                            if ((obj->HasValue(keys[i])) && (!p_keyStr->StartsWith("__")))
                            {
                                SerializeV8Object(obj->GetValue(keys[i]), result, keys[i], callbackRegistry, seen);
                            }
                        }
                        list->SetDictionary(index, result);
                    }
                }
                else
                {
                    list->SetNull(index);
                }
                seen.pop_back();
            }

            template<typename TList, typename TIndex>
            void SerializeV8Object(const CefRefPtr<CefV8Value> &obj, const CefRefPtr<TList>& list, const TIndex& index, JavascriptCallbackRegistry^ callbackRegistry)
            {
                try
                {
                    value_deque seen;
                    SerializeV8Object(obj, list, index, callbackRegistry, seen);
                }
                catch (const exception&)
                {
                    list->SetNull(index);
                }
            }

            template<typename TList, typename TIndex>
            CefRefPtr<CefV8Value> DeserializeV8Object(const CefRefPtr<TList>& list, const TIndex& index)
            {
                auto type = list->GetType(index);

                if (type == VTYPE_BOOL)
                {
                    return CefV8Value::CreateBool(list->GetBool(index));
                }

                if (type == VTYPE_INT)
                {
                    return CefV8Value::CreateInt(list->GetInt(index));
                }

                if (type == VTYPE_DOUBLE)
                {
                    return CefV8Value::CreateDouble(list->GetDouble(index));
                }

                if (type == VTYPE_STRING)
                {
                    return CefV8Value::CreateString(list->GetString(index));
                }

                if (IsCefTime(list, index))
                {
                    return CefV8Value::CreateDate(GetCefTime(list, index));
                }

                if (type == VTYPE_LIST)
                {
                    auto subList = list->GetList(index);
                    size_t size = subList->GetSize();
                    auto result = CefV8Value::CreateArray(size);
                    for (size_t i = 0; i < size; i++)
                    {
                        result->SetValue(i, DeserializeV8Object(subList, i));
                    }

                    return result;
                }

                if (type == VTYPE_DICTIONARY)
                {
                    auto subDict = list->GetDictionary(index);
                    auto size = subDict->GetSize();
                    std::vector<CefString> keys;
                    subDict->GetKeys(keys);
                    auto result = CefV8Value::CreateObject(nullptr, nullptr);
                    for (size_t i = 0; i < size; i++)
                    {
                        result->SetValue(keys[i], DeserializeV8Object(subDict, keys[i]), V8_PROPERTY_ATTRIBUTE_NONE);
                    }

                    return result;
                }

                return CefV8Value::CreateNull();
            }

            template void SerializeV8Object(const CefRefPtr<CefV8Value> &value, const CefRefPtr<CefListValue>& list, const int& index, JavascriptCallbackRegistry^ callbackRegistry);
            template void SerializeV8Object(const CefRefPtr<CefV8Value> &value, const CefRefPtr<CefListValue>& list, const size_t& index, JavascriptCallbackRegistry^ callbackRegistry);
            template void SerializeV8Object(const CefRefPtr<CefV8Value> &value, const CefRefPtr<CefDictionaryValue>& list, const CefString& index, JavascriptCallbackRegistry^ callbackRegistry);
            template void SerializeV8Object(const CefRefPtr<CefV8Value> &value, const CefRefPtr<CefListValue>& list, const size_t& index, JavascriptCallbackRegistry^ callbackRegistry, value_deque &visited);
            template void SerializeV8Object(const CefRefPtr<CefV8Value> &value, const CefRefPtr<CefDictionaryValue>& list, const CefString& index, JavascriptCallbackRegistry^ callbackRegistry, value_deque &visited);
            template CefRefPtr<CefV8Value> DeserializeV8Object(const CefRefPtr<CefListValue>& list, const int& index);
            template CefRefPtr<CefV8Value> DeserializeV8Object(const CefRefPtr<CefDictionaryValue>& list, const CefString& index);
        }
    }
}
