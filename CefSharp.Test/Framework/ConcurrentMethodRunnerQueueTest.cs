// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Example.JavascriptBinding;
using CefSharp.Internals;
using Xunit;
using Xunit.Abstractions;
using Moq;

namespace CefSharp.Test.Framework
{
    public class ConcurrentMethodRunnerQueueTest
    {
        private readonly ITestOutputHelper output;

        public ConcurrentMethodRunnerQueueTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Recreate the workflow that would appear to be the root cause of
        /// https://github.com/cefsharp/CefSharp/discussions/3638
        /// </summary>
        [Fact]
        public void SimulateStartOnTaskAlreadyCompleted()
        {
            var cts = new CancellationTokenSource();

            //Create a new Task
            var task = new Task(async () =>
            {
                await Task.Delay(100);

            }, cts.Token);

            //Cancel before started
            cts.Cancel();

            Assert.Throws<InvalidOperationException>(() => task.Start(TaskScheduler.Default));
        }

        /// <summary>
        /// Proposed fix for
        /// https://github.com/cefsharp/CefSharp/discussions/3638
        /// It's difficult to recreate the exact threading requirements, so simulating the
        /// behaviour to test code executes without exception.
        /// </summary>
        [Fact]
        public void SimulateTaskRunStartOnTaskAlreadyCompleted()
        {
            var cts = new CancellationTokenSource();

            //Cancel before started
            cts.Cancel();

            var task = Task.Run(async () =>
            {
                await Task.Delay(100);
            }, cts.Token);

            Assert.NotNull(task);
            Assert.Equal(TaskStatus.Canceled, task.Status);
        }

        [Fact]
        public void ShouldWorkWhenEnqueueCalledAfterDispose()
        {
            var methodInvocation = new MethodInvocation(1, "1", 1, "Testing", 1);
            methodInvocation.Parameters.Add("Echo Me!");

            var objectRepository = new JavascriptObjectRepository
            {
                NameConverter = null
            };

            var methodRunnerQueue = new ConcurrentMethodRunnerQueue(objectRepository);

            //Dispose
            methodRunnerQueue.Dispose();

            //Enqueue
            var ex = Record.Exception(() => methodRunnerQueue.Enqueue(methodInvocation));

            //Ensure no exception thrown
            Assert.Null(ex);
        }

        [Fact]
        public async Task ShouldDisposeWhenRunningWithoutException()
        {
            var boundObject = new AsyncBoundObject();

            IJavascriptObjectRepositoryInternal objectRepository = new JavascriptObjectRepository();
            objectRepository.NameConverter = null;
#if NETCOREAPP
            objectRepository.Register("testObject", boundObject, BindingOptions.DefaultBinder);
#else
            objectRepository.Register("testObject", boundObject, true, BindingOptions.DefaultBinder);
#endif
            var methodInvocation = new MethodInvocation(1, "1", 1, nameof(boundObject.AsyncWaitTwoSeconds), 1);
            methodInvocation.Parameters.Add("Echo Me!");
            var methodRunnerQueue = new ConcurrentMethodRunnerQueue(objectRepository);

            methodRunnerQueue.Enqueue(methodInvocation);

            //Wait a litle for the queue to start processing our Method call
            await Task.Delay(500);

            var ex = Record.Exception(() => methodRunnerQueue.Dispose());

            Assert.Null(ex);
        }

        [Fact]
        public async Task ShouldCallMethodAsync()
        {
            const string expected = "Echo Me!";
            const string methodName = "AsyncWaitTwoSeconds";

            var mockObjectRepository = new Mock<IJavascriptObjectRepositoryInternal>();
            mockObjectRepository.Setup(x => x.TryCallMethodAsync(1, methodName, It.IsAny<object[]>())).ReturnsAsync(new TryCallMethodResult(true, expected, string.Empty));
            var methodInvocation = new MethodInvocation(1, "1", 1, methodName, 1);
            methodInvocation.Parameters.Add(expected);

            using var methodRunnerQueue = new ConcurrentMethodRunnerQueue(mockObjectRepository.Object);

            var evt = await AssertEx.RaisesAsync<MethodInvocationCompleteArgs>(
                cancelAfter: 10000,
                x => methodRunnerQueue.MethodInvocationComplete += x,
                x => methodRunnerQueue.MethodInvocationComplete -= x,
                () => methodRunnerQueue.Enqueue(methodInvocation));

            Assert.Equal(expected, evt.Arguments.Result.Result);
        }
    }
}
