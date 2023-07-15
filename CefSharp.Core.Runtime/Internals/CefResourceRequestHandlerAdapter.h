// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_resource_request_handler.h"
#include "include\wrapper\cef_stream_resource_handler.h"

#include "CefResponseWrapper.h"
#include "Request.h"
#include "CefFrameWrapper.h"
#include "CefBrowserWrapper.h"
#include "CefResourceHandlerAdapter.h"
#include "CefResponseFilterAdapter.h"
#include "CefRequestCallbackWrapper.h"
#include "CefCookieAccessFilterAdapter.h"

using namespace CefSharp::Core;

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
            //For resource requests that are handled by CefRequestContextHandlerAdapter::GetResourceRequestHandler
            //this will be false
            bool _hasAssociatedBrowserControl;

        public:
            CefResourceRequestHandlerAdapter(IWebBrowser^ browserControl, IResourceRequestHandler^ handler) :
                _handler(handler), _browserControl(browserControl)
            {
                _hasAssociatedBrowserControl = !Object::ReferenceEquals(_browserControl, nullptr);
            }

            ~CefResourceRequestHandlerAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            CefRefPtr<CefCookieAccessFilter> GetCookieAccessFilter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request) override
            {
                ICookieAccessFilter^ accessFilter;
                Request requestWrapper(request);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        accessFilter = _handler->GetCookieAccessFilter(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper);
                    }
                    else
                    {
                        CefBrowserWrapper browserWrapper(browser);

                        accessFilter = _handler->GetCookieAccessFilter(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper);
                    }
                }
                else
                {
                    accessFilter = _handler->GetCookieAccessFilter(_browserControl, nullptr, nullptr, %requestWrapper);
                }

                if (accessFilter == nullptr)
                {
                    return nullptr;
                }

                return new CefCookieAccessFilterAdapter(accessFilter, _browserControl);
            }

            cef_return_value_t OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback) override
            {
                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    //We pass the frame and request wrappers to CefRequestCallbackWrapper so they can be disposed of
                    //when the callback is executed
                    auto frameWrapper = gcnew CefFrameWrapper(frame);
                    auto requestWrapper = gcnew Request(request);
                    auto requestCallback = gcnew CefRequestCallbackWrapper(callback, frameWrapper, requestWrapper);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        return (cef_return_value_t)_handler->OnBeforeResourceLoad(_browserControl, existingBrowserWrapper, frameWrapper, requestWrapper, requestCallback);
                    }

                    CefBrowserWrapper browserWrapper(browser);

                    return (cef_return_value_t)_handler->OnBeforeResourceLoad(_browserControl, %browserWrapper, frameWrapper, requestWrapper, requestCallback);
                }

                auto requestWrapper = gcnew Request(request);
                auto requestCallback = gcnew CefRequestCallbackWrapper(callback, nullptr, requestWrapper);

                return (cef_return_value_t)_handler->OnBeforeResourceLoad(_browserControl, nullptr, nullptr, requestWrapper, requestCallback);
            }

            CefRefPtr<CefResourceHandler> GetResourceHandler(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request) override
            {
                IResourceHandler^ resourceHandler;
                Request requestWrapper(request);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        resourceHandler = _handler->GetResourceHandler(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper);
                    }
                    else
                    {
                        CefBrowserWrapper browserWrapper(browser);

                        resourceHandler = _handler->GetResourceHandler(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper);
                    }
                }
                else
                {
                    resourceHandler = _handler->GetResourceHandler(_browserControl, nullptr, nullptr, %requestWrapper);
                }

                if (resourceHandler == nullptr)
                {
                    return nullptr;
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
                        auto msg = "Unable to load resource CefStreamReader::CreateForFile returned nullptr for file:" + fileResourceHandler->FilePath;
                        LOG(ERROR) << StringUtils::ToNative(msg).ToString();

                        return nullptr;
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

                return new CefResourceHandlerAdapter(resourceHandler);
            }

            void OnResourceRedirect(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response, CefString& newUrl) override
            {
                auto managedNewUrl = StringUtils::ToClr(newUrl);
                Request requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        _handler->OnResourceRedirect(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, managedNewUrl);
                    }
                    else
                    {
                        CefBrowserWrapper browserWrapper(browser);

                        _handler->OnResourceRedirect(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, managedNewUrl);
                    }
                }
                else
                {
                    _handler->OnResourceRedirect(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper, managedNewUrl);
                }

                newUrl = StringUtils::ToNative(managedNewUrl);

            }

            bool OnResourceResponse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response) override
            {
                Request requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        return _handler->OnResourceResponse(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
                    }

                    CefBrowserWrapper browserWrapper(browser);

                    return _handler->OnResourceResponse(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
                }

                return _handler->OnResourceResponse(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper);
            }

            CefRefPtr<CefResponseFilter> GetResourceResponseFilter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response) override
            {
                IResponseFilter^ responseFilter;
                Request requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        responseFilter = _handler->GetResourceResponseFilter(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
                    }
                    else
                    {
                        CefBrowserWrapper browserWrapper(browser);

                        responseFilter = _handler->GetResourceResponseFilter(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
                    }
                }
                else
                {
                    responseFilter = _handler->GetResourceResponseFilter(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper);
                }

                if (responseFilter == nullptr)
                {
                    return nullptr;
                }

                return new CefResponseFilterAdapter(responseFilter);
            }

            void OnResourceLoadComplete(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response, URLRequestStatus status, int64_t receivedContentLength) override
            {
                Request requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        _handler->OnResourceLoadComplete(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, (UrlRequestStatus)status, receivedContentLength);
                    }
                    else
                    {
                        CefBrowserWrapper browserWrapper(browser);

                        _handler->OnResourceLoadComplete(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, (UrlRequestStatus)status, receivedContentLength);
                    }
                }
                else
                {
                    _handler->OnResourceLoadComplete(_browserControl, nullptr, nullptr, %requestWrapper, %responseWrapper, (UrlRequestStatus)status, receivedContentLength);
                }
            }

            void OnProtocolExecution(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool& allowOSExecution) override
            {
                Request requestWrapper(request);

                //For ServiceWorker browser and frame will be null
                if (browser.get() && frame.get())
                {
                    IBrowser^ existingBrowserWrapper;
                    CefFrameWrapper frameWrapper(frame);

                    // If we already have an IBrowser instance then use it, otherwise we'll pass in a scoped instance
                    // which cannot be accessed outside the scope of the method. We should under normal circumstanaces
                    // have a IBrowser reference already ready to use.
                    if (_hasAssociatedBrowserControl && _browserControl->TryGetBrowserCoreById(browser->GetIdentifier(), existingBrowserWrapper))
                    {
                        allowOSExecution = _handler->OnProtocolExecution(_browserControl, existingBrowserWrapper, %frameWrapper, %requestWrapper);
                    }
                    else
                    {
                        CefBrowserWrapper browserWrapper(browser);

                        allowOSExecution = _handler->OnProtocolExecution(_browserControl, %browserWrapper, %frameWrapper, %requestWrapper);
                    }
                }
                else
                {

                    allowOSExecution = _handler->OnProtocolExecution(_browserControl, nullptr, nullptr, %requestWrapper);
                }
            }

            IMPLEMENT_REFCOUNTINGM(CefResourceRequestHandlerAdapter);
        };
    }
}
