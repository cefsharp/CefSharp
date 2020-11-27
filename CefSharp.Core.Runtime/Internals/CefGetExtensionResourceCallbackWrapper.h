// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_download_handler.h"
#include "CefWrapper.h"

using namespace System::IO;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefGetExtensionResourceCallbackWrapper : public IGetExtensionResourceCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefGetExtensionResourceCallback> _callback;

        public:
            CefGetExtensionResourceCallbackWrapper(CefRefPtr<CefGetExtensionResourceCallback> &callback)
                : _callback(callback)
            {
            }

            !CefGetExtensionResourceCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefGetExtensionResourceCallbackWrapper()
            {
                this->!CefGetExtensionResourceCallbackWrapper();

                _disposed = true;
            }

            virtual void Cancel()
            {
                ThrowIfDisposed();

                _callback->Cancel();

                delete this;
            }

            virtual void Continue(Stream^ stream)
            {
                ThrowIfDisposed();

                throw gcnew NotImplementedException();

                delete this;
            }

            virtual void Continue(cli::array<Byte>^ data)
            {
                ThrowIfDisposed();

                pin_ptr<Byte> src = &data[0];

                auto streamReader = CefStreamReader::CreateForData(static_cast<void*>(src), data->Length);

                _callback->Continue(streamReader);

                delete this;
            }
        };
    }
}

