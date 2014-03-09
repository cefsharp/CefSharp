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
                { "/home", Resources.home_html },

                { "/assets/css/shCore.css", Resources.assets_css_shCore_css },
                { "/assets/css/shCoreDefault.css", Resources.assets_css_shCoreDefault_css },
                { "/assets/css/docs.css", Resources.assets_css_docs_css },
                { "/assets/js/application.js", Resources.assets_js_application_js },
                { "/assets/js/jquery.js", Resources.assets_js_jquery_js },
                { "/assets/js/shBrushCSharp.js", Resources.assets_js_shBrushCSharp_js },
                { "/assets/js/shCore.js", Resources.assets_js_shCore_js },

                { "/bootstrap/bootstrap-theme.min.css", Resources.bootstrap_theme_min_css },
                { "/bootstrap/bootstrap.min.css", Resources.bootstrap_min_css },
                { "/bootstrap/bootstrap.min.js", Resources.bootstrap_min_js },

                { "/BindingTest.html", Resources.BindingTest },
                { "/PopupTest.html", Resources.PopupTest },
                { "/SchemeTest.html", Resources.SchemeTest },
                { "/TooltipTest.html", Resources.TooltipTest },
            };
        }

        public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback )
        {
            // The 'host' portion is entirely ignored by this scheme handler.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            string resource;
            if (resources.TryGetValue(fileName, out resource) &&
                !String.IsNullOrEmpty(resource))
            {
                var bytes = Encoding.UTF8.GetBytes(resource);
                response.ResponseStream = new MemoryStream(bytes);
                response.MimeType = GetMimeType(fileName);
                requestCompletedCallback();

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
