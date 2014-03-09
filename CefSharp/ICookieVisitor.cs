using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CefSharp
{
    public interface ICookieVisitor
    {
        bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie);
    }
}
