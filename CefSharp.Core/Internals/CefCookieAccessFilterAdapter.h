// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_resource_request_handler.h"

#include "CefResponseWrapper.h"
#include "Request.h"
#include "CefFrameWrapper.h"
#include "CefBrowserWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefCookieAccessFilterAdapter : public CefCookieAccessFilter
        {
        private:
            gcroot<ICookieAccessFilter^> _handler;
            gcroot<IWebBrowser^> _browserControl;

        public:
            CefCookieAccessFilterAdapter(ICookieAccessFilter^ handler, IWebBrowser^ browserControl)
            {
                _handler = handler;
                _browserControl = browserControl;
            }

            ~CefCookieAccessFilterAdapter()
            {
                delete _handler;
                _handler = nullptr;
                _browserControl = nullptr;
            }


            bool CanSendCookie(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, const CefCookie& cookie) OVERRIDE
            {
                Request requestWrapper(request);
                auto managedCookie = TypeConversion::FromNative(cookie);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {

                    CefBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    return _handler->CanSendCookie(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, managedCookie);
                }

                return _handler->CanSendCookie(_browserControl, nullptr, nullptr, %requestWrapper, managedCookie);
            }

            bool CanSaveCookie(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response, const CefCookie& cookie) OVERRIDE
            {
                Request requestWrapper(request);
                CefResponseWrapper responseWrapper(response);
                auto managedCookie = TypeConversion::FromNative(cookie);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {

                    CefBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    return _handler->CanSaveCookie(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, managedCookie);
                }

                return _handler->CanSaveCookie(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper, managedCookie);
            }

            IMPLEMENT_REFCOUNTING(CefCookieAccessFilterAdapter);
        };
    }
}
