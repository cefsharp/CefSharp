// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Wpf
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class WpfBrowserBasicFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public WpfBrowserBasicFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        //TODO: Add tests
        //Investigate using https://github.com/AArnott/Xunit.StaFact for STA thread management
    }
}
