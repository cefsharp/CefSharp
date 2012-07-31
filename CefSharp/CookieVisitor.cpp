#include "stdafx.h"

#include "CookieVisitor.h"

namespace CefSharp
{
    bool CookieVisitor::Visit(const CefCookie& cefCookie, int count, int total, bool& deleteCookie)
    {
        Cookie^ cookie = gcnew Cookie();
        cookie->Name = toClr(cefCookie.name);
        cookie->Value = toClr(cefCookie.value);
        cookie->Domain = toClr(cefCookie.domain);
        cookie->Path = toClr(cefCookie.path);
        cookie->Secure = cefCookie.secure;
        cookie->HttpOnly = cefCookie.httponly;

        try
        {
            cookie->Expires = DateTime(cefCookie.expires.year,
                cefCookie.expires.month, cefCookie.expires.day_of_month);
        }
        catch (Exception^ ex)
        {

        }

        return _visitor->Visit(cookie, count, total, deleteCookie);
    }
}