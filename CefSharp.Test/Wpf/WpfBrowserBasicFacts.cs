// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using System.Windows;
using CefSharp.Wpf;
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

        [WpfFact]
        public async Task CanLoadGoogle()
        {
            using (var browser = new ChromiumWebBrowser(null, "www.google.com", new Size(1024, 786)))
            {
                await browser.LoadPageAsync();

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Contains("www.google", mainFrame.Url);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }
    }
}
