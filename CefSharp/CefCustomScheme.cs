using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public class CefCustomScheme
    {
        public string SchemeName { get; set; }
        public string DomainName { get; set; }
        public bool IsStandard { get; set; }
        public ISchemeHandlerFactory SchemeHandlerFactory { get; set; }

        public CefCustomScheme()
        {
            IsStandard = true;
        }
    }
}
