// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IURLRequestClient
    {
        bool GetAuthCredentials(bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback);
        void OnDownloadData(IURLRequest request, byte[] data);
        void OnDownloadProgress(IURLRequest request, long current, long total);
        void OnRequestComplete(IURLRequest request);
        void OnUploadProgress(IURLRequest request, long current, long total);
    }
}
