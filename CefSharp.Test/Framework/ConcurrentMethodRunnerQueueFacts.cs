// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading;
using System.Threading.Tasks;
using CefSharp.Example.JavascriptBinding;
using CefSharp.Internals;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    public class ConcurrentMethodRunnerQueueFacts
    {
        private readonly ITestOutputHelper output;

        public ConcurrentMethodRunnerQueueFacts(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task StopConcurrentMethodRunnerQueueWhenMethodRunning()
        {
            var boundObject = new AsyncBoundObject();

            var objectRepository = new JavascriptObjectRepository();
            objectRepository.Register("testObject", boundObject, true, new BindingOptions { CamelCaseJavascriptNames = false });
            var methodInvocation = new MethodInvocation(1, 1, 1, nameof(boundObject.AsyncWaitTwoSeconds), 1);
            methodInvocation.Parameters.Add("Echo Me!");
            var methodRunnerQueue = new ConcurrentMethodRunnerQueue(objectRepository);

            methodRunnerQueue.Enqueue(methodInvocation);

            //Wait a litle for the queue to start processing our Method call
            await Task.Delay(500);

            var ex = Record.Exception(() => methodRunnerQueue.Dispose());

            Assert.Null(ex);
        }

        [Fact]
        public void ValidateAsyncTaskMethodOutput()
        {
            const string expectedResult = "Echo Me!";
            var boundObject = new AsyncBoundObject();

            var objectRepository = new JavascriptObjectRepository();
            objectRepository.Register("testObject", boundObject, true, new BindingOptions { CamelCaseJavascriptNames = false });
            var methodInvocation = new MethodInvocation(1, 1, 1, nameof(boundObject.AsyncWaitTwoSeconds), 1);
            methodInvocation.Parameters.Add(expectedResult);
            var methodRunnerQueue = new ConcurrentMethodRunnerQueue(objectRepository);
            var manualResetEvent = new ManualResetEvent(false);

            var actualResult = "";

            methodRunnerQueue.MethodInvocationComplete += (sender, args) =>
            {
                methodRunnerQueue.Dispose();

                actualResult = args.Result.Result.ToString();

                manualResetEvent.Set();
            };

            methodRunnerQueue.Enqueue(methodInvocation);

            manualResetEvent.WaitOne(3000);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
