// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_download_handler.h"

namespace CefSharp
{
    public ref class CefBeforeDownloadCallbackWrapper : IBeforeDownloadCallback
    {
    private:
        MCefRefPtr<CefBeforeDownloadCallback> _callback;
        bool _disposed;

    public:
        CefBeforeDownloadCallbackWrapper(CefRefPtr<CefBeforeDownloadCallback> &callback)
            : _callback(callback), _disposed(false)
        {
            
        }

        !CefBeforeDownloadCallbackWrapper()
        {
            _callback = NULL;
        }

        ~CefBeforeDownloadCallbackWrapper()
        {
            this->!CefBeforeDownloadCallbackWrapper();

            _disposed = true;
        }

        virtual void Continue(String^ downloadPath, bool showDialog)
        {
            _callback->Continue(StringUtils::ToNative(downloadPath), showDialog);

            delete this;
        }

        virtual property bool IsDisposed
        {
            bool get()
            {
                return _disposed;
            }
        }
    };
}

