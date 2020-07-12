// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading;

namespace CefSharp.Internals
{
    public class NoOpBrowserRefCounter : IBrowserRefCounter
    {
        int IBrowserRefCounter.Count
        {
            get { return 0; }
        }

        bool IBrowserRefCounter.Decrement()
        {
            return false;
        }

        void IBrowserRefCounter.Increment()
        {

        }

        void IBrowserRefCounter.WaitForBrowsersToClose(int timeoutInMiliseconds)
        {

        }

        void IBrowserRefCounter.WaitForBrowsersToClose(int timeoutInMiliseconds, CancellationToken cancellationToken)
        {

        }
    }
}
