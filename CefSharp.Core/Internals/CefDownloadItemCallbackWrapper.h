// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_download_handler.h"

namespace CefSharp
{
    public ref class CefDownloadItemCallbackWrapper : IDownloadItemCallback
    {
    private:
        MCefRefPtr<CefDownloadItemCallback> _callback;
        bool _disposed;

    public:
        CefDownloadItemCallbackWrapper(CefRefPtr<CefDownloadItemCallback> &callback) 
            : _callback(callback), _disposed(false)
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
            _callback->Cancel();

            delete this;
        }

        virtual void Pause()
        {
            _callback->Pause();

            delete this;
        }

        virtual void Resume()
        {
            _callback->Resume();

            _callback = NULL;
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

