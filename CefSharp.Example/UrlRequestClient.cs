// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Example
{

    public class UrlRequestClient : IUrlRequestClient
    {
        private Action<IUrlRequest, byte[]> CompleteAction;
        private MemoryStream ResponseBody = new MemoryStream();

        public UrlRequestClient(Action<IUrlRequest, byte[]> completeAction)
        {
            CompleteAction = completeAction;
        }
        public bool GetAuthCredentials(bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return true;
        }

        public void OnDownloadData(IUrlRequest request, Stream data)
        {
            data.CopyTo(ResponseBody);
        }

        public void OnDownloadProgress(IUrlRequest request, long current, long total)
        {
            return;
        }

        public void OnRequestComplete(IUrlRequest request)
        {

            CompleteAction(request, ResponseBody.ToArray());
        }

        public void OnUploadProgress(IUrlRequest request, long current, long total)
        {
            return;
        }
    }
}
