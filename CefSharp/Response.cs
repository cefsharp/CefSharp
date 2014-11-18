// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public class Response : IResponse
    {
        public String RedirectUrl { get; private set; }
        public ResponseAction Action { get; private set; }

        public Response()
        {
            Action = ResponseAction.Continue;
        }

        public void Cancel()
        {
            Action = ResponseAction.Cancel;
        }

        public void Redirect(String url)
        {
            RedirectUrl = url;
            Action = ResponseAction.Redirect;
        }
    }
}
