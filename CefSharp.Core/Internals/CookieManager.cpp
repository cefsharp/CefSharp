// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CookieManager.h"

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

        bool CookieManager::DisposeAsyncWrapper(Task<bool>^ task)
        {
            auto asyncWrapper = dynamic_cast<CookieAsyncWrapper^>(task->AsyncState);
            if (asyncWrapper != nullptr)
            {
                delete asyncWrapper;
            }

            return task->Result;
        }

        Task<bool>^ CookieManager::DeleteCookiesAsync(String^ url, String^ name)
        {
            ThrowIfDisposed();

            auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, name);

            Task<bool>^ task;
            if (CefCurrentlyOn(TID_IO))
            {
                auto source = gcnew TaskCompletionSource<bool>(cookieInvoker);
                source->TrySetResult(cookieInvoker->DeleteCookies(nullptr));
                task = source->Task;
            }
            else
            {
                task = Cef::IOThreadTaskFactory->StartNew(gcnew Func<Object^, bool>(cookieInvoker, &CookieAsyncWrapper::DeleteCookies), cookieInvoker);
            }

            return task->ContinueWith(gcnew Func<Task<bool>^, bool>(this, &CookieManager::DisposeAsyncWrapper), TaskScheduler::Default);
        }

        Task<bool>^ CookieManager::SetCookieAsync(String^ url, Cookie^ cookie)
        {
            ThrowIfDisposed();

            auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, cookie->Name, cookie->Value, cookie->Domain, cookie->Path, cookie->Secure, cookie->HttpOnly, cookie->Expires.HasValue, cookie->Expires.HasValue ? cookie->Expires.Value : DateTime());

            Task<bool>^ task;
            if (CefCurrentlyOn(TID_IO))
            {
                auto source = gcnew TaskCompletionSource<bool>(cookieInvoker);
                source->TrySetResult(cookieInvoker->SetCookie(nullptr));
                task = source->Task;
            }
            else
            {
                task = Cef::IOThreadTaskFactory->StartNew(gcnew Func<Object^, bool>(cookieInvoker, &CookieAsyncWrapper::SetCookie), cookieInvoker);
            }

            return task->ContinueWith(gcnew Func<Task<bool>^, bool>(this, &CookieManager::DisposeAsyncWrapper), TaskScheduler::Default);
        }

        bool CookieManager::SetStoragePath(String^ path, bool persistSessionCookies)
        {
            ThrowIfDisposed();

            return _cookieManager->SetStoragePath(StringUtils::ToNative(path), persistSessionCookies, NULL);
        }

        void CookieManager::SetSupportedSchemes(... array<String^>^ schemes)
        {
            ThrowIfDisposed();

            _cookieManager->SetSupportedSchemes(StringUtils::ToNative(schemes), NULL);
        }

        bool CookieManager::VisitAllCookies(ICookieVisitor^ visitor)
        {
            ThrowIfDisposed();

            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);

            return _cookieManager->VisitAllCookies(cookieVisitor);
        }

        bool CookieManager::VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor)
        {
            ThrowIfDisposed();

            CefRefPtr<CookieVisitor> cookieVisitor = new CookieVisitor(visitor);

            return _cookieManager->VisitUrlCookies(StringUtils::ToNative(url), includeHttpOnly, cookieVisitor);
        }

        bool CookieManager::FlushStore(ICompletionHandler^ handler)
        {
            ThrowIfDisposed();

            CefRefPtr<CefCompletionCallback> wrapper = new CompletionHandler(handler);

            return _cookieManager->FlushStore(wrapper);
        }
    }
}