using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public abstract class CefAppBase : ObjectBase
    {
        public abstract void OnBrowserCreated(CefBrowserBase cefBrowserWrapper);
    }
}
