// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefUrlRequestWrapper.h"
#include "CefResponseWrapper.h"

bool CefUrlRequestWrapper::ResponseWasCached::get()
{
    ThrowIfDisposed();

    return _urlRequest->ResponseWasCached();
}

IResponse^ CefUrlRequestWrapper::GetResponse()
{
    ThrowIfDisposed();

    return gcnew CefResponseWrapper(_urlRequest->GetResponse());
}

UrlRequestStatus CefUrlRequestWrapper::GetRequestStatus()
{
    ThrowIfDisposed();

    return (UrlRequestStatus)_urlRequest->GetRequestStatus();
}

