// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    internal sealed class JavascriptCallback
    {
        private bool disposed;

        public long Id { get; set; }

        public int BrowserId { get; set; }

        public BrowserProcessServiceHost BrowserProcessServiceHost { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DisposeInternal();
        }

        private void DisposeInternal()
        {
            if (!disposed && BrowserProcessServiceHost != null)
            {
                BrowserProcessServiceHost = null;
            }
            disposed = true;
        }

        ~JavascriptCallback()
        {
            DisposeInternal();
        }
    }
}
