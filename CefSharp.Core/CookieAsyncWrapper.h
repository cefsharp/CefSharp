// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    private ref class CookieAsyncWrapper
    {
        String^ _url;
        String^ _name;
        String^ _value;
        String^ _domain;
        String^ _path;
        bool _secure;
        bool _httponly;
        bool _hasExpires;
        DateTime _expires;

    public:
        //Used for SetCookies
        CookieAsyncWrapper(String^ url, String^ name, String^ value, String^ domain, String^ path, bool secure, bool httponly, bool hasExpires, DateTime expires) :
            _url(url), _name(name), _value(value), _domain(domain), _path(path), _secure(secure), _httponly(httponly), _hasExpires(hasExpires), _expires(expires)
        {
        }

        //Used for DeleteCookies
        CookieAsyncWrapper(String^ url, String^ name) : _url(url), _name(name)
        {
        }

        bool SetCookie()
        {
            CefCookie cookie;
            StringUtils::AssignNativeFromClr(cookie.name, _name);
            StringUtils::AssignNativeFromClr(cookie.value, _value);
            StringUtils::AssignNativeFromClr(cookie.domain, _domain);
            StringUtils::AssignNativeFromClr(cookie.path, _path);
            cookie.secure = _secure;
            cookie.httponly = _httponly;
            cookie.has_expires = _hasExpires;
            cookie.expires.year = _expires.Year;
            cookie.expires.month = _expires.Month;
            cookie.expires.day_of_month = _expires.Day;
            cookie.expires.hour = _expires.Hour;
            cookie.expires.minute = _expires.Minute;
            cookie.expires.second = _expires.Second;
            cookie.expires.millisecond = _expires.Millisecond;

            return CefCookieManager::GetGlobalManager(NULL)->SetCookie(StringUtils::ToNative(_url), cookie, NULL);
        }

        bool DeleteCookies()
        {
            return CefCookieManager::GetGlobalManager(NULL)->DeleteCookies(StringUtils::ToNative(_url), StringUtils::ToNative(_name), NULL);
        }
    };
}