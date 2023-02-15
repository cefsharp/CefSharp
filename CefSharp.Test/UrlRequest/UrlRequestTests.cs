// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.UrlRequest
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class UrlRequestTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public UrlRequestTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWork()
        {
            var taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            IUrlRequest urlRequest = null;
            int statusCode = -1;

            //Can be created on any valid CEF Thread, here we'll use the CEF UI Thread
            await Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var requestClient = new Example.UrlRequestClient((IUrlRequest req, byte[] responseBody) =>
                {
                    statusCode = req.Response.StatusCode;
                    taskCompletionSource.TrySetResult(Encoding.UTF8.GetString(responseBody));
                });

                var request = new Request
                {
                    Method = "GET",
                    Url = "https://code.jquery.com/jquery-3.4.1.min.js"
                };

                //Global RequestContext will be used
                urlRequest = new CefSharp.UrlRequest(request, requestClient);
            });

            var stringResult = await taskCompletionSource.Task;

            Assert.True(!string.IsNullOrEmpty(stringResult));
            Assert.Equal(200, statusCode);
        }
    }
}
