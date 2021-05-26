// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp.Fluent
{
    /// <summary>
    /// Fluent UrlRequestClient Builder
    /// </summary>
    public class UrlRequestClientBuilder
    {
        private UrlRequestClient client = new UrlRequestClient();

        /// <summary>
        /// See <see cref="IUrlRequestClient.GetAuthCredentials(bool, string, int, string, string, IAuthCallback)"/> for details
        /// </summary>
        /// <param name="func">function to be executed when <see cref="IUrlRequestClient.GetAuthCredentials(bool, string, int, string, string, IAuthCallback)"/>
        /// is called </param>
        /// <returns>
        /// Fluent Builder, call <see cref="UrlRequestClientBuilder.Build"/> to create
        /// a new <see cref="IUrlRequestClient"/> instance
        /// </returns>
        public UrlRequestClientBuilder GetAuthCredentials(GetAuthCredentialsDelegate func)
        {
            client.SetGetAuthCredentials(func);

            return this;
        }

        /// <summary>
        /// See <see cref="IUrlRequestClient.OnDownloadData(IUrlRequest, Stream)"/> for details.
        /// </summary>
        /// <param name="action">Action to be executed when <see cref="IUrlRequestClient.OnDownloadData(IUrlRequest, Stream)"/>
        /// is called</param>
        /// <returns>
        /// Fluent Builder, call <see cref="UrlRequestClientBuilder.Build"/> to create
        /// a new <see cref="IUrlRequestClient"/> instance
        /// </returns>
        public UrlRequestClientBuilder OnDownloadData(OnDownloadDataDelegate action)
        {
            client.SetOnDownloadData(action);

            return this;
        }

        /// <summary>
        /// See <see cref="IUrlRequestClient.OnDownloadProgress(IUrlRequest, long, long)"/> for details.
        /// </summary>
        /// <param name="action">Action to be executed when <see cref="IUrlRequestClient.OnDownloadProgress(IUrlRequest, long, long)"/>
        /// is called</param>
        /// <returns>
        /// Fluent Builder, call <see cref="UrlRequestClientBuilder.Build"/> to create
        /// a new <see cref="IUrlRequestClient"/> instance
        /// </returns>
        public UrlRequestClientBuilder OnDownloadProgress(OnDownloadProgressDelegate action)
        {
            client.SetOnDownloadProgress(action);

            return this;
        }

        /// <summary>
        /// See <see cref="IUrlRequestClient.OnRequestComplete"/> for details.
        /// </summary>
        /// <param name="action">Action to be executed when <see cref="IUrlRequestClient.OnRequestComplete(IUrlRequest)"/>
        /// is called</param>
        /// <returns>
        /// Fluent Builder, call <see cref="UrlRequestClientBuilder.Build"/> to create
        /// a new <see cref="IUrlRequestClient"/> instance
        /// </returns>
        public UrlRequestClientBuilder OnRequestComplete(OnRequestCompleteDelegate action)
        {
            client.SetOnRequestComplete(action);

            return this;
        }

        /// <summary>
        /// See <see cref="IUrlRequestClient.OnUploadProgress(IUrlRequest, long, long)"/> for details.
        /// </summary>
        /// <param name="action">Action to be executed when <see cref="IUrlRequestClient.OnUploadProgress(IUrlRequest, long, long)"/>
        /// is called</param>
        /// <returns>
        /// Fluent Builder, call <see cref="UrlRequestClientBuilder.Build"/> to create
        /// a new <see cref="IUrlRequestClient"/> instance
        /// </returns>
        public UrlRequestClientBuilder OnUploadProgress(OnUploadProgressDelegate action)
        {
            client.SetOnUploadProgress(action);

            return this;
        }

        /// <summary>
        /// Create a <see cref="IUrlRequestClient"/> instance
        /// </summary>
        /// <returns> a <see cref="IUrlRequestClient"/> instance</returns>
        public IUrlRequestClient Build()
        {
            return client;
        }
    }
}
