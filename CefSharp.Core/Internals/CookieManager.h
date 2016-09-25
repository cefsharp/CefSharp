// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_cookie.h"
#include "CefCompletionCallbackAdapter.h"

using namespace System::Threading::Tasks;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CookieManager : public ICookieManager
        {
        private:
            MCefRefPtr<CefCookieManager> _cookieManager;

            void ThrowIfDisposed();
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

            CookieManager(const CefRefPtr<CefCookieManager> &cookieManager)
                :_cookieManager(cookieManager.get())
            {

            }

            !CookieManager()
            {
                this->_cookieManager = nullptr;
            }

            ~CookieManager()
            {
                this->!CookieManager();
            }

            virtual Task<bool>^ DeleteCookiesAsync(String^ url, String^ name);
            virtual Task<bool>^ SetCookieAsync(String^ url, Cookie^ cookie);
            virtual bool SetStoragePath(String^ path, bool persistSessionSookies);
            virtual void SetSupportedSchemes(... cli::array<String^>^ schemes);
            virtual Task<List<Cookie^>^>^ VisitAllCookiesAsync();
            virtual bool VisitAllCookies(ICookieVisitor^ visitor);
            virtual Task<List<Cookie^>^>^ VisitUrlCookiesAsync(String^ url, bool includeHttpOnly);
            virtual bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor);
            virtual Task<bool>^ FlushStoreAsync();

            operator CefRefPtr<CefCookieManager>()
            {
                if (this == nullptr)
                {
                    return NULL;
                }
                return _cookieManager.get();
            }
        };
    }
}