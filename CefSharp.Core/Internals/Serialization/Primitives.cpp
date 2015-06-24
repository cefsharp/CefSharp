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
                CEFTIME,
                JSCALLBACK
            };

            template<typename TList, typename TIndex>
            bool IsType(PrimitiveType type, CefRefPtr<TList> list, TIndex index)
            {
                auto result = list->GetType(index) == VTYPE_BINARY;
                if (result)
                {
					unsigned char typeChar;
                    auto binaryValue = list->GetBinary(index);
					binaryValue->GetData(&typeChar, sizeof(PrimitiveType), 0);
					result = typeChar == static_cast<unsigned char>(type);
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
                mem[0] = static_cast<unsigned char>(PrimitiveType::CEFTIME);
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

            template<typename TList, typename TIndex>
            void SetJsCallback(JavascriptCallback^ value, CefRefPtr<TList> list, TIndex index)
            {
                auto id = value->Id;
                auto browserId = value->BrowserId;

                unsigned char mem[1 + sizeof(int) + sizeof(int64)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::JSCALLBACK);
                memcpy(reinterpret_cast<void*>(mem + 1), &browserId, sizeof(int));
                memcpy(reinterpret_cast<void*>(mem + 1 + sizeof(int)), &id, sizeof(int64));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(CefRefPtr<TList> list, TIndex index)
            {
                auto result = gcnew JavascriptCallback();
                int64 id;
                int browserId;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&browserId, sizeof(int), 1);
                binaryValue->GetData(&id, sizeof(int64), 1 + sizeof(int));

                result->Id = id;
                result->BrowserId = browserId;

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsJsCallback(CefRefPtr<TList> list, TIndex index)
            {
                return IsType(PrimitiveType::JSCALLBACK, list, index);
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

            template void SetJsCallback(JavascriptCallback^ value, CefRefPtr<CefListValue> list, int index);
            template void SetJsCallback(JavascriptCallback^ value, CefRefPtr<CefDictionaryValue> list, CefString index);
            template JavascriptCallback^ GetJsCallback(CefRefPtr<CefListValue> list, int index);
            template JavascriptCallback^ GetJsCallback(CefRefPtr<CefDictionaryValue> list, CefString index);
            template bool IsJsCallback(CefRefPtr<CefListValue> list, int index);
            template bool IsJsCallback(CefRefPtr<CefDictionaryValue> list, CefString index);
        }
    }
}