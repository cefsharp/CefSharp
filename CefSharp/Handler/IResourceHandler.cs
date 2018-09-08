// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp
{
    /// <summary>
    /// Class used to implement a custom resource handler. The methods of this class will always be called on the CEF IO thread.
    /// Blocking the CEF IO thread will adversely affect browser performance. We suggest you execute your code in a Task (or similar).
    /// To implement async handling, spawn a new Task (or similar), keep a reference to the callback. When you have a 
    /// fully populated stream, execute the callback. Once the callback Executes, GetResponseHeaders will be called where you
    /// can modify the response including headers, or even redirect to a new Url. Set your responseLength and headers
    /// Populate the dataOut stream in ReadResponse. For those looking for a sample implementation or upgrading from 
    /// a previous version <see cref="ResourceHandler"/>. For those upgrading, inherit from ResourceHandler instead of IResourceHandler
    /// add the override keywoard to existing methods e.g. ProcessRequestAsync.
    /// </summary>
    public interface IResourceHandler : IDisposable
    {
        /// <summary>
        /// Begin processing the request.  
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>To handle the request return true and call
        /// <see cref="ICallback.Continue"/> once the response header information is available
        /// <see cref="ICallback.Continue"/> can also be called from inside this method if
        /// header information is available immediately).
        /// To cancel the request return false.</returns>
        bool ProcessRequest(IRequest request, ICallback callback);

        /// <summary>
        /// Retrieve response header information. If the response length is not known
        /// set responseLength to -1 and ReadResponse() will be called until it
        /// returns false. If the response length is known set responseLength
        /// to a positive value and ReadResponse() will be called until it returns
        /// false or the specified number of bytes have been read. 
        /// If an error occured while setting up the request you can set <see cref="IResponse.ErrorCode"/>
        /// to indicate the error condition.
        /// </summary>
        /// <param name="response">Use the response object to set the mime type, http status code and other optional header values.</param>
        /// <param name="responseLength">If the response length is not known set responseLength to -1</param>
        /// <param name="redirectUrl">To redirect the request to a new URL set redirectUrl to the new Url.</param>
        void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl);

        /// <summary>
        /// Read response data. If data is available immediately copy to
        /// dataOut, set bytesRead to the number of bytes copied, and return true.
        /// To read the data at a later time set bytesRead to 0, return true and call ICallback.Continue() when the
        /// data is available. To indicate response completion return false.
        /// </summary>
        /// <param name="dataOut">Stream to write to</param>
        /// <param name="bytesRead">Number of bytes copied to the stream</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>If data is available immediately copy to dataOut, set bytesRead to the number of bytes copied,
        /// and return true.To indicate response completion return false.</returns>
        /// <remarks>Depending on this size of your response this method may be called multiple times</remarks>
        bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback);

        /// <summary>
        /// Return true if the specified cookie can be sent with the request or false
        /// otherwise. If false is returned for any cookie then no cookies will be sent
        /// with the request.
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <returns>Return true if the specified cookie can be sent with the request or false
        /// otherwise. If false is returned for any cookie then no cookies will be sent
        /// with the request.</returns>
        bool CanGetCookie(Cookie cookie);

        /// <summary>
        /// Return true if the specified cookie returned with the response can be set or false otherwise.
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <returns>Return true if the specified cookie returned with the response can be set or false otherwise.</returns>
        bool CanSetCookie(Cookie cookie);

        /// <summary>
        /// Request processing has been canceled.
        /// </summary>
        void Cancel();
    }
}
