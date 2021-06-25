// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using Xunit;

namespace CefSharp.Test.Framework
{
    public class MethodRunnerQueueFacts
    {
        [Fact]
        public void DisposeQueueThenEnqueueMethodInvocation()
        {
            var methodInvocation = new MethodInvocation(1, 1, 1, "Testing", 1);
            methodInvocation.Parameters.Add("Echo Me!");

            var objectRepository = new JavascriptObjectRepository
            {
                NameConverter = null
            };

            var methodRunnerQueue = new MethodRunnerQueue(objectRepository);

            //Dispose
            methodRunnerQueue.Dispose();

            //Enqueue
            var ex = Record.Exception(() => methodRunnerQueue.Enqueue(methodInvocation));

            //Ensure no exception thrown
            Assert.Null(ex);
        }
    }
}
