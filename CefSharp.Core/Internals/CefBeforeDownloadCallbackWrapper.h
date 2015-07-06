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

    public:
        CefBeforeDownloadCallbackWrapper(CefRefPtr<CefBeforeDownloadCallback> &callback)
            : _callback(callback)
        {
            
        }

        !CefBeforeDownloadCallbackWrapper()
        {
            _callback = NULL;
        }

        ~CefBeforeDownloadCallbackWrapper()
        {
            this->!CefBeforeDownloadCallbackWrapper();
        }

        virtual void Continue(String^ downloadPath, bool showDialog)
        {
            _callback->Continue(StringUtils::ToNative(downloadPath), showDialog);

            delete this;
        }
    };
}

