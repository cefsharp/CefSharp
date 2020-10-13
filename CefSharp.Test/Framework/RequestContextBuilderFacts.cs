// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class RequestContextBuilderFacts
    {
        private CefSharpFixture fixture;
        private ITestOutputHelper output;

        public RequestContextBuilderFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public void ThrowExceptionWithSharedSettingsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var requestContext = RequestContext
                    .Configure()
                    .WithSharedSettings(null);
            });
        }

        [Fact]
        public void ThrowExceptionIfContextAlreadySpecified()
        {
            Assert.Throws<Exception>(() =>
            {
                var requestContext = new RequestContext();

                RequestContext
                    .Configure()
                    .WithSharedSettings(requestContext)
                    .WithCachePath("shouldThrowException");
            });
        }

        [Fact]
        public void ThrowExceptionIfCachePathAlreadySpecified()
        {
            Assert.Throws<Exception>(() =>
            {
                var requestContext = new RequestContext();

                RequestContext
                    .Configure()
                    .WithCachePath("c:\\temp")
                    .WithSharedSettings(requestContext);
            });
        }
    }
}
