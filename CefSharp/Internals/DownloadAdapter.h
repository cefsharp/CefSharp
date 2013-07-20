// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_download_handler.h"
#include "IDownloadHandler.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace Internals
    {
        private class DownloadAdapter : public CefDownloadHandler
        {
            gcroot<IDownloadHandler^> _handler;

        public:
            DownloadAdapter(IDownloadHandler^ handler) :
                _handler(handler)
            {
            }

            virtual bool ReceivedData(void* data, int data_size)
            {
                array<Byte>^ bytes = gcnew array<Byte>(data_size);
                Marshal::Copy(IntPtr(data), bytes, 0, data_size);

                return _handler->ReceivedData(bytes);
            }

            virtual void Complete()
            {
                _handler->Complete();
            }

            IMPLEMENT_REFCOUNTING(DownloadAdapter);
        };
    }
}
