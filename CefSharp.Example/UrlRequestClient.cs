// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Example
{
    public class UrlRequestClient : IUrlRequestClient
    {
        private readonly Action<IUrlRequest, byte[]> completeAction;
        private readonly MemoryStream responseBody = new MemoryStream();

        public UrlRequestClient(Action<IUrlRequest, byte[]> completeAction)
        {
            this.completeAction = completeAction;
        }

        bool IUrlRequestClient.GetAuthCredentials(bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return true;
        }

        void IUrlRequestClient.OnDownloadData(IUrlRequest request, Stream data)
        {
            data.CopyTo(responseBody);
        }

        void IUrlRequestClient.OnDownloadProgress(IUrlRequest request, long current, long total)
        {

        }

        void IUrlRequestClient.OnRequestComplete(IUrlRequest request)
        {
            this?.completeAction(request, responseBody.ToArray());
        }

        void IUrlRequestClient.OnUploadProgress(IUrlRequest request, long current, long total)
        {

        }
    }
}
