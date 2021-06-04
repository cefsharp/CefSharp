// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Response returned from <see cref="IWebBrowser.LoadUrlAsync(string, System.Threading.SynchronizationContext)"/>
    /// </summary>
    public class LoadUrlAsyncResponse
    {
        /// <summary>
        /// Error Code. If the network request was made successfully this value will be <see cref="CefErrorCode.None"/>
        /// (no error occured)
        /// </summary>
        public CefErrorCode ErrorCode { get; private set; }
        
        /// <summary>
        /// Http Status Code. If <see cref="ErrorCode"/> is not equal to <see cref="CefErrorCode.None"/>
        /// then this value will be -1.
        /// </summary>
        public int HttpStatusCode { get; private set; }

        /// <summary>
        /// If <see cref="ErrorCode"/> is equal to <see cref="CefErrorCode.None"/> and
        /// <see cref="HttpStatusCode"/> is equal to 200 (OK) then the main frame loaded without
        /// critical error. 
        /// </summary>
        public bool Success
        {
            get { return ErrorCode == CefErrorCode.None && HttpStatusCode == 200; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="errorCode">CEF Error Code</param>
        /// <param name="httpStatusCode">Http Status Code</param>
        public LoadUrlAsyncResponse(CefErrorCode errorCode, int httpStatusCode)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }
    }
}
