// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefRequestContextHandlerAdapter.h"
#include "CookieManager.h"
#include "Request.h"
#include "RequestContext.h"
#include "Internals\ReportUnhandledExceptions.h"
#include "Internals\TypeConversion.h"
#include "Internals\CefBrowserWrapper.h"
#include "Internals\CefFrameWrapper.h"
#include "Internals\CefResourceRequestHandlerAdapter.h"

using namespace CefSharp::Core;

namespace CefSharp
{
    namespace Internals
    {
        void CefRequestContextHandlerAdapter::OnRequestContextInitialized(CefRefPtr<CefRequestContext> requestContext)
        {
            if (!Object::ReferenceEquals(_requestContextHandler, nullptr))
            {
                RequestContext ctx(requestContext);

                _requestContextHandler->OnRequestContextInitialized(%ctx);
            }
        }

        CefRefPtr<CefResourceRequestHandler> CefRequestContextHandlerAdapter::GetResourceRequestHandler(
            CefRefPtr<CefBrowser> browser,
            CefRefPtr<CefFrame> frame,
            CefRefPtr<CefRequest> request,
            bool is_navigation,
            bool is_download,
            const CefString& request_initiator,
            bool& disable_default_handling)
        {
            if (Object::ReferenceEquals(_requestContextHandler, nullptr))
            {

                return nullptr;
            }

            CefBrowserWrapper browserWrapper(browser);
            CefFrameWrapper frameWrapper(frame);
            Request requestWrapper(request);

            auto handler = _requestContextHandler->GetResourceRequestHandler(%browserWrapper, %frameWrapper, %requestWrapper, is_navigation, is_download, StringUtils::ToClr(request_initiator), disable_default_handling);

            if (Object::ReferenceEquals(handler, nullptr))
            {
                return nullptr;
            }

            //CefRequestContext is not associated with a specific browser
            //so browserControl param is always nullptr.
            return new CefResourceRequestHandlerAdapter(nullptr, handler);
        }
    }
}
