// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_context_menu_handler.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefRunContextMenuCallbackWrapper : public IRunContextMenuCallback, public CefWrapper
        {
        private:
            MCefRefPtr< CefRunContextMenuCallback> _callback;

        public:
            CefRunContextMenuCallbackWrapper(CefRefPtr< CefRunContextMenuCallback> &callback) :
                _callback(callback)
            {
            
            }

            !CefRunContextMenuCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefRunContextMenuCallbackWrapper()
            {
                this->!CefRunContextMenuCallbackWrapper();

                _disposed = true;
            }

            virtual void Cancel()
            {
                ThrowIfDisposed();

                _callback->Cancel();

                delete this;
            }

            virtual void Continue(CefMenuCommand commandId, CefEventFlags eventFlags)
            {
                ThrowIfDisposed();

                _callback->Continue((int)commandId, (CefRunContextMenuCallback::EventFlags) eventFlags);

                delete this;
            }
        };
    }
}