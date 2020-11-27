// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefUrlRequestClientAdapter.h"
#include "UrlRequest.h"
#include "CefAuthCallbackWrapper.h"

using namespace System::IO;

using namespace CefSharp::Core;

void CefUrlRequestClientAdapter::OnRequestComplete(CefRefPtr<CefURLRequest> request)
{
    _client->OnRequestComplete(gcnew UrlRequest(request));
}

void CefUrlRequestClientAdapter::OnUploadProgress(CefRefPtr<CefURLRequest> request, int64 current, int64 total)
{
    _client->OnUploadProgress(gcnew UrlRequest(request), current, total);
}

void CefUrlRequestClientAdapter::OnDownloadProgress(CefRefPtr<CefURLRequest> request, int64 current, int64 total)
{
    _client->OnDownloadProgress(gcnew UrlRequest(request), current, total);
}

void CefUrlRequestClientAdapter::OnDownloadData(CefRefPtr<CefURLRequest> request, const void* data, size_t data_length)
{
    UnmanagedMemoryStream readStream((Byte*)data, (Int64)data_length, (Int64)data_length, FileAccess::Read);

    _client->OnDownloadData(
        gcnew UrlRequest(request),
        %readStream
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

