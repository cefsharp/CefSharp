// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Specialized;
using System.IO;
using CefSharp.Callback;

namespace CefSharp.Lagacy
{
    /// <summary>
    /// Legacy ResourceHandler, will be removed when CEF removes the old code path for
    /// it's CefResourceHandler implementation. This is the older and well tested variant.
    /// It doesn't however support range request headers (seek).
    /// </summary>
    public class ResourceHandler : IResourceHandler
    {
        /// <summary>
        /// Gets or sets the Charset
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the resource stream.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the http status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets ResponseLength, when you know the size of your
        /// Stream (Response) set this property. This is optional.
        /// If you use a MemoryStream and don't provide a value
        /// here then it will be cast and it's size used
        /// </summary>
        public long? ResponseLength { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers { get; private set; }

        /// <summary>
        /// When true the Stream will be Disposed when
        /// this instance is Disposed. The default value for
        /// this property is false.
        /// </summary>
        public bool AutoDisposeStream { get; set; }

        /// <summary>
        /// If the ErrorCode is set then the response will be ignored and
        /// the errorCode returned.
        /// </summary>
        public CefErrorCode? ErrorCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandler"/> class.
        /// </summary>
        /// <param name="mimeType">Optional mimeType defaults to <see cref="CefSharp.ResourceHandler.DefaultMimeType"/></param>
        /// <param name="stream">Optional Stream - must be set at some point to provide a valid response</param>
        /// <param name="autoDisposeStream">When true the Stream will be disposed when this instance is Diposed, you will
        /// be unable to use this ResourceHandler after the Stream has been disposed</param>
        /// <param name="charset">response charset</param>
        public ResourceHandler(string mimeType = CefSharp.ResourceHandler.DefaultMimeType, Stream stream = null, bool autoDisposeStream = false, string charset = null)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException("mimeType", "Please provide a valid mimeType");
            }

            StatusCode = 200;
            StatusText = "OK";
            MimeType = mimeType;
            Headers = new NameValueCollection();
            Stream = stream;
            AutoDisposeStream = autoDisposeStream;
            Charset = charset;
        }

        /// <summary>
        /// Begin processing the request. If you have the data in memory you can execute the callback
        /// immediately and return true. For Async processing you would typically spawn a Task to perform processing,
        /// then return true. When the processing is complete execute callback.Continue(); In your processing Task, simply set
        /// the StatusCode, StatusText, MimeType, ResponseLength and Stream
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>To handle the request return true and call
        /// <see cref="ICallback.Continue"/> once the response header information is available
        /// <see cref="ICallback.Continue"/> can also be called from inside this method if
        /// header information is available immediately).
        /// To cancel the request return false.</returns>
        protected virtual bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            callback.Continue();

            return true;
        }

        /// <summary>
        /// Called if the request is cancelled
        /// </summary>
        protected virtual void Cancel()
        {

        }

        /// <summary>
        /// Dispose of resources here
        /// </summary>
        protected virtual void Dispose()
        {
            if (AutoDisposeStream && Stream != null)
            {
                Stream.Dispose();
                Stream = null;
            }
        }

        /// <summary>
        /// Populate the response stream, response length. When this method is called
        /// the response should be fully populated with data.
        /// It is possible to redirect to another url at this point in time.
        /// NOTE: It's no longer manditory to implement this method, you can simply populate the
        /// properties of this instance and they will be set by the default implementation. 
        /// </summary>
        /// <param name="response">The response object used to set Headers, StatusCode, etc</param>
        /// <param name="responseLength">length of the response</param>
        /// <param name="redirectUrl">If set the request will be redirect to specified Url</param>
        /// <returns>The response stream</returns>
        protected virtual Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            redirectUrl = null;
            responseLength = -1;

            response.MimeType = MimeType;
            response.StatusCode = StatusCode;
            response.StatusText = StatusText;
            response.Headers = Headers;

            if (!string.IsNullOrEmpty(Charset))
            {
                response.Charset = Charset;
            }

            if (ResponseLength.HasValue)
            {
                responseLength = ResponseLength.Value;
            }
            else
            {
                //If no ResponseLength provided then attempt to infer the length
                if (Stream != null && Stream.CanSeek)
                {
                    responseLength = Stream.Length;
                }
            }

            return Stream;
        }

        void IResourceHandler.Cancel()
        {
            Cancel();

            Stream = null;
        }


        void IDisposable.Dispose()
        {
            Dispose();
        }

        void IResourceHandler.GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            if (ErrorCode.HasValue)
            {
                responseLength = 0;
                redirectUrl = null;
                response.ErrorCode = ErrorCode.Value;
            }
            else
            {
                Stream = GetResponse(response, out responseLength, out redirectUrl);

                if (Stream != null && Stream.CanSeek)
                {
                    //Reset the stream position to 0
                    Stream.Position = 0;
                }
            }
        }

        bool IResourceHandler.Open(IRequest request, out bool handleRequest, ICallback callback)
        {
            callback.Dispose();

            //Legacy behaviour
            handleRequest = false;

            return false;
        }

        bool IResourceHandler.ProcessRequest(IRequest request, ICallback callback)
        {
            return ProcessRequestAsync(request, callback);
        }

        bool IResourceHandler.Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
        {
            //Legacy behaviour
            bytesRead = -1;

            return false;
        }

        bool IResourceHandler.ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            //We don't need the callback, as it's an unmanaged resource we should dispose it (could wrap it in a using statement).
            callback.Dispose();

            if (Stream == null)
            {
                bytesRead = 0;

                return false;
            }

            //Data out represents an underlying buffer (typically 32kb in size).
            var buffer = new byte[dataOut.Length];
            bytesRead = Stream.Read(buffer, 0, buffer.Length);

            //If bytesRead is 0 then no point attempting a write to dataOut
            if (bytesRead == 0)
            {
                return false;
            }

            dataOut.Write(buffer, 0, buffer.Length);

            return bytesRead > 0;
        }

        bool IResourceHandler.Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}

