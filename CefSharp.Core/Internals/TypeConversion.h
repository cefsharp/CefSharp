// Copyright © 2015 The CefSharp Authors. All rights reserved.
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
using namespace System::Collections::Specialized;
using namespace System::Security::Cryptography::X509Certificates;
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
                if (item->IsValid)
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
                if (epoch == 0)
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
                else if (System::Collections::IDictionary::typeid->IsAssignableFrom(type))
                {
                    auto dictionary = (System::Collections::IDictionary^) value;
                    auto cefDictionary = CefDictionaryValue::Create();

                    for each (System::Collections::DictionaryEntry entry in dictionary)
                    {
                        auto key = StringUtils::ToNative(Convert::ToString(entry.Key));
                        auto entryValue = entry.Value;
                        //We don't pass a nameConverter here as the keys should
                        //remain unchanged
                        SerializeV8Object(cefDictionary, key, entryValue, nullptr);
                    }

                    cefValue->SetDictionary(cefDictionary);
                }
                else if (System::Collections::IEnumerable::typeid->IsAssignableFrom(type))
                {
                    auto enumerable = (System::Collections::IEnumerable^) value;
                    auto cefList = CefListValue::Create();

                    int i = 0;
                    for each (Object^ arrObj in enumerable)
                    {
                        //We don't pass a nameConverter here as the keys should
                        //remain unchanged
                        SerializeV8Object(cefList, i, arrObj, nullptr);

                        i++;
                    }
                    cefValue->SetList(cefList);
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

                if (type == CefValueType::VTYPE_LIST)
                {
                    return FromNative(value->GetList());
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

                for (size_t i = 0; i < keys.size(); i++)
                {
                    auto key = StringUtils::ToClr(keys[i]);
                    auto value = DeserializeObject(dictionary, keys[i], nullptr);

                    dict->Add(key, value);
                }

                return dict;
            }

            static List<Object^>^ FromNative(const CefRefPtr<CefListValue>& list)
            {
                auto result = gcnew List<Object^>(list->GetSize());
                for (size_t i = 0; i < list->GetSize(); i++)
                {
                    result->Add(DeserializeObject(list, i, nullptr));
                }

                return result;
            }

            // Copied from CefSharp.BrowserSubprocess.Core\TypeUtils.h since it can't be included
            static DateTime ConvertCefTimeToDateTime(CefTime time)
            {
                return DateTimeUtils::FromCefTime(time.year,
                    time.month,
                    time.day_of_month,
                    time.hour,
                    time.minute,
                    time.second,
                    time.millisecond);
            }

            static Cookie^ FromNative(const CefCookie& cefCookie)
            {
                auto cookie = gcnew Cookie();
                auto cookieName = StringUtils::ToClr(cefCookie.name);

                if (!String::IsNullOrEmpty(cookieName))
                {
                    cookie->Name = cookieName;
                    cookie->Value = StringUtils::ToClr(cefCookie.value);
                    cookie->Domain = StringUtils::ToClr(cefCookie.domain);
                    cookie->Path = StringUtils::ToClr(cefCookie.path);
                    cookie->Secure = cefCookie.secure == 1;
                    cookie->HttpOnly = cefCookie.httponly == 1;
                    cookie->Creation = ConvertCefTimeToDateTime(cefCookie.creation);
                    cookie->LastAccess = ConvertCefTimeToDateTime(cefCookie.last_access);

                    if (cefCookie.has_expires)
                    {
                        cookie->Expires = ConvertCefTimeToDateTime(cefCookie.expires);
                    }
                }

                return cookie;
            }

            static NavigationEntry^ FromNative(const CefRefPtr<CefNavigationEntry> entry, bool current)
            {
                SslStatus^ sslStatus;

                if (!entry.get())
                {
                    return nullptr;
                }

                if (!entry->IsValid())
                {
                    return gcnew NavigationEntry(current, DateTime::MinValue, nullptr, -1, nullptr, nullptr, (TransitionType)-1, nullptr, false, false, sslStatus);
                }

                auto time = entry->GetCompletionTime();
                DateTime completionTime = CefTimeUtils::ConvertCefTimeToDateTime(time.GetDoubleT());
                auto ssl = entry->GetSSLStatus();
                X509Certificate2^ sslCertificate;

                if (ssl.get())
                {
                    auto certificate = ssl->GetX509Certificate();
                    if (certificate.get())
                    {
                        auto derEncodedCertificate = certificate->GetDEREncoded();
                        auto byteCount = derEncodedCertificate->GetSize();
                        if (byteCount > 0)
                        {
                            auto bytes = gcnew cli::array<Byte>(byteCount);
                            pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                            derEncodedCertificate->GetData(static_cast<void*>(src), byteCount, 0);

                            sslCertificate = gcnew X509Certificate2(bytes);
                        }
                    }

                    sslStatus = gcnew SslStatus(ssl->IsSecureConnection(), (CertStatus)ssl->GetCertStatus(), (SslVersion)ssl->GetSSLVersion(), (SslContentStatus)ssl->GetContentStatus(), sslCertificate);
                }

                return gcnew NavigationEntry(current, completionTime, StringUtils::ToClr(entry->GetDisplayURL()), entry->GetHttpStatusCode(),
                    StringUtils::ToClr(entry->GetOriginalURL()), StringUtils::ToClr(entry->GetTitle()), (TransitionType)entry->GetTransitionType(),
                    StringUtils::ToClr(entry->GetURL()), entry->HasPostData(), true, sslStatus);
            }
        };
    }
}
