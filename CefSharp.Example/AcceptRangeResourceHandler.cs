// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
// 
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using CefSharp.Callback;

namespace CefSharp.Example
{
    /// <summary>
    /// WORK IN PROGRESS - at this point this is a rough working demo,
    /// it's not production tested.
    /// </summary>
    internal class AcceptRangeResourceHandler : IResourceHandler
    {
        /// <summary>
        /// We reuse a temp buffer where possible for copying the data from the stream
        /// into the output stream
        /// </summary>
        private byte[] tempBuffer;

        private readonly FileInfo fileInfo;
        private string rangeHeader;
        private bool isRangeRequest;
        private string mimeType;
        private Stream stream;

        public AcceptRangeResourceHandler(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            mimeType = Cef.GetMimeType(fileInfo.Extension);
        }

        bool IResourceHandler.Open(IRequest request, out bool handleRequest, ICallback callback)
        {
            handleRequest = true;

            rangeHeader = request.GetHeaderByName("Range");
            isRangeRequest = !string.IsNullOrEmpty(rangeHeader);

            if (isRangeRequest)
            {
                //Range header present, we'll now open the stream for read
                //We don't need the stream open for the initial request as
                //we just need it's length which we get from the FileInfo
                stream = fileInfo.OpenRead();
            }

            return true;
        }

        void IResourceHandler.GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = -1;
            redirectUrl = null;

            if (isRangeRequest)
            {
                var headers = new NameValueCollection();

                headers.Add("Date", DateTime.Now.ToString("R"));
                headers.Add("Last-Modified", fileInfo.LastWriteTime.ToString("R"));
                headers.Add("Content-Type", mimeType);

                string contentRange;
                int contentLength;

                if (TryGetRangeHeader(out contentRange, out contentLength))
                {

                    headers.Add("Content-Length", contentLength.ToString());
                    headers.Add("Content-Range", contentRange);

                    responseLength = contentLength;
                    response.MimeType = mimeType;
                    response.Headers = headers;
                    response.StatusCode = (int)HttpStatusCode.PartialContent;
                    response.StatusText = "Partial Content";
                }
                else
                {
                    responseLength = -1;
                    response.MimeType = mimeType;
                    response.Headers = headers;
                    response.StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable;
                    response.StatusText = "Requested Range Not Satisfiable";
                }
            }
            else
            {
                //First request we only set headers
                //Response headers
                var headers = new NameValueCollection
                {
                    { "Accept-Ranges",  "bytes" },
                    { "Date",           DateTime.Now.ToString("R") },
                    { "Last-Modified",  fileInfo.LastWriteTime.ToString("R") },
                    { "Content-Length", fileInfo.Length.ToString() },
                    { "Content-Type",   mimeType }
                };

                responseLength = fileInfo.Length;

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusText = "OK";
                response.Headers = headers;
                response.MimeType = mimeType;
            }
        }

        private bool TryGetRangeHeader(out string contentRange, out int contentLength)
        {
            contentRange = "";
            contentLength = 0;

            //TODO Error handling
            var range = rangeHeader.Substring(6).Split('-');
            var rangeStart = string.IsNullOrEmpty(range[0]) ? 0 : int.Parse(range[0]);
            var rangeEnd = string.IsNullOrEmpty(range[1]) ? 0 : int.Parse(range[1]);

            var totalBytes = (int)stream.Length;

            if (totalBytes == 0)
            {
                return false;
            }

            if (rangeEnd == 0)
            {
                rangeEnd = totalBytes - 1;
            }

            if (rangeStart > rangeEnd)
            {
                return false;
            }

            if (rangeStart != stream.Position)
            {
                stream.Seek(rangeStart, SeekOrigin.Begin);
            }

            contentRange = "bytes " + rangeStart + "-" + rangeEnd + "/" + totalBytes;
            contentLength = totalBytes - rangeStart;

            return true;
        }

        bool IResourceHandler.Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
        {
            //No Stream or Stream cannot seek then we indicate failure
            if (stream == null || !stream.CanSeek)
            {
                //Indicate failure
                bytesSkipped = -2;

                return false;
            }

            if (stream.Position == (stream.Length - 1))
            {
                bytesSkipped = 0;

                return true;
            }

            var oldPosition = stream.Position;

            var position = stream.Seek(bytesToSkip, SeekOrigin.Current);

            bytesSkipped = position - oldPosition;

            return true;
        }

        bool IResourceHandler.Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
        {
            bytesRead = 0;

            //We don't need the callback, as it's an unmanaged resource we should dispose it (could wrap it in a using statement).
            callback.Dispose();

            if (stream == null)
            {
                return false;
            }

            //Data out represents an underlying unmanaged buffer (typically 64kb in size).
            //We reuse a temp buffer where possible
            if (tempBuffer == null || tempBuffer.Length < dataOut.Length)
            {
                tempBuffer = new byte[dataOut.Length];
            }

            bytesRead = stream.Read(tempBuffer, 0, tempBuffer.Length);

            // To indicate response completion set bytesRead to 0 and return false
            if (bytesRead == 0)
            {
                return false;
            }

            //We need to use bytesRead instead of tempbuffer.Length otherwise
            //garbage from the previous copy would be written to dataOut
            dataOut.Write(tempBuffer, 0, bytesRead);

            return bytesRead > 0;
        }

        void IResourceHandler.Cancel()
        {

        }

        void IDisposable.Dispose()
        {
            stream = null;
            tempBuffer = null;
        }

        //NOT USED
        bool IResourceHandler.ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            throw new NotImplementedException();
        }

        //NOT USED
        bool IResourceHandler.ProcessRequest(IRequest request, ICallback callback)
        {
            throw new NotImplementedException();
        }
    }
}
