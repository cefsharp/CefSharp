#include "Stdafx.h"
#pragma once

#include "include/cef_cookie.h"
#include "ICookieVisitor.h"

namespace CefSharp
{
    public class CookieVisitor : public CefCookieVisitor, public AppDomainSafeCefBase
    {
    private:
        gcroot<ICookieVisitor^> _visitor;

    public:
        CookieVisitor(ICookieVisitor^ visitor)
        {
            _visitor = visitor;
        }

        virtual bool Visit(const CefCookie& cookie, int count, int total, bool& deleteCookie) OVERRIDE;

        IMPLEMENT_SAFE_REFCOUNTING(CookieVisitor);
    };
}