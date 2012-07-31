#include "stdafx.h"
#pragma once

using namespace System::Net;

namespace CefSharp
{
    public interface class ICookieVisitor
    {
    public:
        bool Visit(Cookie^ cookie, int count, int total, bool%  deleteCookie);
    };
}