// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    internal class CefSharpSchemeHandler : IResourceHandler
    {
        private static readonly IDictionary<string, string> ResourceDictionary;

        private string mimeType;
        private MemoryStream stream;
        
        static CefSharpSchemeHandler()
        {
            ResourceDictionary = new Dictionary<string, string>
            {
                { "/home.html", Resources.home_html },

                { "/assets/css/shCore.css", Resources.assets_css_shCore_css },
                { "/assets/css/shCoreDefault.css", Resources.assets_css_shCoreDefault_css },
                { "/assets/css/docs.css", Resources.assets_css_docs_css },
                { "/assets/js/application.js", Resources.assets_js_application_js },
                { "/assets/js/jquery.js", Resources.assets_js_jquery_js },
                { "/assets/js/shBrushCSharp.js", Resources.assets_js_shBrushCSharp_js },
                { "/assets/js/shBrushJScript.js", Resources.assets_js_shBrushJScript_js },
                { "/assets/js/shCore.js", Resources.assets_js_shCore_js },

                { "/bootstrap/bootstrap-theme.min.css", Resources.bootstrap_theme_min_css },
                { "/bootstrap/bootstrap.min.css", Resources.bootstrap_min_css },
                { "/bootstrap/bootstrap.min.js", Resources.bootstrap_min_js },

                { "/BindingTest.html", Resources.BindingTest },
                { "/ExceptionTest.html", Resources.ExceptionTest },
                { "/PopupTest.html", Resources.PopupTest },
                { "/SchemeTest.html", Resources.SchemeTest },
                { "/TooltipTest.html", Resources.TooltipTest },
                { "/FramedWebGLTest.html", Resources.FramedWebGLTest },
                { "/MultiBindingTest.html", Resources.MultiBindingTest },
                { "/ScriptedMethodsTest.html", Resources.ScriptedMethodsTest },
            };
        }

        public bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            // The 'host' portion is entirely ignored by this scheme handler.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            if(string.Equals(fileName, "/PostDataTest.html", StringComparison.OrdinalIgnoreCase))
            {
                var postDataElement = request.PostData.Elements.FirstOrDefault();
                var resourceHandler = ResourceHandler.FromString("Post Data: " + (postDataElement == null ? "null" : postDataElement.GetBody()));
                stream = (MemoryStream)resourceHandler.Stream;
                mimeType = "text/html";
                callback.Continue();
                return true;
            }

            if (string.Equals(fileName, "/PostDataAjaxTest.html", StringComparison.OrdinalIgnoreCase))
            {
                var postData = request.PostData;
                if(postData == null)
                {
                    var resourceHandler = ResourceHandler.FromString("Post Data: null");
                    stream = (MemoryStream)resourceHandler.Stream;
                    mimeType = "text/html";
                    callback.Continue();
                }
                else
                { 
                    var postDataElement = postData.Elements.FirstOrDefault();
                    var resourceHandler = ResourceHandler.FromString("Post Data: " + (postDataElement == null ? "null" : postDataElement.GetBody()));
                    stream = (MemoryStream)resourceHandler.Stream;
                    mimeType = "text/html";
                    callback.Continue();
                }

                return true;
            }

            string resource;
            if (ResourceDictionary.TryGetValue(fileName, out resource) && !string.IsNullOrEmpty(resource))
            {
                Task.Run(() =>
                {
                    using (callback)
                    { 
                        var bytes = Encoding.UTF8.GetBytes(resource);
                        stream = new MemoryStream(bytes);

                        var fileExtension = Path.GetExtension(fileName);
                        mimeType = ResourceHandler.GetMimeType(fileExtension);

                        callback.Continue();
                    }
                });

                return true;
            }
            else
            {
                callback.Dispose();
            }

            return false;
        }

        public Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream.Length;
            redirectUrl = null;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusText = "OK";
            response.MimeType = mimeType;

            return stream;
        }
    }
}
