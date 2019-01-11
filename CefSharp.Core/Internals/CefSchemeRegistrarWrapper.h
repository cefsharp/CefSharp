// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_scheme.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefSchemeRegistrarWrapper : public ISchemeRegistrar, public CefWrapper
        {
        private:
            CefSchemeRegistrar* _registra;

        public:
            CefSchemeRegistrarWrapper(CefRawPtr<CefSchemeRegistrar> &registra) :
                _registra(registra)
            {

            }

            !CefSchemeRegistrarWrapper()
            {
                _registra = NULL;
            }

            ~CefSchemeRegistrarWrapper()
            {
                this->!CefSchemeRegistrarWrapper();

                _disposed = true;
            }

            virtual bool AddCustomScheme(String^ schemeName, bool isStandard, bool isLocal, bool isDisplayIsolated, bool isSecure, bool isCorsEnabled, bool isCspBypassing)
            {
                ThrowIfDisposed();

                return _registra->AddCustomScheme(StringUtils::ToNative(schemeName), isStandard, isLocal, isDisplayIsolated, isSecure, isCorsEnabled, isCspBypassing);
            }
        };
    }
}
