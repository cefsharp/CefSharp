// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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

        public IFrame Frame { get; }
        public IRequest Request { get; }
        public IResponse Response { get; }
        public UrlRequestStatus Status { get; }
        public long ReceivedContentLength { get; }
    }
}
