// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnResourceResponseEventArgs : BaseRequestEventArgs
    {
        public OnResourceResponseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;

            RedirectOrRetry = false; // default
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }

        /// <summary>
        ///     To allow the resource to load normally set to false.
        ///     To redirect or retry the resource, modify <see cref="OnBeforeResourceLoadEventArgs.Request" /> (url, headers or
        ///     post body) and set to true.
        /// </summary>
        public bool RedirectOrRetry { get; set; }
    }
}
