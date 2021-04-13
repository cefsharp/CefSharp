// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_callback.h"

#include "CefBrowserWrapper.h"
#include "StringUtils.h"

using namespace CefSharp::Callback;
using namespace System::IO;

namespace CefSharp
{
    namespace Internals
    {
        private class CefDevToolsMessageObserverAdapter : public CefDevToolsMessageObserver
        {
        private:
            gcroot<IDevToolsMessageObserver^> _handler;

        public:
            CefDevToolsMessageObserverAdapter(IDevToolsMessageObserver^ handler)
            {
                _handler = handler;
            }

            ~CefDevToolsMessageObserverAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            virtual bool OnDevToolsMessage(CefRefPtr<CefBrowser> browser, const void* message, size_t message_size)
            {
                CefBrowserWrapper browserWrapper(browser);
                UnmanagedMemoryStream messageStream((Byte*)message, (Int64)message_size, (Int64)message_size, FileAccess::Read);

                return _handler->OnDevToolsMessage(%browserWrapper, %messageStream);
            }

            virtual void OnDevToolsMethodResult(CefRefPtr<CefBrowser> browser,
                int message_id,
                bool success,
                const void* result,
                size_t result_size) OVERRIDE
            {
                CefBrowserWrapper browserWrapper(browser);
                UnmanagedMemoryStream resultStream((Byte*)result, (Int64)result_size, (Int64)result_size, FileAccess::Read);

                _handler->OnDevToolsMethodResult(%browserWrapper, message_id, success, %resultStream);
            }

            virtual void OnDevToolsEvent(CefRefPtr<CefBrowser> browser, const CefString& method, const void* params, size_t params_size) OVERRIDE
            {
                CefBrowserWrapper browserWrapper(browser);
                UnmanagedMemoryStream paramsStream((Byte*)params, (Int64)params_size, (Int64)params_size, FileAccess::Read);

                _handler->OnDevToolsEvent(%browserWrapper, StringUtils::ToClr(method), %paramsStream);
            }

            void OnDevToolsAgentAttached(CefRefPtr<CefBrowser> browser)  OVERRIDE
            {
                CefBrowserWrapper browserWrapper(browser);

                _handler->OnDevToolsAgentAttached(%browserWrapper);
            }

            void OnDevToolsAgentDetached(CefRefPtr<CefBrowser> browser) OVERRIDE
            {
                CefBrowserWrapper browserWrapper(browser);

                _handler->OnDevToolsAgentDetached(%browserWrapper);
            }

            IMPLEMENT_REFCOUNTING(CefDevToolsMessageObserverAdapter);
        };
    }
}
