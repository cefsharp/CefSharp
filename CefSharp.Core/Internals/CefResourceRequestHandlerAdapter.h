// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_resource_request_handler.h"
#include "include\wrapper\cef_stream_resource_handler.h"

#include "CefResponseWrapper.h"
#include "CefRequestWrapper.h"
#include "CefFrameWrapper.h"
#include "CefSharpBrowserWrapper.h"
#include "ResourceHandlerWrapper.h"
#include "CefResponseFilterAdapter.h"
#include "CefRequestCallbackWrapper.h"
#include "CefCookieAccessFilterAdapter.h"

namespace CefSharp
{
    namespace Internals
    {
        //TODO: NetworkService - Code duplication should be improved
        //going with the simplest and easiest option now
        private class CefResourceRequestHandlerAdapter : public CefResourceRequestHandler
        {
        private:
            gcroot<IResourceRequestHandler^> _handler;
            gcroot<IWebBrowser^> _browserControl;

        public:
            CefResourceRequestHandlerAdapter(IWebBrowser^ browserControl, IResourceRequestHandler^ handler) :
                _handler(handler), _browserControl(browserControl)
            {

            }

            ~CefResourceRequestHandlerAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            CefRefPtr<CefCookieAccessFilter> GetCookieAccessFilter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request) OVERRIDE
            {
                ICookieAccessFilter^ accessFilter;
                CefRequestWrapper requestWrapper(request);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    accessFilter = _handler->GetCookieAccessFilter(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper);
                }
                else
                {
                    accessFilter = _handler->GetCookieAccessFilter(_browserControl, nullptr, nullptr, %requestWrapper);
                }

                if (accessFilter == nullptr)
                {
                    return NULL;
                }

                return new CefCookieAccessFilterAdapter(accessFilter, _browserControl);
            }

            cef_return_value_t OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefRequestCallback> callback) OVERRIDE
            {
                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    //TODO: We previously used GetBrowserWrapper - investigate passing in reference to this adapter
                    CefSharpBrowserWrapper browserWrapper(browser);
                    //We pass the frame and request wrappers to CefRequestCallbackWrapper so they can be disposed of
                    //when the callback is executed
                    auto frameWrapper = gcnew CefFrameWrapper(frame);
                    auto requestWrapper = gcnew CefRequestWrapper(request);
                    auto requestCallback = gcnew CefRequestCallbackWrapper(callback, frameWrapper, requestWrapper);

                    return (cef_return_value_t)_handler->OnBeforeResourceLoad(_browserControl, %browserWrapper, frameWrapper, requestWrapper, requestCallback);
                }

                auto requestWrapper = gcnew CefRequestWrapper(request);
                auto requestCallback = gcnew CefRequestCallbackWrapper(callback, nullptr, requestWrapper);

                return (cef_return_value_t)_handler->OnBeforeResourceLoad(_browserControl, nullptr, nullptr, requestWrapper, requestCallback);
            }

            CefRefPtr<CefResourceHandler> GetResourceHandler(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request) OVERRIDE
            {
                IResourceHandler^ resourceHandler;
                CefRequestWrapper requestWrapper(request);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    resourceHandler = _handler->GetResourceHandler(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper);
                }
                else
                {
                    resourceHandler = _handler->GetResourceHandler(_browserControl, nullptr, nullptr, %requestWrapper);
                }

                if (resourceHandler == nullptr)
                {
                    return NULL;
                }

                if (resourceHandler->GetType() == FileResourceHandler::typeid)
                {
                    auto fileResourceHandler = static_cast<FileResourceHandler^>(resourceHandler);

                    auto streamReader = CefStreamReader::CreateForFile(StringUtils::ToNative(fileResourceHandler->FilePath));

                    if (streamReader.get())
                    {
                        return new CefStreamResourceHandler(StringUtils::ToNative(fileResourceHandler->MimeType), streamReader);
                    }
                    else
                    {
                        auto msg = "Unable to load resource CefStreamReader::CreateForFile returned NULL for file:" + fileResourceHandler->FilePath;
                        LOG(ERROR) << StringUtils::ToNative(msg).ToString();

                        return NULL;
                    }
                }
                else if (resourceHandler->GetType() == ByteArrayResourceHandler::typeid)
                {
                    auto byteArrayResourceHandler = static_cast<ByteArrayResourceHandler^>(resourceHandler);

                    //NOTE: Prefix with cli:: namespace as VS2015 gets confused with std::array
                    cli::array<Byte>^ buffer = byteArrayResourceHandler->Data;
                    pin_ptr<Byte> src = &buffer[0];

                    auto streamReader = CefStreamReader::CreateForData(static_cast<void*>(src), buffer->Length);

                    return new CefStreamResourceHandler(StringUtils::ToNative(byteArrayResourceHandler->MimeType), streamReader);
                }

                return new ResourceHandlerWrapper(resourceHandler);
            }

            void OnResourceRedirect(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response, CefString& newUrl) OVERRIDE
            {
                auto managedNewUrl = StringUtils::ToClr(newUrl);
                CefRequestWrapper requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    _handler->OnResourceRedirect(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, managedNewUrl);
                }
                else
                {
                    _handler->OnResourceRedirect(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper, managedNewUrl);
                }

                newUrl = StringUtils::ToNative(managedNewUrl);

            }

            bool OnResourceResponse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response) OVERRIDE
            {
                CefRequestWrapper requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {

                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    return _handler->OnResourceResponse(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
                }

                return _handler->OnResourceResponse(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper);
            }

            CefRefPtr<CefResponseFilter> GetResourceResponseFilter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response) OVERRIDE
            {
                IResponseFilter^ responseFilter;
                CefRequestWrapper requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    responseFilter = _handler->GetResourceResponseFilter(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
                }
                else
                {
                    responseFilter = _handler->GetResourceResponseFilter(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper);
                }

                if (responseFilter == nullptr)
                {
                    return NULL;
                }

                return new CefResponseFilterAdapter(responseFilter);
            }

            void OnResourceLoadComplete(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response, URLRequestStatus status, int64 receivedContentLength) OVERRIDE
            {
                CefRequestWrapper requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);

                    _handler->OnResourceLoadComplete(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, (UrlRequestStatus)status, receivedContentLength);
                }
                else
                {
                    _handler->OnResourceLoadComplete(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper, (UrlRequestStatus)status, receivedContentLength);
                }
            }

            void OnProtocolExecution(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool& allowOSExecution) OVERRIDE
            {
                CefRequestWrapper requestWrapper(request);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    CefSharpBrowserWrapper browserWrapper(browser);
                    CefFrameWrapper frameWrapper(frame);


                    allowOSExecution = _handler->OnProtocolExecution(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper);
                }
                else
                {

                    allowOSExecution = _handler->OnProtocolExecution(_browserControl, nullptr, nullptr, %requestWrapper);
                }
            }

            IMPLEMENT_REFCOUNTING(CefResourceRequestHandlerAdapter);
        };
    }
}
