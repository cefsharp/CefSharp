// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test
{
    /// <summary>
    /// Each browser instance will be created with it's own <see cref="IRequestContext"/>
    /// using an InMemory cache
    /// </summary>
    public abstract class RequestContextIsolatedBrowserTests : BrowserTests
    {
        protected RequestContextIsolatedBrowserTests()
        {
            RequestContextIsolated = true;
        }
    }
}
