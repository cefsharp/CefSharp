// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Net;
using Moq;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    public class RequestContextExtensionFacts
    {
        private const string ProxyPreferenceKey = "proxy";

        private readonly ITestOutputHelper output;
        private ProxyServer proxyServer;

        private delegate void SetPreferenceDelegate(string name, object value, out string errorMessage);

        public RequestContextExtensionFacts(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory(Skip = "Starts/Stops a proxy server, will need to be run manually for now.")]
        //[Theory]
        [InlineData("http", "localhost", 8080, "http://localhost:8080")]
        [InlineData("socks", "localhost", null, "socks://localhost")]
        [InlineData(null, "localhost", null, "http://localhost")]
        public void CanSetProxyWithSchemeHostAndPort(string scheme, string host, int? port, string expected)
        {
            StartProxyServer();

            string preferenceName = "";
            object preferenceValue = null;
            var mockRequestContext = new Mock<IRequestContext>();

            mockRequestContext.Setup(x => x.CanSetPreference(ProxyPreferenceKey)).Returns(true);
            
            mockRequestContext.Setup(x => x.SetPreference(ProxyPreferenceKey, It.IsAny<IDictionary<string, object>>(), out It.Ref<string>.IsAny))
                .Callback(new SetPreferenceDelegate((string name, object value, out string errorMessage) =>
                {
                    preferenceName = name;
                    preferenceValue = value;
                    errorMessage = "OK";
                }))
                .Returns(true);

            string msg;

            var result = mockRequestContext.Object.SetProxy(scheme, host, port, out msg);
            var dict = (Dictionary<string, object>)preferenceValue;

            Assert.True(result);
            Assert.Equal("OK", msg);
            Assert.Equal(ProxyPreferenceKey, preferenceName);
            Assert.Equal(2, dict.Count);
            Assert.Equal("fixed_servers", dict["mode"]);
            Assert.Equal(expected, dict["server"]);

            StopProxyServer();
        }

        [Fact]
        public void SetProxyThrowsExceptionOnInvalidScheme()
        {
            var mockRequestContext = new Mock<IRequestContext>();

            mockRequestContext.Setup(x => x.SetPreference(ProxyPreferenceKey, It.IsAny<IDictionary<string, object>>(), out It.Ref<string>.IsAny)) .Returns(true);

            Assert.Throws<ArgumentException>(() =>
            {
                string msg;
                mockRequestContext.Object.SetProxy("myscheme", "localhost", 0, out msg);
            });
        }

        private void StartProxyServer()
        {
            proxyServer = new ProxyServer(userTrustRootCertificate: false);

            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, 8080, false);

            // An explicit endpoint is where the client knows about the existence of a proxy
            // So client sends request in a proxy friendly manner
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();
        }

        private void StopProxyServer()
        {
            proxyServer.Stop();
        }
    }
}
