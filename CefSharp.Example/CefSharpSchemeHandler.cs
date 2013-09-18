using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    internal class CefSharpSchemeHandler : ISchemeHandler
    {
        private readonly IDictionary<string, string> resources;

        public CefSharpSchemeHandler()
        {
            resources = new Dictionary<string, string>
            {
                { "/", Resources.Home },
                { "/bootstrap/bootstrap-theme.min.css", Resources.bootstrap_theme_min_css },
                { "/bootstrap/bootstrap.min.css", Resources.bootstrap_min_css },
                { "/bootstrap/bootstrap.min.js", Resources.bootstrap_min_js },

                { "/BindingTest.html", Resources.BindingTest },
                { "/PopupTest.html", Resources.PopupTest },
                { "/SchemeTest.html", Resources.SchemeTest },
                { "/TooltipTest.html", Resources.TooltipTest },
            };
        }

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            string resource;
            if (resources.TryGetValue(fileName, out resource) &&
                !String.IsNullOrEmpty(resource))
            {
                var bytes = Encoding.UTF8.GetBytes(resource);
                stream = new MemoryStream(bytes);
                mimeType = GetMimeType(fileName);
                
                return true;
            }

            return false;
        }

        private string GetMimeType(string fileName)
        {
            if (fileName.EndsWith(".css")) return "text/css";
            if (fileName.EndsWith(".js")) return "text/javascript";
            
            return "text/html";
        }
    }
}
