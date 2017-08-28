// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/internal/cef_ptr.h"
#include "include\cef_download_item.h"
#include "include\cef_response.h"
#include "include\cef_web_plugin.h"

#include "Serialization\ObjectsSerialization.h"
#include "Serialization\V8Serialization.h"

using namespace System::Collections::Generic;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace Internals
    {
        //Static class for converting to/from Cef classes
        private ref class TypeConversion abstract sealed
        {
        public:
            //Convert from NameValueCollection to HeaderMap
            static CefResponse::HeaderMap ToNative(NameValueCollection^ headers)
            {
                CefResponse::HeaderMap result;

                if (headers == nullptr)
                {
                    return result;
                }

                for each (String^ key in headers)
                {
                    for each(String^ value in headers->GetValues(key))
                    {
                        result.insert(std::pair<CefString, CefString>(StringUtils::ToNative(key), StringUtils::ToNative(value)));
                    }
                }

                return result;
            }

            //ConvertFrom CefDownload to DownloadItem
            static DownloadItem^ FromNative(CefRefPtr<CefDownloadItem> downloadItem)
            {
                auto item = gcnew CefSharp::DownloadItem();
                item->IsValid = downloadItem->IsValid();
                //NOTE: Description for IsValid says `Do not call any other methods if this function returns false.` so only load if IsValid = true
                if(item->IsValid)
                {
                    item->IsInProgress = downloadItem->IsInProgress();
                    item->IsComplete = downloadItem->IsComplete();
                    item->IsCancelled = downloadItem->IsCanceled();
                    item->CurrentSpeed = downloadItem->GetCurrentSpeed();
                    item->PercentComplete = downloadItem->GetPercentComplete();
                    item->TotalBytes = downloadItem->GetTotalBytes();
                    item->ReceivedBytes = downloadItem->GetReceivedBytes();
                    item->StartTime = FromNative(downloadItem->GetStartTime());
                    item->EndTime = FromNative(downloadItem->GetEndTime());
                    item->FullPath = StringUtils::ToClr(downloadItem->GetFullPath());
                    item->Id = downloadItem->GetId();
                    item->Url = StringUtils::ToClr(downloadItem->GetURL());
                    item->OriginalUrl = StringUtils::ToClr(downloadItem->GetOriginalUrl());
                    item->SuggestedFileName = StringUtils::ToClr(downloadItem->GetSuggestedFileName());
                    item->ContentDisposition = StringUtils::ToClr(downloadItem->GetContentDisposition());
                    item->MimeType = StringUtils::ToClr(downloadItem->GetMimeType());
                }

                return item;
            }

            //Convert from CefTime to Nullable<DateTime>
            static Nullable<DateTime> FromNative(CefTime time)
            {
                auto epoch = time.GetDoubleT();
                if(epoch == 0)
                {
                    return Nullable<DateTime>();
                }
                return Nullable<DateTime>(DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime());
            }

            static WebPluginInfo^ FromNative(CefRefPtr<CefWebPluginInfo> webPluginInfo)
            {
                return gcnew WebPluginInfo(StringUtils::ToClr(webPluginInfo->GetName()),
                                           StringUtils::ToClr(webPluginInfo->GetDescription()),
                                           StringUtils::ToClr(webPluginInfo->GetPath()),
                                           StringUtils::ToClr(webPluginInfo->GetVersion()));
            }

            static IList<DraggableRegion>^ FromNative(const std::vector<CefDraggableRegion>& regions)
            {
                if (regions.size() == 0)
                {
                    return nullptr;
                }

                auto list = gcnew List<DraggableRegion>();

                for each (CefDraggableRegion region in regions)
                {
                    list->Add(DraggableRegion(region.bounds.width, region.bounds.height, region.bounds.x, region.bounds.y, region.draggable == 1));
                }
                
                return list;
            }

            static CefRefPtr<CefValue> ToNative(Object^ value)
            {
                auto cefValue = CefValue::Create();

                if (value == nullptr)
                {
                    cefValue->SetNull();

                    return cefValue;
                }

                auto type = value->GetType();
                Type^ underlyingType = Nullable::GetUnderlyingType(type);
                if (underlyingType != nullptr)
                {
                    type = underlyingType;
                }

                if (type == Boolean::typeid)
                {
                    cefValue->SetBool(safe_cast<bool>(value));
                }
                else if (type == Int32::typeid)
                {
                    cefValue->SetInt(safe_cast<int>(value));
                }
                else if (type == String::typeid)
                {
                    cefValue->SetString(StringUtils::ToNative(safe_cast<String^>(value)));
                }
                else if (type == Double::typeid)
                {
                    cefValue->SetDouble(safe_cast<double>(value));
                }
                else if (type == Decimal::typeid)
                {
                    cefValue->SetDouble(Convert::ToDouble(value));
                }
                else if (type == List<Object^>::typeid)
                {
                    auto list = safe_cast<List<Object^>^>(value);
                    auto cefList = CefListValue::Create();
                    for (int i = 0; i < list->Count; i++)
                    {
                        auto value = list[i];
                        SerializeV8Object(cefList, i, value);
                    }
                    cefValue->SetList(cefList);
                }
                else if (type == Dictionary<String^, Object^>::typeid)
                {
                    auto dictionary = safe_cast<Dictionary<String^, Object^>^>(value);
                    auto cefDictionary = CefDictionaryValue::Create();

                    for each (KeyValuePair<String^, Object^>^ entry in dictionary)
                    {
                        auto key = StringUtils::ToNative(entry->Key);
                        auto value = entry->Value;
                        SerializeV8Object(cefDictionary, key, value);
                    }

                    cefValue->SetDictionary(cefDictionary);
                }
            
                return cefValue;
            }

            static Object^ FromNative(const CefRefPtr<CefValue>& value)
            {
                if (!value.get())
                {
                    return nullptr;
                }

                auto type = value->GetType();

                if (type == CefValueType::VTYPE_BOOL)
                {
                    return value->GetBool();
                }

                if (type == CefValueType::VTYPE_DOUBLE)
                {
                    return value->GetDouble();
                }

                if (type == CefValueType::VTYPE_INT)
                {
                    return value->GetInt();
                }

                if (type == CefValueType::VTYPE_STRING)
                {
                    return StringUtils::ToClr(value->GetString());
                }

                if (type == CefValueType::VTYPE_DICTIONARY)
                {
                    return FromNative(value->GetDictionary());
                }
                
                return nullptr;
            }

            static IDictionary<String^, Object^>^ FromNative(const CefRefPtr<CefDictionaryValue>& dictionary)
            {
                if (!dictionary.get() || dictionary->GetSize() == 0)
                {
                    return nullptr;
                }

                auto dict = gcnew Dictionary<String^, Object^>();

                CefDictionaryValue::KeyList keys;
                dictionary->GetKeys(keys);

                for (auto i = 0; i < keys.size(); i++)
                {
                    auto key = StringUtils::ToClr(keys[i]);
                    auto value = DeserializeObject(dictionary, keys[i], nullptr);

                    dict->Add(key, value);
                }

                return dict;
            }
        };
    }
}