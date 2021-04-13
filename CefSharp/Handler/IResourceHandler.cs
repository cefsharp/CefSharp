// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using CefSharp.Callback;

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
    /// add the override keyword to existing methods e.g. ProcessRequestAsync.
    /// </summary>
    public interface IResourceHandler : IDisposable
    {
        /// <summary>
        /// Open the response stream.
        /// - To handle the request immediately set <paramref name="handleRequest"/> to true and return true.
        /// - To decide at a later time set <paramref name="handleRequest"/> to false, return true, and execute <paramref name="callback"/>
        /// to continue or cancel the request.
        /// - To cancel the request immediately set <paramref name="handleRequest"/> to true and return false.
        /// This method will be called in sequence but not from a dedicated thread.
        /// For backwards compatibility set <paramref name="handleRequest"/> to false and return false and the <see cref="ProcessRequest(IRequest, ICallback)"/> method
        /// will be called.
        /// </summary>
        /// <param name="request">request </param>
        /// <param name="handleRequest">see main summary</param>
        /// <param name="callback">callback </param>
        /// <returns>see main summary</returns>
        bool Open(IRequest request, out bool handleRequest, ICallback callback);

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
        [Obsolete("This method is deprecated and will be removed in the next version. Use Open instead.")]
        bool ProcessRequest(IRequest request, ICallback callback);

        /// <summary>
        /// Retrieve response header information. If the response length is not known
        /// set <paramref name="responseLength"/> to -1 and ReadResponse() will be called until it
        /// returns false. If the response length is known set <paramref name="responseLength"/>
        /// to a positive value and ReadResponse() will be called until it returns
        /// false or the specified number of bytes have been read.
        /// 
        /// It is also possible to set <paramref name="response"/> to a redirect http status code
        /// and pass the new URL via a Location header. Likewise with <paramref name="redirectUrl"/> it
        /// is valid to set a relative or fully qualified URL as the Location header
        /// value. If an error occured while setting up the request you can call
        /// <see cref="IResponse.ErrorCode"/> on <paramref name="response"/> to indicate the error condition.
        /// </summary>
        /// <param name="response">Use the response object to set the mime type, http status code and other optional header values.</param>
        /// <param name="responseLength">If the response length is not known set responseLength to -1</param>
        /// <param name="redirectUrl">To redirect the request to a new URL set this to the new URL. Can be either a relative or fully qualified URL.</param>
        void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl);

        /// <summary>
        /// Skip response data when requested by a Range header.
        /// Skip over and discard bytesToSkip bytes of response data.
        /// - If data is available immediately set bytesSkipped to the number of of bytes skipped and return true.
        /// - To read the data at a later time set bytesSkipped to 0, return true and execute callback when the data is available.
        /// - To indicate failure set bytesSkipped to &lt; 0 (e.g. -2 for ERR_FAILED) and return false.
        /// This method will be called in sequence but not from a dedicated thread.
        /// </summary>
        /// <param name="bytesToSkip">number of bytes to be skipped</param>
        /// <param name="bytesSkipped">
        /// If data is available immediately set bytesSkipped to the number of of bytes skipped and return true.
        /// To read the data at a later time set bytesSkipped to 0, return true and execute callback when the data is available.
        /// </param>
        /// <param name="callback">To read the data at a later time set bytesSkipped to 0,
        /// return true and execute callback when the data is available.</param>
        /// <returns>See summary</returns>
        bool Skip(Int64 bytesToSkip, out Int64 bytesSkipped, IResourceSkipCallback callback);

        /// <summary>
        /// Read response data. If data is available immediately copy up to
        /// dataOut.Length bytes into dataOut, set bytesRead to the number of
        /// bytes copied, and return true. To read the data at a later time keep a
        /// pointer to dataOut, set bytesRead to 0, return true and execute
        /// callback when the data is available (dataOut will remain valid until
        /// the callback is executed). To indicate response completion set bytesRead
        /// to 0 and return false. To indicate failure set bytesRead to &lt; 0 (e.g. -2
        /// for ERR_FAILED) and return false. This method will be called in sequence
        /// but not from a dedicated thread.
        /// 
        /// For backwards compatibility set bytesRead to -1 and return false and the ReadResponse method will be called.
        /// </summary>
        /// <param name="dataOut">If data is available immediately copy up to <see cref="Stream.Length"/> bytes into dataOut.</param>
        /// <param name="bytesRead">To indicate response completion set bytesRead to 0 and return false.</param>
        /// <param name="callback">set <paramref name="bytesRead"/> to 0, return true and execute callback when the data is available
        /// (dataOut will remain valid until the callback is executed). If you have no need
        /// of the callback then Dispose of it immediately.</param>
        /// <returns>return true or false depending on the criteria, see summary.</returns>
        bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback);

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
        [Obsolete("This method is deprecated and will be removed in the next version. Use Skip and Read instead.")]
        bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback);

        /// <summary>
        /// Request processing has been canceled.
        /// </summary>
        void Cancel();
    }
}
