// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnResourceLoadCompleteEventArgs : BaseRequestEventArgs
    {
        public OnResourceLoadCompleteEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;
            Status = status;
            ReceivedContentLength = receivedContentLength;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }
        public UrlRequestStatus Status { get; private set; }
        public long ReceivedContentLength { get; private set; }
    }
}
