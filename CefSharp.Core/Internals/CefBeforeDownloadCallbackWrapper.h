// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_download_handler.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefBeforeDownloadCallbackWrapper : public IBeforeDownloadCallback, public CefWrapper
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

                _disposed = true;
            }

            virtual void Continue(String^ downloadPath, bool showDialog)
            {
                ThrowIfDisposed();

                _callback->Continue(StringUtils::ToNative(downloadPath), showDialog);

                delete this;
            }
        };
    }
}

