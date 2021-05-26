// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp.Fluent
{
    /// <summary>
    /// Called on the CEF IO thread when the browser needs credentials from the user.
    /// This method will only be called for requests initiated from the browser process. 
    /// </summary>
    /// <param name="isProxy">indicates whether the host is a proxy server.</param>
    /// <param name="host">the hostname.</param>
    /// <param name="port">the port number.</param>
    /// <param name="realm">realm</param>
    /// <param name="scheme">scheme</param>
    /// <param name="callback">is a callback for authentication information</param>
    /// <returns>
    /// Return true to continue the request and call <see cref="IAuthCallback.Continue(string, string)"/> when the authentication information is available.
    /// If the request has an associated browser/frame then returning false will result in a call to <see cref="IRequestHandler.GetAuthCredentials"/> 
    /// on the <see cref="IRequestHandler"/> associated with that browser, if any.
    /// Otherwise, returning false will cancel the request immediately.
    /// </returns>
    public delegate bool GetAuthCredentialsDelegate(bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback);

    /// <summary>
    /// Called when some part of the response is read. This method will not be called if the <see cref="UrlRequestFlags.NoDownloadData"/> flag is set on the request. 
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="data">A stream containing the bytes received since the last call. Cannot be used outside the scope of this method. </param>
    public delegate void OnDownloadDataDelegate(IUrlRequest request, Stream data);

    /// <summary>
    /// Notifies the client of download progress.
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="current">denotes the number of bytes received up to the call </param>
    /// <param name="total">is the expected total size of the response (or -1 if not determined).</param>
    public delegate void OnDownloadProgressDelegate(IUrlRequest request, long current, long total);

    /// <summary>
    /// Notifies the client that the request has completed.
    /// Use the <see cref="IUrlRequest.RequestStatus"/> property to determine if the
    /// request was successful or not.
    /// </summary>
    /// <param name="request">request</param>
    public delegate void OnRequestCompleteDelegate(IUrlRequest request);

    /// <summary>
    /// Notifies the client of upload progress.
    /// This method will only be called if the UR_FLAG_REPORT_UPLOAD_PROGRESS flag is set on the request.
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="current">denotes the number of bytes sent so far.</param>
    /// <param name="total">is the total size of uploading data (or -1 if chunked upload is enabled).</param>
    public delegate void OnUploadProgressDelegate(IUrlRequest request, long current, long total);

    /// <summary>
    /// Fluent UrlRequestClient
    /// </summary>
    public class UrlRequestClient : CefSharp.UrlRequestClient
    {
        private GetAuthCredentialsDelegate getAuthCredentials;
        private OnDownloadDataDelegate onDownloadData;
        private OnDownloadProgressDelegate onDownloadProgress;
        private OnRequestCompleteDelegate onRequestComplete;
        private OnUploadProgressDelegate onUploadProgress;

        /// <summary>
        /// Create a new UrlRequestClient Builder
        /// </summary>
        /// <returns>Fluent UrlRequestClient Builder</returns>
        public static UrlRequestClientBuilder Create()
        {
            return new UrlRequestClientBuilder();
        }

        /// <summary>
        /// Use <see cref="Create"/> to create a new instance of the fluent builder
        /// </summary>
        internal UrlRequestClient()
        {

        }

        internal void SetGetAuthCredentials(GetAuthCredentialsDelegate func)
        {
            getAuthCredentials = func;
        }

        internal void SetOnDownloadData(OnDownloadDataDelegate action)
        {
            onDownloadData = action;
        }

        internal void SetOnDownloadProgress(OnDownloadProgressDelegate action)
        {
            onDownloadProgress = action;
        }

        internal void SetOnRequestComplete(OnRequestCompleteDelegate action)
        {
            onRequestComplete = action;
        }

        internal void SetOnUploadProgress(OnUploadProgressDelegate action)
        {
            onUploadProgress = action;
        }

        /// <inheritdoc/>
        protected override bool GetAuthCredentials(bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return getAuthCredentials?.Invoke(isProxy, host, port, realm, scheme, callback) ?? false;
        }

        /// <inheritdoc/>
        protected override void OnDownloadData(IUrlRequest request, Stream data)
        {
            onDownloadData?.Invoke(request, data);
        }

        /// <inheritdoc/>
        protected override void OnDownloadProgress(IUrlRequest request, long current, long total)
        {
            onDownloadProgress?.Invoke(request, current, total);
        }

        /// <inheritdoc/>
        protected override void OnRequestComplete(IUrlRequest request)
        {
            onRequestComplete?.Invoke(request);
        }

        /// <inheritdoc/>
        protected override void OnUploadProgress(IUrlRequest request, long current, long total)
        {
            onUploadProgress?.Invoke(request, current, total);
        }
    }
}
