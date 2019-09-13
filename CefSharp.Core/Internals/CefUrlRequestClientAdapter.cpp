// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefUrlRequestClientAdapter.h"
#include "CefUrlRequestWrapper.h"
#include "CefAuthCallbackWrapper.h"


void CefUrlRequestClientAdapter::OnRequestComplete(CefRefPtr<CefURLRequest> request)
{
    _client->OnRequestComplete(gcnew CefUrlRequestWrapper(request));
}

void CefUrlRequestClientAdapter::OnUploadProgress(CefRefPtr<CefURLRequest> request, int64 current, int64 total)
{
    _client->OnUploadProgress(gcnew CefUrlRequestWrapper(request), current, total);
}

void CefUrlRequestClientAdapter::OnDownloadProgress(CefRefPtr<CefURLRequest> request, int64 current, int64 total)
{
    _client->OnDownloadProgress(gcnew CefUrlRequestWrapper(request), current, total);
}

void CefUrlRequestClientAdapter::OnDownloadData(CefRefPtr<CefURLRequest> request, const void* data, size_t data_length)
{
    auto data_array = gcnew array<Byte>(data_length);
    pin_ptr<Byte> data_array_start = &data_array[0];
    memcpy(data_array_start, data, data_length);
    _client->OnDownloadData(
        gcnew CefUrlRequestWrapper(request),
        data_array
    );
}

bool CefUrlRequestClientAdapter::GetAuthCredentials(bool isProxy,
    const CefString& host,
    int port,
    const CefString& realm,
    const CefString& scheme,
    CefRefPtr<CefAuthCallback> callback)
{
    return _client->GetAuthCredentials(
        isProxy,
        StringUtils::ToClr(host),
        port,
        StringUtils::ToClr(realm),
        StringUtils::ToClr(scheme),
        gcnew CefAuthCallbackWrapper(callback));
}

