#include "Stdafx.h"
#include "Internals/Serialization/Primitives.h"
#include "ObjectsDeserialization.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            JavascriptRootObject^ DeserializeJsRootObject(CefRefPtr<CefListValue> list)
            {
                auto result = gcnew JavascriptRootObject();
                auto memberCount = list->GetSize();

                for (auto i = 0; i < memberCount; i++)
                {
                    result->MemberObjects->Add(DeserializeJsObject(list, i));
                }

                return result;
            }

            JavascriptObject^ DeserializeJsObject(CefRefPtr<CefListValue> list, int index)
            {
                auto result = gcnew JavascriptObject();
                auto val = list->GetList(index);
                auto i = 0;

                result->Id = GetInt64(val, i++);
                result->Name = StringUtils::ToClr(val->GetString(i++));
                result->JavascriptName = StringUtils::ToClr(val->GetString(i++));

                auto methodCount = val->GetInt(i++);
                for (auto j = 0; j < methodCount; j++)
                {
                    auto method = gcnew JavascriptMethod();
                    method->Id = GetInt64(val, i++);
                    method->JavascriptName = StringUtils::ToClr(val->GetString(i++));
                    method->ManagedName = StringUtils::ToClr(val->GetString(i++));
                    method->ParameterCount = val->GetInt(i++);

                    result->Methods->Add(method);
                }

                auto propertyCount = val->GetInt(i++);
                for (auto j = 0; j < propertyCount; j++)
                {
                    auto prop = gcnew JavascriptProperty();
                    prop->Id = GetInt64(val, i++);
                    prop->JavascriptName = StringUtils::ToClr(val->GetString(i++));
                    prop->ManagedName = StringUtils::ToClr(val->GetString(i++));
                    prop->IsComplexType = val->GetBool(i++);
                    prop->IsReadOnly = val->GetBool(i++);
                    if (prop->IsComplexType)
                    {
                        auto type = val->GetType(i);
                        if (type == VTYPE_LIST)
                        {
                            prop->JsObject = DeserializeJsObject(val, i++);
                        }
                    }

                    result->Properties->Add(prop);
                }

                return result;
            }
        }
    }
}