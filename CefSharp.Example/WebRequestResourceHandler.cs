// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CefSharp.Example
{
    /// <summary>
    /// A simple <see cref="ResourceHandler"/> that uses <see cref="WebRequest.Create(string)"/> to fulfill requests.
    /// </summary>
    /// <remarks>
    /// This example doesn't cover all cases, for example POST requests, if you'd like to see the example
    /// expanded then please subit a Pull Request.
    /// </remarks>
    public class WebRequestResourceHandler : ResourceHandler
    {
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            //Spawn a Task and immediately return CefReturnValue.ContinueAsync
            Task.Run(async () =>
            {
                using (callback)
                {
                    //Create a clone of the headers so we can modify it
                    var headers = new NameValueCollection(request.Headers);

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(request.Url);
                    httpWebRequest.UserAgent = headers["User-Agent"];
                    httpWebRequest.Accept = headers["Accept"];
                    httpWebRequest.Method = request.Method;
                    httpWebRequest.Referer = request.ReferrerUrl;

                    //These headers must be set via the appropriate properties.
                    headers.Remove("User-Agent");
                    headers.Remove("Accept");

                    httpWebRequest.Headers.Add(headers);

                    //TODO: Deal with post data
                    var postData = request.PostData;

                    var httpWebResponse = await httpWebRequest.GetResponseAsync() as HttpWebResponse;

                    // Get the stream associated with the response.
                    var receiveStream = httpWebResponse.GetResponseStream();

                    var contentType = new ContentType(httpWebResponse.ContentType);
                    var mimeType = contentType.MediaType;
                    var charSet = contentType.CharSet;
                    var statusCode = httpWebResponse.StatusCode;

                    var memoryStream = new MemoryStream();
                    receiveStream.CopyTo(memoryStream);
                    receiveStream.Dispose();
                    httpWebResponse.Dispose();

                    //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
                    memoryStream.Position = 0;

                    ResponseLength = memoryStream.Length;
                    MimeType = mimeType;
                    Charset = charSet ?? "UTF-8";
                    StatusCode = (int)statusCode;
                    Stream = memoryStream;
                    AutoDisposeStream = true;

                    callback.Continue();
                }
            });

            return CefReturnValue.ContinueAsync;
        }
    }
}
