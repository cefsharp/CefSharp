// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Fluent
{
    /// <summary>
    /// Fluent UrlRequestClient
    /// </summary>
    public class UrlRequestClient : CefSharp.UrlRequestClient
    {
        private Func<bool, string, int, string, string, IAuthCallback, bool> getAuthCredentials;
        private Action<IUrlRequest, Stream> onDownloadData;
        private Action<IUrlRequest, long, long> onDownloadProgress;
        private Action<IUrlRequest> onRequestComplete;
        private Action<IUrlRequest, long, long> onUploadProgress;

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

        internal void SetGetAuthCredentials(Func<bool, string, int, string, string, IAuthCallback, bool> func)
        {
            getAuthCredentials = func;
        }

        internal void SetOnDownloadData(Action<IUrlRequest, Stream> action)
        {
            onDownloadData = action;
        }

        internal void SetOnDownloadProgress(Action<IUrlRequest, long, long> action)
        {
            onDownloadProgress = action;
        }

        internal void SetOnRequestComplete(Action<IUrlRequest> action)
        {
            onRequestComplete = action;
        }

        internal void SetOnUploadProgress(Action<IUrlRequest, long, long> action)
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
