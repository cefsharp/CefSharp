// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// FileResourceHandler is used as a placeholder to represent the underlying CefStreamResourceHandler
    /// (Was previously ResourceHandlerType::File to differentiate, going for a more flexible approach now)
    /// </summary>
    public class FileResourceHandler : IResourceHandler
    {
        /// <summary>
        /// Path of the underlying file
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FileResourceHandler"/> class.
        /// </summary>
        /// <param name="mimeType">mimeType</param>
        /// <param name="filePath">filePath</param>
        public FileResourceHandler(string mimeType, string filePath)
        {
            if(string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException("mimeType", "Please provide a valid mimeType");
            }

            if(string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath", "Please provide a valid filePath");
            }

            
            MimeType = mimeType;
            FilePath = filePath;
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

        bool IResourceHandler.CanGetCookie(Cookie cookie)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        bool IResourceHandler.CanSetCookie(Cookie cookie)
        {
            //Should never be called
            throw new NotImplementedException("This method should never be called");
        }

        void IResourceHandler.Cancel()
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

