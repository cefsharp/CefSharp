// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_download_handler.h"
#include "IDownloadHandler.h"

namespace CefSharp
{
    namespace Internals
    {
        private class DownloadAdapter : public CefDownloadHandler
        {
            gcroot<IDownloadHandler^> _handler;

        public:
            DownloadAdapter(IDownloadHandler^ handler) : _handler(handler) { }

            virtual bool ReceivedData(void* data, int data_size);
            virtual void Complete();

            IMPLEMENT_REFCOUNTING(DownloadAdapter);
        };
    }
}
