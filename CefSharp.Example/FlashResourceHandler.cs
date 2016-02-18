// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CefSharp.Example
{
    public class FlashResourceHandler : ResourceHandler
    {
        private MemoryStream stream;
        private string mime;

        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            Task.Run(() =>
            {
                using (callback)
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://samples.mplayerhq.hu/SWF/zeldaADPCM5bit.swf");

                    var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    // Get the stream associated with the response.
                    var receiveStream = httpWebResponse.GetResponseStream();
                    mime = httpWebResponse.ContentType;

                    stream = new MemoryStream();
                    receiveStream.CopyTo(stream);
                    httpWebResponse.Close();
               
                    callback.Continue();
                }
            });

            return true;
        }

        public override Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream.Length;
            redirectUrl = null;

            response.MimeType = mime;
            response.StatusCode = (int)HttpStatusCode.OK;

            //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
            stream.Position = 0;

            return stream;
        }
    }
}
