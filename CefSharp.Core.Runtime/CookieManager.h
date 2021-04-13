// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_cookie.h"
#include "internals\CefCompletionCallbackAdapter.h"

namespace CefSharp
{
    //TODO: No longer possible for users to create a CookieManager, can be made private now
    /// <exclude />
    private ref class CookieManager : public ICookieManager
    {
    private:
        MCefRefPtr<CefCookieManager> _cookieManager;

        void ThrowIfDisposed();

    internal:
        CookieManager(const CefRefPtr<CefCookieManager> &cookieManager)
            :_cookieManager(cookieManager.get())
        {

        }

        operator CefRefPtr<CefCookieManager>()
        {
            if (this == nullptr)
            {
                return NULL;
            }
            return _cookieManager.get();
        }

    public:
        !CookieManager()
        {
            this->_cookieManager = nullptr;
        }

        ~CookieManager()
        {
            this->!CookieManager();
        }

        virtual bool DeleteCookies(String^ url, String^ name, IDeleteCookiesCallback^ callback);
        virtual bool SetCookie(String^ url, Cookie^ cookie, ISetCookieCallback^ callback);
        virtual bool VisitAllCookies(ICookieVisitor^ visitor);
        virtual bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor);
        virtual bool FlushStore(ICompletionCallback^ callback);

        virtual property bool IsDisposed
        {
            bool get()
            {
                return !_cookieManager.get();
            }
        }
    };
}
