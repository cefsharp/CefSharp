// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit.Abstractions;
using Xunit;
using System.Threading.Tasks;
using CefSharp.DevTools.Page;
using System.IO;
using CefSharp.OffScreen;
using System.Drawing;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class ScreenshotTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public ScreenshotTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWork()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com", useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var result1 = await browser.CaptureScreenshotAsync();
                Assert.Equal(1366, browser.Size.Width);
                Assert.Equal(768, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result1)))
                {
                    Assert.Equal(1366, screenshot.Width);
                    Assert.Equal(768, screenshot.Height);
                }

                var result2 = await browser.CaptureScreenshotAsync(viewport: new Viewport { Width = 1366, Height = 768, X = 100, Y = 200, Scale = 2 });
                Assert.Equal(1466, browser.Size.Width);
                Assert.Equal(968, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result2)))
                {
                    Assert.Equal(2732, screenshot.Width);
                    Assert.Equal(1536, screenshot.Height);
                }

                var result3 = await browser.CaptureScreenshotAsync(viewport: new Viewport { Width = 100, Height = 200, Scale = 2 });
                Assert.Equal(1466, browser.Size.Width);
                Assert.Equal(968, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result3)))
                {
                    Assert.Equal(200, screenshot.Width);
                    Assert.Equal(400, screenshot.Height);
                }

                var result4 = await browser.CaptureScreenshotAsync(viewport: new Viewport { Width = 100, Height = 200, Scale = 1 });
                Assert.Equal(1466, browser.Size.Width);
                Assert.Equal(968, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);
                using (var screenshot = Image.FromStream(new MemoryStream(result4)))
                {
                    Assert.Equal(100, screenshot.Width);
                    Assert.Equal(200, screenshot.Height);
                }
            }
        }

        [Fact]
        public async Task ShouldWorkWhenResizingWithDeviceScalingFactor()
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                Assert.Equal(1366, browser.Size.Width);
                Assert.Equal(768, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);


                await browser.ResizeAsync(800, 600, 2);

                Assert.Equal(800, browser.Size.Width);
                Assert.Equal(600, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);

                using (var screenshot = browser.ScreenshotOrNull())
                {
                    Assert.Equal(1600, screenshot.Width);
                    Assert.Equal(1200, screenshot.Height);
                }

                await browser.ResizeAsync(400, 300);

                Assert.Equal(400, browser.Size.Width);
                Assert.Equal(300, browser.Size.Height);
                Assert.Equal(2, browser.DeviceScaleFactor);

                using (var screenshot = browser.ScreenshotOrNull())
                {
                    Assert.Equal(800, screenshot.Width);
                    Assert.Equal(600, screenshot.Height);
                }

                await browser.ResizeAsync(1366, 768, 1);

                Assert.Equal(1366, browser.Size.Width);
                Assert.Equal(768, browser.Size.Height);
                Assert.Equal(1, browser.DeviceScaleFactor);

                using (var screenshot = browser.ScreenshotOrNull())
                {
                    Assert.Equal(1366, screenshot.Width);
                    Assert.Equal(768, screenshot.Height);
                }
            }
        }
    }
}
