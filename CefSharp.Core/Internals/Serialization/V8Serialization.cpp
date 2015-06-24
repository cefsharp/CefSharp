#include "Stdafx.h"
#include "V8Serialization.h"
#include "Primitives.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            template<typename TList, typename TIndex>
			Object^ DeserializeV8Object(CefRefPtr<TList> list, TIndex index, IJavascriptCallbackFactory^ callbackFactory)
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
                else if (IsJsCallback(list, index))
                {
					result = callbackFactory->Create(GetJsCallback(list, index));
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
                        array->Add(DeserializeV8Object(subList, i, callbackFactory));
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
                        dict->Add(StringUtils::ToClr(keys[i]), DeserializeV8Object(subDict, keys[i], callbackFactory));
                    }

                    result = dict;
                }
                
                return result;
            }

            template<typename TList, typename TIndex>
            void SerializeV8Object(Object^ obj, CefRefPtr<TList> list, TIndex index)
            {
                list->SetNull(index);

                if (obj == nullptr)
                {
                    return;
                }

                auto type = obj->GetType();
                Type^ underlyingType = Nullable::GetUnderlyingType(type);
                if (underlyingType != nullptr) type = underlyingType;

                if (type == Boolean::typeid)
                    list->SetBool(index, safe_cast<bool>(obj));
                else if (type == Int32::typeid)
                    list->SetInt(index, safe_cast<int>(obj));
                else if (type == String::typeid)
                    list->SetString(index, StringUtils::ToNative(safe_cast<String^>(obj)));
                else if (type == Double::typeid)
                    list->SetDouble(index, safe_cast<double>(obj));
                else if (type == Decimal::typeid)
                    list->SetDouble(index, Convert::ToDouble(obj));
                else if (type == SByte::typeid)
                    list->SetInt(index, Convert::ToInt32(obj));
                else if (type == Int16::typeid)
                    list->SetInt(index, Convert::ToInt32(obj));
                else if (type == Int64::typeid)
                    list->SetDouble(index, Convert::ToDouble(obj));
                else if (type == Byte::typeid)
                    list->SetInt(index, Convert::ToInt32(obj));
                else if (type == UInt16::typeid)
                    list->SetInt(index, Convert::ToInt32(obj));
                else if (type == UInt32::typeid)
                    list->SetDouble(index, Convert::ToDouble(obj));
                else if (type == UInt64::typeid)
                    list->SetDouble(index, Convert::ToDouble(obj));
                else if (type == Single::typeid)
                    list->SetDouble(index, Convert::ToDouble(obj));
                else if (type == Char::typeid)
                    list->SetInt(index, Convert::ToInt32(obj));
                else if (type == DateTime::typeid)
                    SetCefTime(ConvertDateTimeToCefTime(safe_cast<DateTime>(obj)), list, index);
                else if (type->IsArray)
                {
                    auto subList = CefListValue::Create();
                    Array^ managedArray = (Array^)obj;
                    for (int i = 0; i < managedArray->Length; i++)
                    {
                        Object^ arrObj;
                        arrObj = managedArray->GetValue(i);
                        SerializeV8Object(arrObj, subList, i);
                    }
                    list->SetList(index, subList);
                }
                else if (type->IsValueType && !type->IsPrimitive && !type->IsEnum)
                {
                    auto fields = type->GetFields();
                    auto subDict = CefDictionaryValue::Create();

                    for (int i = 0; i < fields->Length; i++)
                    {
                        auto fieldName = fields[i]->Name;
                        auto strFieldName = StringUtils::ToNative(safe_cast<String^>(fieldName));
                        auto fieldVal = fields[i]->GetValue(obj);
                        SerializeV8Object(fieldVal, subDict, strFieldName);
                    }
                }
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

            CefTime ConvertDateTimeToCefTime(DateTime dateTime)
            {
                auto timeSpan = dateTime - DateTime(1970, 1, 1);

                return CefTime(timeSpan.TotalSeconds);
            }

            template void SerializeV8Object(Object^ obj, CefRefPtr<CefListValue> list, int index);
            template void SerializeV8Object(Object^ obj, CefRefPtr<CefDictionaryValue> list, CefString index);
			template Object^ DeserializeV8Object(CefRefPtr<CefListValue> list, int index, IJavascriptCallbackFactory^ callbackFactory);
			template Object^ DeserializeV8Object(CefRefPtr<CefDictionaryValue> list, CefString index, IJavascriptCallbackFactory^ callbackFactory);
        }
    }
}