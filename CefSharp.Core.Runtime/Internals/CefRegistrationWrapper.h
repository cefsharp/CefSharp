// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_registration.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefRegistrationWrapper : public IRegistration, public CefWrapper
        {
        private:
            MCefRefPtr<CefRegistration> _callback;

        public:
            CefRegistrationWrapper(CefRefPtr<CefRegistration> &callback)
                : _callback(callback)
            {

            }

            !CefRegistrationWrapper()
            {
                _callback = NULL;
            }

            ~CefRegistrationWrapper()
            {
                this->!CefRegistrationWrapper();

                _disposed = true;
            }
        };
    }
}
