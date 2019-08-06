// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using CefSharp.Callback;

namespace CefSharp
{
    /// <summary>
    /// ByteArrayResourceHandler is used as a placeholder class which uses native CEF implementations.
    /// CefStreamReader::CreateForData(); reads the byte array that is passed to a new instance
    /// of CefStreamResourceHandler 
    /// TODO: Move this class into Handler namespace 
    /// </summary>
    public class ByteArrayResourceHandler : IResourceHandler
    {
        /// <summary>
        /// Underlying byte array that represents the data
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayResourceHandler"/> class.
        /// </summary>
        /// <param name="mimeType">mimeType</param>
        /// <param name="data">byte array</param>
        public ByteArrayResourceHandler(string mimeType, byte[] data)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException("mimeType", "Please provide a valid mimeType");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data", "Please provide a valid array");
            }

            MimeType = mimeType;
            Data = data;
        }

        bool IResourceHandler.ProcessRequest(IRequest request, ICallback callback)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        void IResourceHandler.GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        bool IResourceHandler.ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        void IResourceHandler.Cancel()
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        bool IResourceHandler.Open(IRequest request, out bool handleRequest, ICallback callback)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        bool IResourceHandler.Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        bool IResourceHandler.Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        void IDisposable.Dispose()
        {
            //NOOP
        }
    }
}


