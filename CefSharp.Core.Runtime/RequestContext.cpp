// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "RequestContext.h"

#include "include\cef_parser.h"

//For the x86 version we define and undefine CEF_INCLUDE_BASE_INTERNAL_CEF_BIND_INTERNAL_WIN_H_
//as the /clr compliation option attempts to be helpful and convers all the __fastcall versions
//to __stdcall which already exist, so we just use the standard calling convention and ignore
//the optimised ones. The original error is
//warning C4561: '__fastcall' incompatible with the '/clr' option: converting to '__stdcall'
//(compiling source file RequestContext.cpp) 
#define CEF_INCLUDE_BASE_INTERNAL_CEF_BIND_INTERNAL_WIN_H_
#include "include\base\cef_bind.h"
#undef CEF_INCLUDE_BASE_INTERNAL_CEF_BIND_INTERNAL_WIN_H_

#include "include\wrapper\cef_closure_task.h"

#include "CookieManager.h"
#include "Internals\CefSchemeHandlerFactoryAdapter.h"
#include "Internals\CefCompletionCallbackAdapter.h"
#include "Internals\CefExtensionWrapper.h"
#include "Internals\CefExtensionHandlerAdapter.h"
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

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

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

        void RequestContext::PurgePluginListCache(bool reloadPages)
        {
            ThrowIfDisposed();

            _requestContext->PurgePluginListCache(reloadPages);
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

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

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

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

            _requestContext->ClearHttpAuthCredentials(wrapper);
        }

        void RequestContext::CloseAllConnections(ICompletionCallback^ callback)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

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

        bool RequestContext::DidLoadExtension(String^ extensionId)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            return _requestContext->DidLoadExtension(StringUtils::ToNative(extensionId));
        }

        IExtension^ RequestContext::GetExtension(String^ extensionId)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            auto extension = _requestContext->GetExtension(StringUtils::ToNative(extensionId));

            if (extension.get())
            {
                return gcnew CefExtensionWrapper(extension);
            }

            return nullptr;
        }

        bool RequestContext::GetExtensions([Out] IList<String^>^ %extensionIds)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            std::vector<CefString> extensions;

            auto success = _requestContext->GetExtensions(extensions);

            extensionIds = StringUtils::ToClr(extensions);

            return success;
        }

        bool RequestContext::HasExtension(String^ extensionId)
        {
            ThrowIfDisposed();

            ThrowIfExecutedOnNonCefUiThread();

            return _requestContext->HasExtension(StringUtils::ToNative(extensionId));
        }

        void RequestContext::LoadExtension(String^ rootDirectory, String^ manifestJson, IExtensionHandler^ handler)
        {
            ThrowIfDisposed();

            CefRefPtr<CefDictionaryValue> manifest;

            if (!String::IsNullOrEmpty(manifestJson))
            {
                CefString errorMessage;
                auto value = CefParseJSONAndReturnError(StringUtils::ToNative(manifestJson),
                    cef_json_parser_options_t::JSON_PARSER_ALLOW_TRAILING_COMMAS,
                    errorMessage);

                if (value.get())
                {
                    manifest = value->GetDictionary();
                }
                else
                {
                    throw gcnew Exception("Unable to parse JSON - ErrorMessage:" + StringUtils::ToClr(errorMessage));
                }
            }

            CefRefPtr<CefExtensionHandler> extensionHandler = handler == nullptr ? NULL : new CefExtensionHandlerAdapter(handler);

            if (CefCurrentlyOn(CefThreadId::TID_UI))
            {
                _requestContext->LoadExtension(StringUtils::ToNative(rootDirectory), manifest, extensionHandler);
            }
            else
            {
                CefPostTask(TID_UI, base::Bind(&CefRequestContext::LoadExtension, _requestContext.get(), StringUtils::ToNative(rootDirectory), manifest, extensionHandler));
            }
        }

        IRequestContext^ RequestContext::UnWrap()
        {
            return this;
        }
    }
}
