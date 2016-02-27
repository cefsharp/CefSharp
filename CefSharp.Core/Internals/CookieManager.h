// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_cookie.h"

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
            virtual bool VisitAllCookies(ICookieVisitor^ visitor);
            virtual bool VisitUrlCookies(String^ url, bool includeHttpOnly, ICookieVisitor^ visitor);
            virtual Task<bool>^ FlushStoreAsync();
        };
    }
}