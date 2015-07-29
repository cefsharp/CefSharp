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

        bool CookieManager::DeleteCookies(String^ url, String^ name)
        {
            ThrowIfDisposed();

            auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, name);

            if (CefCurrentlyOn(TID_IO))
            {
                return cookieInvoker->DeleteCookies();
            }

            auto task = Cef::IOThreadTaskFactory->StartNew(gcnew Func<bool>(cookieInvoker, &CookieAsyncWrapper::DeleteCookies));

            task->Wait();
            delete cookieInvoker;

            return task->Result;
        }

        bool CookieManager::SetCookie(String^ url, Cookie^ cookie)
        {
            ThrowIfDisposed();

            auto cookieInvoker = gcnew CookieAsyncWrapper(_cookieManager.get(), url, cookie->Name, cookie->Value, cookie->Domain, cookie->Path, cookie->Secure, cookie->HttpOnly, cookie->Expires.HasValue, cookie->Expires.HasValue ? cookie->Expires.Value : DateTime());

            if (CefCurrentlyOn(TID_IO))
            {
                return cookieInvoker->SetCookie();
            }

            auto task = Cef::IOThreadTaskFactory->StartNew(gcnew Func<bool>(cookieInvoker, &CookieAsyncWrapper::SetCookie));

            task->Wait();
            delete cookieInvoker;

            return task->Result;
        }

        bool CookieManager::SetStoragePath(String^ path, bool persistSessionCookies)
        {
            ThrowIfDisposed();

            return _cookieManager->SetStoragePath(StringUtils::ToNative(path), persistSessionCookies, NULL);
        }

        void CookieManager::SetSupportedSchemes(... array<String^>^ schemes)
        {
            ThrowIfDisposed();

            vector<CefString> schemeVector;
            for each(String^ scheme in schemes)
            {
                schemeVector.push_back(StringUtils::ToNative(scheme));
            }

            _cookieManager->SetSupportedSchemes(schemeVector, NULL);
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
    }
}