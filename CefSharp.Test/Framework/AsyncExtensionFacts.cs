// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    /// <summary>
    /// Async Extensions - This test doesn't need to be part of the 
    /// </summary>
    public class AsyncExtensionFacts
    {
        private readonly ITestOutputHelper output;

        public AsyncExtensionFacts(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task DeleteCookiesReturnsTaskWhenReturnsFalse()
        {
            var mockCookieManager = new Mock<ICookieManager>();
            //When the DeleteCookies method returns false we'll expect to get a -1 as result
            mockCookieManager.Setup(x => x.DeleteCookies("test.com", "testcookie", null)).Returns(false);

            var result = await mockCookieManager.Object.DeleteCookiesAsync("test.com", "testcookie");

            Assert.Equal(TaskDeleteCookiesCallback.InvalidNoOfCookiesDeleted, result);
        }

        [Fact]
        public async Task TaskDeleteCookiesCallbackOnComplete()
        {
            const int numberOfCookiesDeleted = 10;

            var callback = new TaskDeleteCookiesCallback();

            //Execute OnComplete on seperate Thread as in practice will be called on the CEF IO Thread.
            _ = Task.Delay(100).ContinueWith(x =>
            {
                var c = (IDeleteCookiesCallback)callback;

                c.OnComplete(numberOfCookiesDeleted);
                c.Dispose();
            }, TaskScheduler.Default);

            var result = await callback.Task;

            Assert.Equal(numberOfCookiesDeleted, result);
        }

        /// <summary>
        /// Test to validate PR https://github.com/cefsharp/CefSharp/pull/2349
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task TaskDeleteCookiesCallbackOnCompleteLoop()
        {
            const int numberOfCookiesDeleted = 10;

            for (var i = 0; i < 100; i++)
            {
                var callback = new TaskDeleteCookiesCallback();

                //Execute OnComplete on seperate Thread as in practice will be called on the CEF IO Thread.
                _ = Task.Delay(100).ContinueWith(x =>
                {
                    var c = (IDeleteCookiesCallback)callback;

                    c.OnComplete(numberOfCookiesDeleted);
                    c.Dispose();
                }, TaskScheduler.Default);

                var result = await callback.Task;

                Assert.Equal(numberOfCookiesDeleted, result);
            }
        }

        [Fact]
        public async Task TaskDeleteCookiesCallbackDispose()
        {
            var callback = new TaskDeleteCookiesCallback();

            //Execute Dispose on seperate Thread as in practice will be called on the CEF IO Thread.
            _ = Task.Delay(100).ContinueWith(x =>
            {
                var c = (IDeleteCookiesCallback)callback;

                //Dispose of the callback, no cookies were deleted
                //This is a realworld example of how CEF interacts with the callback
                c.Dispose();
            }, TaskScheduler.Default);

            var result = await callback.Task;

            Assert.Equal(TaskDeleteCookiesCallback.InvalidNoOfCookiesDeleted, result);
        }
    }
}
