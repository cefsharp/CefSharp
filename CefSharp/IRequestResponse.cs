// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
    public interface IRequestResponse
    {
        /// cancel the request, return nothing
        void Cancel();

        /// respond with redirection to the provided URL
        void Redirect(string url);

        /// respond with data from Stream
        void RespondWith(Stream stream, string mimeType);
    }
}
