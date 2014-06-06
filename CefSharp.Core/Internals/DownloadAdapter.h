// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_download_handler.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace Internals
    {
        private class DownloadAdapter : public CefDownloadHandler
        {
            gcroot<IDownloadHandler^> _handler;

        public:
            DownloadAdapter(IDownloadHandler^ handler) :
                _handler(handler)
            {
            }

            ~DownloadAdapter()
            {
                _handler = nullptr;
            }

            virtual void OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback) OVERRIDE
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

            virtual void OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item, CefRefPtr<CefDownloadItemCallback> callback) OVERRIDE
            {
                
                if (_handler->OnDownloadUpdated(GetDownloadItem(download_item)))
                {
                    callback->Cancel();
                }
            }

        private:
            CefSharp::DownloadItem^ GetDownloadItem(CefRefPtr<CefDownloadItem> download_item)
            {
                auto item = gcnew CefSharp::DownloadItem();
                item->Id = download_item->GetId();
                item->Url = StringUtils::ToClr(download_item->GetURL());
                item->MimeType = StringUtils::ToClr(download_item->GetMimeType());
                item->ContentDisposition = StringUtils::ToClr(download_item->GetContentDisposition());
                item->TotalBytes = download_item->GetTotalBytes();
                item->IsComplete = download_item->IsComplete();
                item->IsInProgress = download_item->IsInProgress();
                item->IsCancelled = download_item->IsCanceled();
                item->PercentComplete = download_item->GetPercentComplete();
                item->SuggestedFileName = StringUtils::ToClr(download_item->GetSuggestedFileName());
                item->FullPath = StringUtils::ToClr(download_item->GetFullPath());


                return item;
            }

            IMPLEMENT_REFCOUNTING(DownloadAdapter);
        };
    }
}
