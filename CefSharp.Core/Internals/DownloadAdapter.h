// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_download_handler.h"

using namespace System;
using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        private class DownloadAdapter : public CefDownloadHandler
        {
        private:
            gcroot<IDownloadHandler^> _handler;
            DownloadItem^ DownloadAdapter::GetDownloadItem(CefRefPtr<CefDownloadItem> download_item);
            Nullable<DateTime> ConvertCefTimeToNullableDateTime(CefTime time);

        public:
            DownloadAdapter(IDownloadHandler^ handler) : _handler(handler) { }
            ~DownloadAdapter();

            virtual void OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback) OVERRIDE;
            virtual void OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
                CefRefPtr<CefDownloadItemCallback> callback) OVERRIDE;
            
            IMPLEMENT_REFCOUNTING(DownloadAdapter);
        };
    }
}