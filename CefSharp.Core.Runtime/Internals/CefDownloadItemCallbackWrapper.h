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
        private ref class CefDownloadItemCallbackWrapper : public IDownloadItemCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefDownloadItemCallback> _callback;

        public:
            CefDownloadItemCallbackWrapper(CefRefPtr<CefDownloadItemCallback> &callback)
                : _callback(callback)
            {
            }

            !CefDownloadItemCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefDownloadItemCallbackWrapper()
            {
                this->!CefDownloadItemCallbackWrapper();

                _disposed = true;
            }

            virtual void Cancel()
            {
                ThrowIfDisposed();

                _callback->Cancel();

                delete this;
            }

            virtual void Pause()
            {
                ThrowIfDisposed();

                //Only free the callback when cancel called 
                //Need to be able to pause/resume #3145
                _callback->Pause();
            }

            virtual void Resume()
            {
                ThrowIfDisposed();

                //Only free the callback when cancel called 
                //Need to be able to pause/resume #3145
                _callback->Resume();
            }
        };
    }
}

