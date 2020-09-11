// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.DevTools
{
    /// <summary>
    /// The exception that is thrown when there's a problem executing a DevTools protocol method.
    /// </summary>
    public class DevToolsClientException : Exception
    {
        /// <summary>
        /// Get the Error Response
        /// </summary>
        public DevToolsDomainErrorResponse Response
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevToolsClientException"/> class with its message
        /// string set to a default message.
        /// </summary>
        public DevToolsClientException() : base("Error occurred whilst executing DevTools protocol method")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevToolsClientException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public DevToolsClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevToolsClientException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="errorResponse">error response</param>
        public DevToolsClientException(string message, DevToolsDomainErrorResponse errorResponse) : base(message)
        {
            Response = errorResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevToolsClientException"/> class with a specified error message
        /// and an inner exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public DevToolsClientException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
