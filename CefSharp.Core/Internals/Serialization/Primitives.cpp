#include "Stdafx.h"
#include "Primitives.h"

#include "include/cef_app.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            enum class PrimitiveType : unsigned char
            {
                INT64,
                CEFTIME
            };

            template<typename TList, typename TIndex>
            bool IsType(PrimitiveType type, CefRefPtr<TList> list, TIndex index)
            {
                auto result = list->GetType(index) == VTYPE_BINARY;
                if (result)
                {
                    PrimitiveType type;
                    auto binaryValue = list->GetBinary(index);
                    binaryValue->GetData(&type, sizeof(PrimitiveType), 0);
                    result = type == type;
                }
                return result;
            }

            template<typename TList, typename TIndex>
            void SetInt64(const int64 &value, CefRefPtr<TList> list, TIndex index)
            {
                unsigned char mem[1 + sizeof(int64)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::INT64);
                memcpy(reinterpret_cast<void*>(mem+1), &value, sizeof(int64));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            int64 GetInt64(CefRefPtr<TList> list, TIndex index)
            {
                int64 result;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&result, sizeof(int64), 1);

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsInt64(CefRefPtr<TList> list, TIndex index)
            {
                return IsType(PrimitiveType::INT64, list, index);
            }

            template<typename TList, typename TIndex>
            void SetCefTime(const CefTime &value, CefRefPtr<TList> list, TIndex index)
            {
                auto doubleT = value.GetDoubleT();
                unsigned char mem[1 + sizeof(double)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::INT64);
                memcpy(reinterpret_cast<void*>(mem + 1), &doubleT, sizeof(double));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            CefTime GetCefTime(CefRefPtr<TList> list, TIndex index)
            {
                double doubleT;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&doubleT, sizeof(double), 1);

                return CefTime(doubleT);
            }

            template<typename TList, typename TIndex>
            bool IsCefTime(CefRefPtr<TList> list, TIndex index)
            {
                return IsType(PrimitiveType::CEFTIME, list, index);
            }

            template void SetInt64(const int64 &value, CefRefPtr<CefListValue> list, int index);
            template void SetInt64(const int64 &value, CefRefPtr<CefDictionaryValue> list, CefString index);
            template int64 GetInt64(CefRefPtr<CefListValue> list, int index);
            template int64 GetInt64(CefRefPtr<CefDictionaryValue> list, CefString index);
            template bool IsInt64(CefRefPtr<CefListValue> list, int index);
            template bool IsInt64(CefRefPtr<CefDictionaryValue> list, CefString index);

            template void SetCefTime(const CefTime &value, CefRefPtr<CefListValue> list, int index);
            template void SetCefTime(const CefTime &value, CefRefPtr<CefDictionaryValue> list, CefString index);
            template CefTime GetCefTime(CefRefPtr<CefListValue> list, int index);
            template CefTime GetCefTime(CefRefPtr<CefDictionaryValue> list, CefString index);
            template bool IsCefTime(CefRefPtr<CefListValue> list, int index);
            template bool IsCefTime(CefRefPtr<CefDictionaryValue> list, CefString index);
        }
    }
}