#include "stdafx.h"

#include "CookieVisitor.h"

namespace CefSharp
{
    bool CookieVisitor::Visit(const CefCookie& cookie, int count, int total, bool& deleteCookie)
    {
        return _visitor->Visit(gcnew Cookie(), count, total, deleteCookie);
    }
}