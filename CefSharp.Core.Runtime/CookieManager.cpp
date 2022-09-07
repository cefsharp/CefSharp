// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CookieManager.h"

#include "Internals\CefCookieVisitorAdapter.h"
#include "Internals\CefCompletionCallbackAdapter.h"
#include "Internals\CefSetCookieCallbackAdapter.h"
#include "Internals\CefDeleteCookiesCallbackAdapter.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    void CookieManager::ThrowIfDisposed()
    {
        if (_cookieManager.get() == nullptr)
        {
            throw gcnew ObjectDisposedException("CookieManager");
        }
    }

    bool CookieManager::DeleteCookies(String^ url, String^ name, IDeleteCookiesCallback^ callback)
    {
        ThrowIfDisposed();

        CefRefPtr<CefDeleteCookiesCallback> wrapper = callback == nullptr ? nullptr : new CefDeleteCookiesCallbackAdapter(callback);

        return _cookieManager->DeleteCookies(StringUtils::ToNative(url), StringUtils::ToNative(name), wrapper);
    }

    bool CookieManager::SetCookie(String^ url, Cookie^ cookie, ISetCookieCallback^ callback)
    {
        ThrowIfDisposed();

        CefRefPtr<CefSetCookieCallback> wrapper = callback == nullptr ? nullptr : new CefSetCookieCallbackAdapter(callback);

        CefCookie c;
        StringUtils::AssignNativeFromClr(c.name, cookie->Name);
        StringUtils::AssignNativeFromClr(c.value, cookie->Value);
        StringUtils::AssignNativeFromClr(c.domain, cookie->Domain);
        StringUtils::AssignNativeFromClr(c.path, cookie->Path);
        c.secure = cookie->Secure;
        c.httponly = cookie->HttpOnly;
        c.has_expires = cookie->Expires.HasValue;

        if (cookie->Expires.HasValue)
        {
            c.expires.val = CefTimeUtils::FromDateTimeToBaseTime(cookie->Expires.Value);
        }

        // creation/last_access are basically readonly (assigned by Chromium when the cookie is created)
        // So I don't think we actually need to set them. The other option is to assign them to DateTime.Now
        // Issue #4234
        //c.creation.val = CefTimeUtils::FromDateTimeToBaseTime(cookie->Creation);
        //c.last_access.val = CefTimeUtils::FromDateTimeToBaseTime(cookie->LastAccess);
        c.same_site = (cef_cookie_same_site_t)cookie->SameSite;
        c.priority = (cef_cookie_priority_t)cookie->Priority;

        return _cookieManager->SetCookie(StringUtils::ToNative(url), c, wrapper);
    }

    bool CookieManager::VisitAllCookies(ICookieVisitor^ visitor)
    {
        ThrowIfDisposed();

        CefRefPtr<CefCookieVisitorAdapter> cookieVisitor = new CefCookieVisitorAdapter(visitor);

        return _cookieManager->VisitAllCookies(cookieVisitor);
    }

    bool CookieManager::VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor)
    {
        ThrowIfDisposed();

        CefRefPtr<CefCookieVisitorAdapter> cookieVisitor = new CefCookieVisitorAdapter(visitor);

        return _cookieManager->VisitUrlCookies(StringUtils::ToNative(url), includeHttpOnly, cookieVisitor);
    }

    bool CookieManager::FlushStore(ICompletionCallback^ callback)
    {
        ThrowIfDisposed();

        CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? nullptr : new CefCompletionCallbackAdapter(callback);

        return _cookieManager->FlushStore(wrapper);
    }
}
