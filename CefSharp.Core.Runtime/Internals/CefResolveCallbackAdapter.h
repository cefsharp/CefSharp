// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_request_context.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefResolveCallbackAdapter : public CefResolveCallback
        {
        private:
            gcroot<IResolveCallback^> _handler;

        public:
            CefResolveCallbackAdapter(IResolveCallback^ handler)
            {
                _handler = handler;
            }

            ~CefResolveCallbackAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            void OnResolveCompleted(cef_errorcode_t result, const std::vector<CefString>& resolvedIps) override
            {
                _handler->OnResolveCompleted((CefErrorCode)result, StringUtils::ToClr(resolvedIps));
            }

        public:
            void AddRef() const override {  }
            bool Release() const override { return false; }
            bool HasOneRef() const override { return false; }
            bool HasAtLeastOneRef() const override { return false; }
        };
    }
}
