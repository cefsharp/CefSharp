// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit;

namespace CefSharp.Test.Framework
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class RequestContextFacts
    {
        [Fact]
        public void IsSameAs()
        {
            var ctx1 = new RequestContext();
            var ctx2 = ctx1.UnWrap();

            Assert.True(ctx1.IsSame(ctx2));
        }

        [Fact]
        public void IsSharingWith()
        {
            var ctx1 = RequestContext.Configure()
                .WithCachePath(@"c:\temp")
                .Create();
            var ctx2 = new RequestContext(ctx1);

            Assert.True(ctx1.IsSharingWith(ctx2));
        }
    }
}
