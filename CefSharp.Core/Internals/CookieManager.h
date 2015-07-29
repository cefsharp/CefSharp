// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "MCefRefPtr.h"
#include "include/cef_cookie.h"

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

            virtual bool DeleteCookies(String^ url, String^ name);
            virtual bool SetCookie(String^ url, Cookie^ cookie);
            virtual bool SetStoragePath(String^ path, bool persistSessionSookies);
            virtual void SetSupportedSchemes(... array<String^>^ schemes);
            virtual bool VisitAllCookies(ICookieVisitor^ visitor);
            virtual bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor);
            virtual bool FlushStore(ICompletionHandler^ handler);
        };
    }
}