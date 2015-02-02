// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/internal/cef_ptr.h"
#include "include/cef_download_item.h"
#include "Internals/StringUtils.h"

using namespace System;
using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        //Static class for converting to/from Cef classes
        private ref class TypeConversion abstract sealed
        {
        public:
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
                auto managedWebPluginInfo = gcnew WebPluginInfo();
                managedWebPluginInfo->Description = StringUtils::ToClr(webPluginInfo->GetDescription());
                managedWebPluginInfo->Name = StringUtils::ToClr(webPluginInfo->GetName());
                managedWebPluginInfo->Path = StringUtils::ToClr(webPluginInfo->GetPath());
                managedWebPluginInfo->Version = StringUtils::ToClr(webPluginInfo->GetVersion());

                return managedWebPluginInfo;
            }
        };
    }
}