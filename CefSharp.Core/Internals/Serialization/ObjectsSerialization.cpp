// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "ObjectsSerialization.h"
#include "Primitives.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            template<typename TList, typename TIndex>
            Object^ DeserializeObject(const CefRefPtr<TList>& list, TIndex index, IJavascriptCallbackFactory^ javascriptCallbackFactory)
            {
                Object^ result = nullptr;
                auto type = list->GetType(index);

                if (type == VTYPE_BOOL)
                {
                    result = list->GetBool(index);
                }
                else if (type == VTYPE_INT)
                {
                    result = list->GetInt(index);
                }
                else if (IsInt64(list, index))
                {
                    result = GetInt64(list, index);
                }
                else if (IsCefTime(list, index))
                {
                    auto cefTime = GetCefTime(list, index);
                    result = ConvertCefTimeToDateTime(cefTime);
                }
                else if (IsJsCallback(list, index) && javascriptCallbackFactory != nullptr)
                {
                    auto jsCallbackDto = GetJsCallback(list, index);
                    result = javascriptCallbackFactory->Create(jsCallbackDto);
                }
                else if (type == VTYPE_DOUBLE)
                {
                    result = list->GetDouble(index);
                }
                else if (type == VTYPE_STRING)
                {
                    result = StringUtils::ToClr(list->GetString(index));
                }
                else if (type == VTYPE_LIST)
                {
                    auto subList = list->GetList(index);
                    auto array = gcnew List<Object^>(subList->GetSize());
                    for (auto i = 0; i < subList->GetSize(); i++)
                    {
                        array->Add(DeserializeObject(subList, i, javascriptCallbackFactory));
                    }
                    result = array;
                }
                else if (type == VTYPE_DICTIONARY)
                {
                    auto dict = gcnew Dictionary<String^, Object^>();
                    auto subDict = list->GetDictionary(index);
                    std::vector<CefString> keys;
                    subDict->GetKeys(keys);

                    for (auto i = 0; i < keys.size(); i++)
                    {
                        dict->Add(StringUtils::ToClr(keys[i]), DeserializeObject(subDict, keys[i], javascriptCallbackFactory));
                    }

                    result = dict;
                }

                return result;
            }

            DateTime ConvertCefTimeToDateTime(CefTime time)
            {
                auto epoch = time.GetDoubleT();
                if (epoch == 0)
                {
                    return DateTime::MinValue;
                }
                return DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime();
            }
        }
    }
}