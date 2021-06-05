// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CefSharp.SchemeHandler
{
    //Shorthand for Owin pipeline func
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// <see cref="ResourceHandler"/> implementation that uses an OWIN capable host of fulfilling requests.
    /// Can be used with NancyFx or AspNet Core
    /// </summary>
    /// TODO:
    ///   - Multipart post data
    ///   - Cancellation Token
    public class OwinResourceHandler : ResourceHandler
    {
        private static readonly Dictionary<int, string> StatusCodeToStatusTextMapping = new Dictionary<int, string>
        {
            {200, "OK"},
            {301, "Moved Permanently"},
            {304, "Not Modified"},
            {404, "Not Found"}
        };

        private readonly AppFunc appFunc;

        public OwinResourceHandler(AppFunc appFunc)
        {
            this.appFunc = appFunc;
        }

        /// <summary>
        /// Read the request, then process it through the OWEN pipeline
        /// then populate the response properties.
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="callback">callback</param>
        /// <returns>always returns true as we'll handle all requests this handler is registered for.</returns>
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            // PART 1 - Read the request - here we read the request and create a dictionary
            // that follows the OWEN standard

            var responseStream = new MemoryStream();
            var requestBody = Stream.Null;

            if (request.Method == "POST")
            {
                using (var postData = request.PostData)
                {
                    if (postData != null)
                    {
                        var postDataElements = postData.Elements;


                        var firstPostDataElement = postDataElements.First();

                        var bytes = firstPostDataElement.Bytes;

                        requestBody = new MemoryStream(bytes, 0, bytes.Length);

                        //TODO: Investigate how to process multi part POST data
                        //var charSet = request.GetCharSet();
                        //foreach (var element in elements)
                        //{
                        //    if (element.Type == PostDataElementType.Bytes)
                        //    {
                        //        var body = element.GetBody(charSet);
                        //    }
                        //}
                    }
                }
            }

            //var cancellationTokenSource = new CancellationTokenSource();
            //var cancellationToken = cancellationTokenSource.Token;
            var uri = new Uri(request.Url);
            var requestHeaders = ToDictionary(request.Headers);
            //Add Host header as per http://owin.org/html/owin.html#5-2-hostname
            requestHeaders.Add("Host", new[] { uri.Host + (uri.Port > 0 ? (":" + uri.Port) : "") });

            //http://owin.org/html/owin.html#3-2-environment
            //The Environment dictionary stores information about the request,
            //the response, and any relevant server state.
            //The server is responsible for providing body streams and header collections for both the request and response in the initial call.
            //The application then populates the appropriate fields with response data, writes the response body, and returns when done.
            //Keys MUST be compared using StringComparer.Ordinal.
            var owinEnvironment = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                //Request http://owin.org/html/owin.html#3-2-1-request-data
                {"owin.RequestBody", requestBody},
                {"owin.RequestHeaders", requestHeaders},
                {"owin.RequestMethod", request.Method},
                {"owin.RequestPath", uri.AbsolutePath},
                {"owin.RequestPathBase", "/"},
                {"owin.RequestProtocol", "HTTP/1.1"},
                //To conform to the OWIN spec we need to remove the leading '?'
                {"owin.RequestQueryString", string.IsNullOrEmpty(uri.Query) ? string.Empty : uri.Query.Substring(1)},
                {"owin.RequestScheme", uri.Scheme},
                //Response http://owin.org/html/owin.html#3-2-2-response-data
                {"owin.ResponseHeaders", new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)},
                {"owin.ResponseBody", responseStream},
                //Other Data
                {"owin.Version", "1.0.0"},
                //{"owin.CallCancelled", cancellationToken}
            };

            //PART 2 - Spawn a new task to execute the OWIN pipeline
            //We execute this in an async fashion and return true so other processing
            //can occur
            Task.Run(async () =>
            {
                //Call into the OWEN pipeline
                try
                {
                    await appFunc(owinEnvironment);

                    //Response has been populated - reset the position to 0 so it can be read
                    responseStream.Position = 0;

                    int statusCode;

                    if (owinEnvironment.ContainsKey("owin.ResponseStatusCode"))
                    {
                        statusCode = Convert.ToInt32(owinEnvironment["owin.ResponseStatusCode"]);
                        //TODO: Improve status code mapping - see if CEF has a helper function that can be exposed
                        //StatusText = StatusCodeToStatusTextMapping[response.StatusCode];
                    }
                    else
                    {
                        statusCode = (int)HttpStatusCode.OK;
                        //StatusText = "OK";
                    }

                    //Grab a reference to the ResponseHeaders
                    var responseHeaders = (Dictionary<string, string[]>)owinEnvironment["owin.ResponseHeaders"];

                    //Populate the response properties
                    Stream = responseStream;
                    ResponseLength = responseStream.Length;
                    StatusCode = statusCode;

                    if(responseHeaders.ContainsKey("Content-Type"))
                    {
                        var contentType = responseHeaders["Content-Type"].First();
                        MimeType = contentType.Split(';').First();
                    }
                    else
                    {
                        MimeType = DefaultMimeType;
                    }                    

                    //Add the response headers from OWIN to the Headers NameValueCollection
                    foreach (var responseHeader in responseHeaders)
                    {
                        //It's possible for headers to have multiple values
                        foreach (var val in responseHeader.Value)
                        {
                            Headers.Add(responseHeader.Key, val);
                        }
                    }
                }
                catch(Exception ex)
                {
                    int statusCode = (int)HttpStatusCode.InternalServerError;

                    var responseData = GetByteArray("Error: " + ex.ToString(), System.Text.Encoding.UTF8, true);

                    //Populate the response properties
                    Stream = new MemoryStream(responseData);
                    ResponseLength = responseData.Length;
                    StatusCode = statusCode;
                    MimeType = "text/html";
                }

                //Once we've finished populating the properties we execute the callback
                //Callback wraps an unmanaged resource, so let's explicitly Dispose when we're done    
                using (callback)
                {
                    callback.Continue();
                }
            });

            return CefReturnValue.ContinueAsync;
        }

        private static IDictionary<string, string[]> ToDictionary(NameValueCollection nameValueCollection)
        {
            var dict = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            foreach (var key in nameValueCollection.AllKeys)
            {
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, new string[0]);
                }
                var strings = nameValueCollection.GetValues(key);
                if (strings == null)
                {
                    continue;
                }
                foreach (string value in strings)
                {
                    var values = dict[key].ToList();
                    values.Add(value);
                    dict[key] = values.ToArray();
                }
            }
            return dict;
        }
    }
}
