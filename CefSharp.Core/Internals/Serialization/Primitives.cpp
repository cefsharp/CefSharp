// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Primitives.h"

#include "include/cef_app.h"

using namespace std;

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            enum class PrimitiveType : unsigned char
            {
                INT64,
                CEFTIME,
                JSCALLBACK
            };

            template<typename TList, typename TIndex>
            bool IsType(PrimitiveType type, const CefRefPtr<TList>& list, TIndex index)
            {
                auto result = list->GetType(index) == VTYPE_BINARY;
                if (result)
                {
                    underlying_type<PrimitiveType>::type typeRead;
                    auto binaryValue = list->GetBinary(index);
                    binaryValue->GetData(&typeRead, sizeof(underlying_type<PrimitiveType>::type), 0);
                    result = typeRead == static_cast<underlying_type<PrimitiveType>::type>(type);
                }
                return result;
            }

            template<typename TList, typename TIndex>
            void SetInt64(const CefRefPtr<TList>& list, TIndex index, const int64 &value)
            {
                unsigned char mem[1 + sizeof(int64)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::INT64);
                memcpy(reinterpret_cast<void*>(mem + 1), &value, sizeof(int64));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            int64 GetInt64(const CefRefPtr<TList>& list, TIndex index)
            {
                int64 result;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&result, sizeof(int64), 1);

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsInt64(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::INT64, list, index);
            }

            template<typename TList, typename TIndex>
            void SetCefTime(const CefRefPtr<TList>& list, TIndex index, const CefTime &value)
            {
                auto doubleT = value.GetDoubleT();
                unsigned char mem[1 + sizeof(double)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::CEFTIME);
                memcpy(reinterpret_cast<void*>(mem + 1), &doubleT, sizeof(double));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            CefTime GetCefTime(const CefRefPtr<TList>& list, TIndex index)
            {
                double doubleT;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&doubleT, sizeof(double), 1);

                return CefTime(doubleT);
            }

            template<typename TList, typename TIndex>
            bool IsCefTime(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::CEFTIME, list, index);
            }
            template<typename TList, typename TIndex>
            void SetJsCallback(const CefRefPtr<TList>& list, TIndex index, JavascriptCallback^ value)
            {
                auto id = value->Id;
                auto browserId = value->BrowserId;
                auto frameId = value->FrameId;

                unsigned char mem[1 + sizeof(int) + sizeof(int64) + sizeof(int64)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::JSCALLBACK);
                memcpy(reinterpret_cast<void*>(mem + 1), &browserId, sizeof(int));
                memcpy(reinterpret_cast<void*>(mem + 1 + sizeof(int)), &id, sizeof(int64));
                memcpy(reinterpret_cast<void*>(mem + 1 + sizeof(int) + sizeof(int64)), &frameId, sizeof(int64));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(const CefRefPtr<TList>& list, TIndex index)
            {
                auto result = gcnew JavascriptCallback();
                int64 id;
                int browserId;
                int64 frameId;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&browserId, sizeof(int), 1);
                binaryValue->GetData(&id, sizeof(int64), 1 + sizeof(int));
                binaryValue->GetData(&frameId, sizeof(int64), 1 + sizeof(int) + sizeof(int64));

                result->Id = id;
                result->BrowserId = browserId;
                result->FrameId = frameId;

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsJsCallback(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::JSCALLBACK, list, index);
            }
        }
    }
}