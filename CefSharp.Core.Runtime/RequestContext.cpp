// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "RequestContext.h"

#include "include\cef_parser.h"

#include "CookieManager.h"
#include "Internals\CefSchemeHandlerFactoryAdapter.h"
#include "Internals\CefCompletionCallbackAdapter.h"
#include "Internals\CefResolveCallbackAdapter.h"
#include "Internals\TypeConversion.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace Core
    {
        bool RequestContext::IsSame(IRequestContext^ context)
        {
            ThrowIfDisposed();

            auto requestContext = (RequestContext^)context->UnWrap();

            return _requestContext->IsSame(requestContext);
        }

        bool RequestContext::IsSharingWith(IRequestContext^ context)
        {
            ThrowIfDisposed();

            auto requestContext = (RequestContext^)context->UnWrap();

            return _requestContext->IsSharingWith(requestContext);
        }

        ICookieManager^ RequestContext::GetCookieManager(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? nullptr : new CefCompletionCallbackAdapter(callback);

            auto cookieManager = _requestContext->GetCookieManager(wrapper);
            if (cookieManager.get())
            {
                return gcnew CookieManager(cookieManager);
            }
            return nullptr;
        }

        bool RequestContext::RegisterSchemeHandlerFactory(String^ schemeName, String^ domainName, ISchemeHandlerFactory^ factory)
        {
            ThrowIfDisposed();

            auto wrapper = new CefSchemeHandlerFactoryAdapter(factory);
            return _requestContext->RegisterSchemeHandlerFactory(StringUtils::ToNative(schemeName), StringUtils::ToNative(domainName), wrapper);
        }

        bool RequestContext::ClearSchemeHandlerFactories()
        {
            ThrowIfDisposed();

            return _requestContext->ClearSchemeHandlerFactories();
        }

        bool RequestContext::HasPreference(String^ name)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            return _requestContext->HasPreference(StringUtils::ToNative(name));
        }

        Object^ RequestContext::GetPreference(String^ name)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            return TypeConversion::FromNative(_requestContext->GetPreference(StringUtils::ToNative(name)));
        }

        IDictionary<String^, Object^>^ RequestContext::GetAllPreferences(bool includeDefaults)
        {
            ThrowIfDisposed();

            auto preferences = _requestContext->GetAllPreferences(includeDefaults);

            return TypeConversion::FromNative(preferences);
        }

        bool RequestContext::CanSetPreference(String^ name)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            return _requestContext->CanSetPreference(StringUtils::ToNative(name));
        }

        bool RequestContext::SetPreference(String^ name, Object^ value, [Out] String^ %error)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            CefString cefError;

            auto success = _requestContext->SetPreference(StringUtils::ToNative(name), TypeConversion::ToNative(value), cefError);

            error = StringUtils::ToClr(cefError);

            return success;
        }

        void RequestContext::ClearCertificateExceptions(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? nullptr : new CefCompletionCallbackAdapter(callback);

            _requestContext->ClearCertificateExceptions(wrapper);
        }

        void RequestContext::ClearHttpAuthCredentials(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            //TODO: Remove this once CEF Issue has been resolved
            //ClearHttpAuthCredentials crashes when no callback specified
            if (callback == nullptr)
            {
                callback = gcnew CefSharp::Callback::NoOpCompletionCallback();
            }

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? nullptr : new CefCompletionCallbackAdapter(callback);

            _requestContext->ClearHttpAuthCredentials(wrapper);
        }

        void RequestContext::CloseAllConnections(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? nullptr : new CefCompletionCallbackAdapter(callback);

            _requestContext->CloseAllConnections(wrapper);
        }

        Task<ResolveCallbackResult>^ RequestContext::ResolveHostAsync(Uri^ origin)
        {
            ThrowIfDisposed();

            auto callback = gcnew TaskResolveCallback();

            CefRefPtr<CefResolveCallback> callbackWrapper = new CefResolveCallbackAdapter(callback);

            _requestContext->ResolveHost(StringUtils::ToNative(origin->AbsoluteUri), callbackWrapper);

            return callback->Task;
        }

        IRequestContext^ RequestContext::UnWrap()
        {
            return this;
        }
    }
}
