// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_cookie.h"
#include "internals\CefCompletionCallbackAdapter.h"

namespace CefSharp
{
    public ref class CookieManager : public ICookieManager
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
        ///
        // Creates a new cookie manager. If |path| is empty data will be stored in
        // memory only. Otherwise, data will be stored at the specified |path|. To
        // persist session cookies (cookies without an expiry date or validity
        // interval) set |persist_session_cookies| to true. Session cookies are
        // generally intended to be transient and most Web browsers do not persist
        // them. If |callback| is non-NULL it will be executed asnychronously on the
        // IO thread after the manager's storage has been initialized.
        ///
        /*--cef(optional_param=path,optional_param=callback)--*/
        CookieManager(String^ path, bool persistSessionCookies, ICompletionCallback^ callback)
        {
            CefRefPtr<CefCompletionCallback> wrapper = callback == nullptr ? NULL : new CefCompletionCallbackAdapter(callback);

            _cookieManager = CefCookieManager::CreateManager(StringUtils::ToNative(path), persistSessionCookies, wrapper);
        }

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
        virtual bool SetStoragePath(String^ path, bool persistSessionSookies, ICompletionCallback^ callback);
        virtual void SetSupportedSchemes(cli::array<String^>^ schemes, ICompletionCallback^ callback);
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