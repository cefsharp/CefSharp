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

    public:
        CefDownloadItemCallbackWrapper(CefRefPtr<CefDownloadItemCallback> &callback) : _callback(callback)
        {
            
        }

        ~CefDownloadItemCallbackWrapper()
        {
            _callback = NULL;
        }

        virtual void Cancel()
        {
            _callback->Cancel();
        }

        virtual void Pause()
        {
            _callback->Pause();
        }

        virtual void Resume()
        {
            _callback->Resume();
        }
    };
}

