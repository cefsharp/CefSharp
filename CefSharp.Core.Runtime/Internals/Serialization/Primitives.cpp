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
                JSCALLBACK,
                ARRAYBUFFER
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
            void SetInt64(const CefRefPtr<TList>& list, TIndex index, const int64_t&value)
            {
                unsigned char mem[1 + sizeof(int64_t)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::INT64);
                memcpy(reinterpret_cast<void*>(mem + 1), &value, sizeof(int64_t));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            int64_t GetInt64(const CefRefPtr<TList>& list, TIndex index)
            {
                int64_t result;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&result, sizeof(int64_t), 1);

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsInt64(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::INT64, list, index);
            }

            template<typename TList, typename TIndex>
            void SetCefTime(const CefRefPtr<TList>& list, TIndex index, const int64_t&value)
            {
                unsigned char mem[1 + sizeof(int64_t)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::CEFTIME);
                memcpy(reinterpret_cast<void*>(mem + 1), &value, sizeof(int64_t));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            CefBaseTime GetCefTime(const CefRefPtr<TList>& list, TIndex index)
            {
                CefBaseTime baseTime;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&baseTime.val, sizeof(int64_t), 1);

                return baseTime;
            }

            template<typename TList, typename TIndex>
            bool IsCefTime(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::CEFTIME, list, index);
            }

            template<typename TList, typename TIndex>
            void SetArrayBuffer(const CefRefPtr<TList>& list, TIndex index, const size_t& size, const void* value)
            {
                const auto src = static_cast<const uint8_t*>(value);

                auto dest = new uint8_t[size + 1];
                dest[0] = static_cast<uint8_t>(PrimitiveType::ARRAYBUFFER);
                memcpy(&dest[1], src, size);

                list->SetBinary(index, CefBinaryValue::Create(dest, size + 1));
            }

            template<typename TList, typename TIndex>
            cli::array<Byte>^ GetArrayBuffer(const CefRefPtr<TList>& list, TIndex index)
            {
                auto binaryValue = list->GetBinary(index);
                auto size = binaryValue->GetSize() - 1;

                auto bufferByte = gcnew cli::array<Byte>(static_cast<int>(size));
                pin_ptr<Byte> src = &bufferByte[0]; // pin pointer to first element in arr

                binaryValue->GetData(static_cast<void*>(src), size, 1);

                return bufferByte;
            }

            template<typename TList, typename TIndex>
            bool IsArrayBuffer(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::ARRAYBUFFER, list, index);
            }

            template<typename TList, typename TIndex>
            void SetJsCallback(const CefRefPtr<TList>& list, TIndex index, JavascriptCallback^ value)
            {
                auto bytes = value->ToByteArray(static_cast<unsigned char>(PrimitiveType::JSCALLBACK));
                pin_ptr<Byte> bytesPtr = &bytes[0];                

                auto binaryValue = CefBinaryValue::Create(bytesPtr, bytes->Length);
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(const CefRefPtr<TList>& list, TIndex index)
            {
                auto binaryValue = list->GetBinary(index);
                auto bufferSize = (int)binaryValue->GetSize();
                auto buffer = gcnew cli::array<Byte>(bufferSize);
                pin_ptr<Byte> bufferPtr = &buffer[0]; // pin pointer to first element in arr

                //TODO: We can potentially further optimise this by geting binaryValue->GetRawData
                // and then reading directly from that                
                binaryValue->GetData(static_cast<void*>(bufferPtr), bufferSize, 0);

                return JavascriptCallback::FromBytes(buffer);
            }

            template<typename TList, typename TIndex>
            bool IsJsCallback(const CefRefPtr<TList>& list, TIndex index)
            {
                return IsType(PrimitiveType::JSCALLBACK, list, index);
            }
        }
    }
}
