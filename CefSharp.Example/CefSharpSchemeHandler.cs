using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    internal class CefSharpSchemeHandler : ISchemeHandler
    {
        private static readonly IDictionary<string, string> ResourceDictionary;

        private string mimeType;
        private MemoryStream stream;
        
        static CefSharpSchemeHandler()
        {
            ResourceDictionary = new Dictionary<string, string>
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

        public bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            // The 'host' portion is entirely ignored by this scheme handler.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            string resource;
            if (ResourceDictionary.TryGetValue(fileName, out resource) && !string.IsNullOrEmpty(resource))
            {
                Task.Run(() =>
                {
                    var bytes = Encoding.UTF8.GetBytes(resource);
                    stream = new MemoryStream(bytes);

                    var fileExtension = Path.GetExtension(fileName);
                    mimeType = ResourceHandler.GetMimeType(fileExtension);

                    callback.Continue();
                });

                return true;
            }

            return false;
        }

        public Stream GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream.Length;
            redirectUrl = null;

            response.StatusCode = 300;
            response.StatusText = "OK";
            response.MimeType = mimeType;

            return stream;
        }
    }
}
