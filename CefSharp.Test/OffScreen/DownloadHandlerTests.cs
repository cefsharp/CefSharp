// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class DownloadHandlerTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public DownloadHandlerTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Theory]
        [InlineData("https://code.jquery.com/jquery-3.4.1.min.js")]
        public async Task ShouldWorkWithoutAskingUser(string url)
        {
            var tcs = new TaskCompletionSource<string>(TaskContinuationOptions.RunContinuationsAsynchronously);

            using (var chromiumWebBrowser = new ChromiumWebBrowser(url, useLegacyRenderHandler: false))
            {
                var userTempPath = System.IO.Path.GetTempPath();

                chromiumWebBrowser.DownloadHandler =
                    Fluent.DownloadHandler.UseFolder(userTempPath,
                        (chromiumBrowser, browser, downloadItem, callback) =>
                        {
                            if (downloadItem.IsComplete)
                            {
                                tcs.SetResult(downloadItem.FullPath);
                            }
                            else if (downloadItem.IsCancelled)
                            {
                                tcs.SetResult(null);
                            }
                        });

                await chromiumWebBrowser.WaitForInitialLoadAsync();

                chromiumWebBrowser.StartDownload(url);

                var downloadedFilePath = await tcs.Task;

                Assert.NotNull(downloadedFilePath);
                Assert.Contains(userTempPath, downloadedFilePath);
                Assert.True(System.IO.File.Exists(downloadedFilePath));

                var downloadedFileContent = System.IO.File.ReadAllText(downloadedFilePath);

                Assert.NotEqual(0, downloadedFileContent.Length);

                var htmlSrc = await chromiumWebBrowser.GetSourceAsync();

                Assert.Contains(downloadedFileContent.Substring(0, 100), htmlSrc);

                System.IO.File.Delete(downloadedFilePath);
            }
        }
    }
}
