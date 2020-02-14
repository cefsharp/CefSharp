// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
// 
using System.Threading;

namespace CefSharp.Internals
{
    public interface IBrowserRefCounter
    {
        int Count { get; }

        bool Decrement();
        void Increment();
        void WaitForBrowsersToClose(int timeoutInMiliseconds = 500);
        void WaitForBrowsersToClose(int timeoutInMiliseconds, CancellationToken cancellationToken);
    }
}
