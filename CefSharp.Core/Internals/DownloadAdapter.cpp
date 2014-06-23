// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_download_handler.h"
#include "DownloadAdapter.h"

using namespace System;
using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        DownloadAdapter::~DownloadAdapter()
        {
            _handler = nullptr;
        }

        void DownloadAdapter::OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback)
        {
            String^ download_path;
            bool show_dialog;
            auto downloadItem = GetDownloadItem(download_item);
            downloadItem->SuggestedFileName = StringUtils::ToClr(suggested_name);

            if (_handler->OnBeforeDownload(downloadItem, download_path, show_dialog))
            {
                callback->Continue(StringUtils::ToNative(download_path), show_dialog);
            }
        };

        void DownloadAdapter::OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            CefRefPtr<CefDownloadItemCallback> callback) 
        {
            if (_handler->OnDownloadUpdated(GetDownloadItem(download_item)))
            {
                callback->Cancel();
            }
        }

        CefSharp::DownloadItem^ DownloadAdapter::GetDownloadItem(CefRefPtr<CefDownloadItem> download_item)
        {
            auto item = gcnew CefSharp::DownloadItem();
            item->IsValid = download_item->IsValid();
            //NOTE: Description for IsValid says `Do not call any other methods if this function returns false.` so only load if IsValid = true
            if(item->IsValid)
            {
                item->IsInProgress = download_item->IsInProgress();
                item->IsComplete = download_item->IsComplete();
                item->IsCancelled = download_item->IsCanceled();
                item->CurrentSpeed = download_item->GetCurrentSpeed();
                item->PercentComplete = download_item->GetPercentComplete();
                item->TotalBytes = download_item->GetTotalBytes();
                item->ReceivedBytes = download_item->GetReceivedBytes();
                item->StartTime = ConvertCefTimeToNullableDateTime(download_item->GetStartTime());
                item->EndTime = ConvertCefTimeToNullableDateTime(download_item->GetEndTime());
                item->FullPath = StringUtils::ToClr(download_item->GetFullPath());
                item->Id = download_item->GetId();
                item->Url = StringUtils::ToClr(download_item->GetURL());
                item->SuggestedFileName = StringUtils::ToClr(download_item->GetSuggestedFileName());
                item->ContentDisposition = StringUtils::ToClr(download_item->GetContentDisposition());
                item->MimeType = StringUtils::ToClr(download_item->GetMimeType());
            }
            
            return item;
        }

        Nullable<DateTime> DownloadAdapter::ConvertCefTimeToNullableDateTime(CefTime time)
        {
            auto epoch = time.GetDoubleT();
            if(epoch == 0)
            {
                return Nullable<DateTime>();
            }
            return Nullable<DateTime>(DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime());
        }
    }
}
