// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
    /// <summary>
    /// Class used to implement a custom resource handler. The methods of this class will always be called on the CEF IO thread.
    /// Blocking the CEF IO thread will adversely affect browser performance. We suggest you execute your code in a Task (or similar).
    /// To implement async handling, spawn a new Task (or similar), keep a reference to the callback. When you have a 
    /// fully populated stream, execute the callback. Once the callback Executes, GetResponse will be called where you
    /// can modify the response including headers, or even redirect to a new Url. Typically you would set the responseLength
    /// and return your fully populated Stream
    /// </summary>
    public interface IResourceHandler
    {
        /// <summary>
        /// Processes request asynchronously.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>true if the request is handled, false otherwise.</returns>
        bool ProcessRequestAsync(IRequest request, ICallback callback);

        /// <summary>
        /// Populate the response stream, response length. When this method is called
        /// the response should be fully populated with data
        /// It is possible to redirect to another url at this point in time
        /// </summary>
        /// <param name="response">The response object used to set Headers, StatusCode, etc</param>
        /// <param name="responseLength">length of the response</param>
        /// <param name="redirectUrl">If set the request will be redirect to specified Url</param>
        /// <returns>The response stream</returns>
        Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl);
    }
}
