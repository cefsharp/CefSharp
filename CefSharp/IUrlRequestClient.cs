// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
    /// <summary>
    /// Interface that should be implemented by the CefURLRequest client.
    /// The methods of this class will be called on the same thread that created the request unless otherwise documented. 
    /// </summary>
    public interface IUrlRequestClient
    {
        /// <summary>
        /// Called on the IO thread when the browser needs credentials from the user.
        /// This method will only be called for requests initiated from the browser process. 
        /// </summary>
        /// <param name="isProxy">indicates whether the host is a proxy server.</param>
        /// <param name="host">the hostname.</param>
        /// <param name="port">the port number.</param>
        /// <param name="realm"></param>
        /// <param name="scheme"></param>
        /// <param name="callback">is a callback for authentication information</param>
        /// <returns>
        /// Return true to continue the request and call IAuthCallback.Continue() when the authentication information is available.
        /// If the request has an associated browser/frame then returning false will result in a call to GetAuthCredentials
        /// on the IRequestHandler associated with that browser, if any.
        /// Otherwise, returning false will cancel the request immediately.
        /// </returns>

        bool GetAuthCredentials(bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback);
        /// <summary>
        /// Called when some part of the response is read. This method will not be called if the UR_FLAG_NO_DOWNLOAD_DATA flag is set on the request. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data">Contains the current bytes received since the last call.</param>
        void OnDownloadData(IUrlRequest request, Stream data);

        /// <summary>
        /// Notifies the client of download progress.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="current">denotes the number of bytes received up to the call </param>
        /// <param name="total">is the expected total size of the response (or -1 if not determined).</param>
        void OnDownloadProgress(IUrlRequest request, long current, long total);

        /// <summary>
        /// Notifies the client that the request has completed. Use the
        /// IURLRequest.GetRequestStatus method to determine if the request was
        /// successful or not.
        /// </summary>
        /// <param name="request"></param>
        void OnRequestComplete(IUrlRequest request);

        /// <summary>
        /// Notifies the client of upload progress.
        /// This method will only be called if the UR_FLAG_REPORT_UPLOAD_PROGRESS flag is set on the request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="current">denotes the number of bytes sent so far.</param>
        /// <param name="total">is the total size of uploading data (or -1 if chunked upload is enabled).</param>
        void OnUploadProgress(IUrlRequest request, long current, long total);
    }
}
