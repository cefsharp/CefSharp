// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_download_handler.h"
#include "DownloadAdapter.h"
#include "TypeConversion.h"

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
            auto downloadItem = TypeConversion::FromNative(download_item);
            downloadItem->SuggestedFileName = StringUtils::ToClr(suggested_name);

            if (_handler->OnBeforeDownload(downloadItem, download_path, show_dialog))
            {
                callback->Continue(StringUtils::ToNative(download_path), show_dialog);
            }
        };

        void DownloadAdapter::OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            CefRefPtr<CefDownloadItemCallback> callback) 
        {
            if (!download_item->IsInProgress() && browser->IsPopup() && !browser->HasDocument())
            {
                browser->GetHost()->CloseBrowser(false);
            }

            if (_handler->OnDownloadUpdated(TypeConversion::FromNative(download_item)))
            {
                callback->Cancel();
            }
        }
    }
}
