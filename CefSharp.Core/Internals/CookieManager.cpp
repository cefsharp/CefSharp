// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CookieManager.h"

#include "CookieAsyncWrapper.h"
#include "CookieVisitor.h"
#include "Internals\CefCompletionCallbackAdapter.h"
#include "Cef.h"

namespace CefSharp
{
    namespace Internals
    {
        void CookieManager::ThrowIfDisposed()
        {
            if (_cookieManager.get() == nullptr)
            {
                throw gcnew ObjectDisposedException("CookieManager");
            }
        }

        Task<bool>^ CookieManager::DeleteCookiesAsync(String^ url, String^ name)
        {
            ThrowIfDisposed();

            auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, name);

            if (CefCurrentlyOn(TID_IO))
            {
                auto source = gcnew TaskCompletionSource<bool>();
                TaskExtensions::TrySetResultAsync<bool>(source, cookieInvoker->DeleteCookies());
                return source->Task;
            }

            return Cef::IOThreadTaskFactory->StartNew(gcnew Func<bool>(cookieInvoker, &CookieAsyncWrapper::DeleteCookies));
        }

        Task<bool>^ CookieManager::SetCookieAsync(String^ url, Cookie^ cookie)
        {
            ThrowIfDisposed();

            auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, cookie->Name, cookie->Value, cookie->Domain, cookie->Path, cookie->Secure, cookie->HttpOnly, cookie->Expires.HasValue, cookie->Expires.HasValue ? cookie->Expires.Value : DateTime());

            if (CefCurrentlyOn(TID_IO))
            {
                auto source = gcnew TaskCompletionSource<bool>();
                TaskExtensions::TrySetResultAsync<bool>(source, cookieInvoker->SetCookie());
                return source->Task;
            }

            return Cef::IOThreadTaskFactory->StartNew(gcnew Func<bool>(cookieInvoker, &CookieAsyncWrapper::SetCookie));
        }

        bool CookieManager::SetStoragePath(String^ path, bool persistSessionCookies)
        {
            ThrowIfDisposed();

            return _cookieManager->SetStoragePath(StringUtils::ToNative(path), persistSessionCookies, NULL);
        }

        void CookieManager::SetSupportedSchemes(... cli::array<String^>^ schemes)
        {
            ThrowIfDisposed();

            _cookieManager->SetSupportedSchemes(StringUtils::ToNative(schemes), NULL);
        }

        Task<List<Cookie^>^>^ CookieManager::VisitAllCookiesAsync()
        {
            ThrowIfDisposed();

            auto cookieVisitor = gcnew TaskCookieVisitor();

            auto result = VisitAllCookies(cookieVisitor);

            if (result == false)
            {
                delete cookieVisitor;
            }

            return cookieVisitor->Task;
        }

        bool CookieManager::VisitAllCookies(ICookieVisitor^ visitor)
        {
            ThrowIfDisposed();

            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);

            return _cookieManager->VisitAllCookies(cookieVisitor);
        }

        Task<List<Cookie^>^>^ CookieManager::VisitUrlCookiesAsync(String^ url, bool includeHttpOnly)
        {
            ThrowIfDisposed();

            auto cookieVisitor = gcnew TaskCookieVisitor();

            auto result = VisitUrlCookies(url, includeHttpOnly, cookieVisitor);

            if (result == false)
            {
                delete cookieVisitor;
            }

            return cookieVisitor->Task;
        }

        bool CookieManager::VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor)
        {
            ThrowIfDisposed();

            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);

            return _cookieManager->VisitUrlCookies(StringUtils::ToNative(url), includeHttpOnly, cookieVisitor);
        }

        Task<bool>^ CookieManager::FlushStoreAsync()
        {
            ThrowIfDisposed();

            auto handler = gcnew TaskCompletionHandler();

            CefRefPtr<CefCompletionCallback> wrapper = new CefCompletionCallbackAdapter(handler);

            if (_cookieManager->FlushStore(wrapper))
            {
                return handler->Task;
            }

            //returns false if cookies cannot be accessed.
            return TaskExtensions::FromResult(false);
        }
    }
}