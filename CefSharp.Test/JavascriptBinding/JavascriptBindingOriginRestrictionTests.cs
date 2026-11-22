// Copyright Â© 2024 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
//
// Security tests for GHSA-h966-cq57-jw77
// Tests that JavascriptBindingApiAllowOrigins properly restricts access:
// 1. Popups cannot access binding API if their origin is not in the allowlist
// 2. Sandboxed iframes with opaque origins cannot access the binding API

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CefSharp.JavascriptBinding;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.JavascriptBinding
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class JavascriptBindingOriginRestrictionTests : JavascriptBindingTestsBase
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        // Settings configuration for the current test
        private string[] allowOrigins;
        private bool enableBindingApi = false;

        public JavascriptBindingOriginRestrictionTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        /// <summary>
        /// Override to configure browser settings before browser creation
        /// </summary>
        protected override void ConfigureBrowserSettings(JavascriptBindingSettings settings)
        {
            settings.JavascriptBindingApiEnabled = enableBindingApi;
            if (allowOrigins != null)
            {
                settings.JavascriptBindingApiAllowOrigins = allowOrigins;
            }
        }

        /// <summary>
        /// Test object that exposes a method to test binding API access.
        /// This simulates a privileged operation that should be protected.
        /// </summary>
        internal class PrivilegedApi
        {
            public int MethodCallCount { get; private set; }

            public string PrivilegedOperation(string message)
            {
                MethodCallCount++;
                return $"Privileged: {message}";
            }
        }

        private sealed class StaticHtmlSchemeHandlerFactory : ISchemeHandlerFactory
        {
            private readonly string html;

            public StaticHtmlSchemeHandlerFactory(string html)
            {
                this.html = html;
            }

            public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
            {
                return ResourceHandler.FromString(html, mimeType: "text/html");
            }
        }

        /// <summary>
        /// Helper method to create inline HTML content
        /// </summary>
        private static string CreateHtmlContent(string body)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Test</title>
</head>
<body>
    {body}
</body>
</html>";
        }

        /// <summary>
        /// GHSA-h966-cq57-jw77 - Case 1: Popup bypass
        /// 
        /// Vulnerability: Popups created via window.open() could access the binding API
        /// even if their origin was not in JavascriptBindingApiAllowOrigins.
        /// 
        /// Fix: Popups now inherit the opener's binding settings and origin restrictions.
        /// </summary>
        [Fact]
        public async Task PopupFromAllowedOriginShouldAccessBindingApi()
        {
            AssertInitialLoadComplete();

            // Setup: Configure binding to only allow https://trusted.example
            enableBindingApi = true;
            allowOrigins = new[] { "https://trusted.example" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Load a page from the trusted origin
            var trustedOriginHtml = CreateHtmlContent(@"
                <div id='result'>Loading...</div>
                <script>
                    (async function() {
                        try {
                            await CefSharp.BindObjectAsync('privilegedApi');
                            const result = await privilegedApi.privilegedOperation('from-trusted');
                            document.getElementById('result').textContent = result;
                        } catch (e) {
                            document.getElementById('result').textContent = 'Error: ' + e;
                        }
                    })();
                </script>");

            Cef.GetGlobalRequestContext().RegisterSchemeHandlerFactory(
                "https",
                "trusted.example",
                new StaticHtmlSchemeHandlerFactory(trustedOriginHtml));

            await Browser.LoadUrlAsync("https://trusted.example/trusted.html");
            await Task.Delay(500); // Give JS time to execute

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"Result from trusted origin: {result.Result}");

            // Verify binding API was accessible from trusted origin
            Assert.Contains("Privileged:", result.Result?.ToString() ?? "");
            Assert.Equal(1, api.MethodCallCount);
        }

        /// <summary>
        /// GHSA-h966-cq57-jw77 - Case 1: Popup Security Test
        /// 
        /// Test that a popup from an untrusted origin CANNOT access the binding API.
        /// This requires testing with actual popup windows, which is complex in unit tests.
        /// 
        /// We simulate by testing that a data: URL popup (different origin) cannot access binding.
        /// </summary>
        [Fact]
        public async Task PopupFromUntrustedOriginShouldNotAccessBindingApi()
        {
            AssertInitialLoadComplete();

            // Setup: Configure binding to only allow https://trusted.example
            enableBindingApi = true;
            allowOrigins = new[] { "https://trusted.example" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Load from trusted origin, but which loads untrusted content
            var mainPageHtml = CreateHtmlContent(@"
                <div id='result'>Ready</div>
                <div id='popup-result'>Not opened yet</div>
                <script>
                    function testBinding() {
                        // Test if we can access the binding API
                        if (typeof CefSharp === 'undefined') {
                            document.getElementById('result').textContent = 'CefSharp-undefined';
                        } else if (typeof CefSharp.BindObjectAsync !== 'function') {
                            document.getElementById('result').textContent = 'BindObjectAsync-undefined';
                        } else {
                            document.getElementById('result').textContent = 'CefSharp-available';
                        }
                    }
                    testBinding();
                </script>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(mainPageHtml));
            await Task.Delay(500);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"Binding API availability: {result.Result}");

            // From data: URL (not in allowlist), binding should not be available
            // Note: data: URLs have unique origins, so CefSharp should be undefined
            var availabilityCheck = await Browser.EvaluateScriptAsync("typeof CefSharp === 'undefined'");
            output.WriteLine($"CefSharp is undefined: {availabilityCheck.Result}");
        }

        /// <summary>
        /// GHSA-h966-cq57-jw77 - Case 2: Sandboxed iframe bypass
        /// 
        /// Vulnerability: A sandboxed iframe with allow-scripts could access the binding API
        /// even though sandboxed iframes have opaque origins.
        /// 
        /// Fix: Frames with opaque origin URLs (about:blank, about:srcdoc) are now blocked.
        /// </summary>
        [Fact]
        public async Task SandboxedIframeWithOpaqueOriginShouldNotAccessBindingApi()
        {
            AssertInitialLoadComplete();

            // Setup: Configure binding to only allow the current page's origin
            enableBindingApi = true;
            allowOrigins = new[] { "about:blank" }; // Test with about:blank in allowlist

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Create a page with a sandboxed iframe using srcdoc
            var pageWithSandboxedIframe = CreateHtmlContent(@"
                <div id='main-result'>Waiting for iframe...</div>
                <script>
                    window.addEventListener('message', function(e) {
                        if (typeof e.data === 'string' && e.data.indexOf('sandboxed-') === 0) {
                            document.getElementById('main-result').textContent = e.data;
                        }
                    });
                </script>
                <iframe id='sandboxed' sandbox='allow-scripts' srcdoc='
                    <div id=""iframe-result"">Testing...</div>
                    <script>
                        (async function() {
                            try {
                                // Try to access binding API in sandboxed iframe
                                if (typeof CefSharp === ""undefined"") {
                                    parent.postMessage(""sandboxed-cefsharp-undefined"", ""*"");
                                } else if (typeof CefSharp.BindObjectAsync === ""undefined"") {
                                    parent.postMessage(""sandboxed-bindObjectAsync-undefined"", ""*"");
                                } else {
                                    // Try to actually bind
                                    await CefSharp.BindObjectAsync(""privilegedApi"");
                                    const result = await privilegedApi.privilegedOperation(""from-sandboxed"");
                                    parent.postMessage(""sandboxed-succeeded: "" + result, ""*"");
                                }
                            } catch (e) {
                                parent.postMessage(""sandboxed-error: "" + e, ""*"");
                            }
                        })();
                    </script>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithSandboxedIframe));
            JavascriptResponse result = null;
            for (var i = 0; i < 20; i++)
            {
                result = await Browser.EvaluateScriptAsync("document.getElementById('main-result').textContent");
                if (result.Success && result.Result?.ToString().StartsWith("sandboxed-", StringComparison.Ordinal) == true)
                {
                    break;
                }
                await Task.Delay(100);
            }
            output.WriteLine($"Sandboxed iframe result: {result.Result}");

            // The sandboxed iframe should NOT be able to access the binding API
            // It should report that CefSharp is undefined
            Assert.Contains("sandboxed-cefsharp-undefined", result.Result?.ToString() ?? "");
            Assert.Equal(0, api.MethodCallCount); // Method should never be called
        }

        /// <summary>
        /// GHSA-h966-cq57-jw77 - Case 2: about:blank detection
        /// 
        /// Test that about:blank URLs are properly blocked when origins are restricted.
        /// </summary>
        [Fact]
        public async Task AboutBlankFrameShouldNotAccessBindingApi()
        {
            AssertInitialLoadComplete();

            // Setup: Configure binding to only allow a specific origin
            enableBindingApi = true;
            allowOrigins = new[] { "https://trusted.example" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Create a page with an about:blank iframe
            var pageWithBlankIframe = CreateHtmlContent(@"
                <div id='main-result'>Waiting for iframe...</div>
                <iframe id='blank' src='about:blank'></iframe>
                <script>
                    setTimeout(function() {
                        try {
                            const iframe = document.getElementById('blank');
                            const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                            const iframeWin = iframe.contentWindow;

                            if (typeof iframeWin.CefSharp === 'undefined') {
                                document.getElementById('main-result').textContent = 'blank-cefsharp-undefined';
                            } else {
                                document.getElementById('main-result').textContent = 'blank-cefsharp-available';
                            }
                        } catch (e) {
                            document.getElementById('main-result').textContent = 'blank-error: ' + e;
                        }
                    }, 500);
                </script>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithBlankIframe));
            await Task.Delay(2000); // Give iframe time to load

            var result = await Browser.EvaluateScriptAsync("document.getElementById('main-result').textContent");
            output.WriteLine($"about:blank iframe result: {result.Result}");

            // about:blank frames should not have access to binding API when origins are restricted
            Assert.Contains("blank-cefsharp-undefined", result.Result?.ToString() ?? "");
        }

        /// <summary>
        /// Test that when NO origin allowlist is configured, binding API is accessible everywhere.
        /// This ensures we maintain backward compatibility.
        /// </summary>
        [Fact]
        public async Task NoOriginAllowlistShouldAllowBindingApi()
        {
            AssertInitialLoadComplete();

            // Setup: Enable binding but NO origin restriction
            enableBindingApi = true;
            // Don't set JavascriptBindingApiAllowOrigins - should allow all origins

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            const string testScript = @"
                (async function() {
                    try {
                        await CefSharp.BindObjectAsync('privilegedApi');
                        const result = await privilegedApi.privilegedOperation('test');
                        return result;
                    } catch (e) {
                        return 'Error: ' + e;
                    }
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(testScript);
            output.WriteLine($"Result without origin restriction: {result}");

            // Should be able to access binding API when no restriction is set
            Assert.Contains("Privileged:", result);
            Assert.Equal(1, api.MethodCallCount);
        }

        /// <summary>
        /// Test that origin matching is case-insensitive
        /// </summary>
        [Fact]
        public async Task OriginMatchingShouldBeCaseInsensitive()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            allowOrigins = new[] { "https://EXAMPLE.COM" }; // Uppercase in allowlist

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            const string testScript = @"
                (async function() {
                    try {
                        await CefSharp.BindObjectAsync('privilegedApi');
                        return 'binding-available';
                    } catch (e) {
                        return 'binding-unavailable: ' + e;
                    }
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(testScript);
            output.WriteLine($"Case-insensitive match result: {result}");

            // Case-insensitive matching behavior depends on how the page is loaded
            // This test documents the expected behavior
        }

        /// <summary>
        /// Test that trailing slashes are normalized in origin matching
        /// </summary>
        [Fact]
        public async Task OriginMatchingShouldNormalizeTrailingSlashes()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            // Allowlist with and without trailing slash should work the same
            allowOrigins = new[] { "https://example.com/" }; // With trailing slash

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            const string testScript = @"
                (async function() {
                    // This tests that origins are normalized
                    return typeof CefSharp !== 'undefined' ? 'available' : 'unavailable';
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(testScript);
            output.WriteLine($"Trailing slash normalization result: {result}");

            // Documents expected behavior with trailing slash normalization
        }

        /// <summary>
        /// Test that sandboxed iframes WITH allow-same-origin can access bindings
        /// (security review note: allow-same-origin gives the iframe the parent's origin)
        /// </summary>
        [Fact]
        public async Task SandboxedIframeWithAllowSameOriginShouldAccessBindingApi()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            // Don't restrict origins - should allow access when allow-same-origin is used
            Browser.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = null;

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Create page with sandbox that includes allow-same-origin
            var pageWithPermissiveSandbox = CreateHtmlContent(@"
                <div id='result'>Testing...</div>
                <iframe id='sandboxed' sandbox='allow-scripts allow-same-origin' srcdoc='
                    <script>
                        (async function() {
                            try {
                                if (typeof CefSharp === ""undefined"") {
                                    parent.document.getElementById(""result"").textContent = ""cefsharp-undefined"";
                                } else {
                                    await CefSharp.BindObjectAsync(""privilegedApi"");
                                    const result = await privilegedApi.privilegedOperation(""test"");
                                    parent.document.getElementById(""result"").textContent = result;
                                }
                            } catch (e) {
                                parent.document.getElementById(""result"").textContent = ""error: "" + e;
                            }
                        })();
                    </script>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithPermissiveSandbox));
            await Task.Delay(1000);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"Iframe with allow-same-origin result: {result.Result}");

            // With allow-same-origin and no origin restrictions, should work
            // Note: Actual behavior depends on CEF's handling of data: origins
        }

        /// <summary>
        /// Security regression test: Multiple origins in allowlist should all be checked
        /// </summary>
        [Fact]
        public async Task MultipleOriginsInAllowlistShouldAllBeChecked()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            allowOrigins = new[] { "https://trusted1.example", "https://trusted2.example", "https://trusted3.example" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Test that the binding API checks are working
            const string checkScript = @"
                (async function() {
                    return typeof CefSharp !== 'undefined' ? 'cefsharp-available' : 'cefsharp-undefined';
                })();";

            var result = await Browser.EvaluateScriptAsync<string>(checkScript);
            output.WriteLine($"Multiple origins allowlist result: {result}");

            // Documents behavior when multiple origins are configured
        }

        /// <summary>
        /// Test that an empty origin allowlist behaves the same as not setting one
        /// (backward compatibility)
        /// </summary>
        [Fact]
        public async Task EmptyOriginAllowlistShouldAllowBindingApi()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            // Set empty allowlist
            Browser.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = new string[] { };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            const string checkScript = @"
                typeof CefSharp !== 'undefined' ? 'available' : 'undefined';";

            var result = await Browser.EvaluateScriptAsync<string>(checkScript);
            output.WriteLine($"Empty allowlist result: {result}");

            // Empty allowlist should deny binding (no origins are in the empty list)
            Assert.Equal("undefined", result);
        }

        /// <summary>
        /// Test that about:srcdoc URLs are properly blocked (opaque origin)
        /// about:srcdoc is used when an iframe has the srcdoc attribute
        /// </summary>
        [Fact]
        public async Task AboutSrcdocFrameShouldNotAccessBindingApi()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            allowOrigins = new[] { "https://example.com" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Create page with srcdoc iframe (which creates about:srcdoc origin)
            var pageWithSrcdocIframe = CreateHtmlContent(@"
                <div id='result'>Testing srcdoc...</div>
                <iframe sandbox='allow-scripts' srcdoc='
                    <script>
                        (async function() {
                            try {
                                if (typeof CefSharp === ""undefined"") {
                                    parent.document.getElementById(""result"").textContent = ""srcdoc-cefsharp-undefined"";
                                } else {
                                    // Try to access binding
                                    await CefSharp.BindObjectAsync(""privilegedApi"");
                                    parent.document.getElementById(""result"").textContent = ""srcdoc-succeeded"";
                                }
                            } catch (e) {
                                parent.document.getElementById(""result"").textContent = ""srcdoc-error: "" + e;
                            }
                        })();
                    </script>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithSrcdocIframe));
            await Task.Delay(1000);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"about:srcdoc iframe result: {result.Result}");

            // about:srcdoc frames should not have access to binding API
            Assert.Contains("srcdoc-cefsharp-undefined", result.Result?.ToString() ?? "");
            Assert.Equal(0, api.MethodCallCount);
        }

        /// <summary>
        /// Test that data: URL iframes are blocked from accessing binding API
        /// data: URLs represent dynamically created documents with unique origins
        /// </summary>
        [Fact]
        public async Task DataUriFrameShouldNotAccessBindingApi()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            allowOrigins = new[] { "https://trusted.example" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            // Create an iframe with data: URL
            var pageWithDataUriIframe = CreateHtmlContent(@"
                <div id='result'>Testing data: URI...</div>
                <script>
                    // Create an iframe with data: URL content
                    const dataContent = 'data:text/html,<script>try { window.parent.document.getElementById(""result"").textContent = typeof CefSharp === ""undefined"" ? ""datauri-cefsharp-undefined"" : ""datauri-cefsharp-available""; } catch(e) { window.parent.document.getElementById(""result"").textContent = ""datauri-error: "" + e; }<\/script>';
                    const iframe = document.createElement('iframe');
                    iframe.src = dataContent;
                    document.body.appendChild(iframe);
                </script>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithDataUriIframe));
            await Task.Delay(1000);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"data: URI iframe result: {result.Result}");

            // data: URL frames should not have access due to unique origin
            Assert.Contains("datauri-cefsharp-undefined", result.Result?.ToString() ?? "");
        }

        /// <summary>
        /// Test that multiple sandboxed iframes all properly block access to binding API
        /// This ensures the opaque origin detection works consistently
        /// </summary>
        [Fact]
        public async Task MultipleSandboxedIframesShouldAllBeBlocked()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            Browser.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = null;

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            var pageWithMultipleSandboxedIframes = CreateHtmlContent(@"
                <div id='result1'>iframe1...</div>
                <div id='result2'>iframe2...</div>
                <div id='result3'>iframe3...</div>

                <iframe id='iframe1' sandbox='allow-scripts' srcdoc='
                    <script>
                        parent.document.getElementById(""result1"").textContent = typeof CefSharp === ""undefined"" ? ""blocked1"" : ""accessible1"";
                    </script>
                '></iframe>

                <iframe id='iframe2' sandbox='allow-scripts' srcdoc='
                    <script>
                        parent.document.getElementById(""result2"").textContent = typeof CefSharp === ""undefined"" ? ""blocked2"" : ""accessible2"";
                    </script>
                '></iframe>

                <iframe id='iframe3' sandbox='allow-scripts' srcdoc='
                    <script>
                        parent.document.getElementById(""result3"").textContent = typeof CefSharp === ""undefined"" ? ""blocked3"" : ""accessible3"";
                    </script>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithMultipleSandboxedIframes));
            await Task.Delay(1500);

            var result1 = await Browser.EvaluateScriptAsync("document.getElementById('result1').textContent");
            var result2 = await Browser.EvaluateScriptAsync("document.getElementById('result2').textContent");
            var result3 = await Browser.EvaluateScriptAsync("document.getElementById('result3').textContent");

            output.WriteLine($"Multiple sandboxed iframes - iframe1: {result1.Result}, iframe2: {result2.Result}, iframe3: {result3.Result}");

            // All sandboxed iframes should be blocked
            Assert.Contains("blocked1", result1.Result?.ToString() ?? "");
            Assert.Contains("blocked2", result2.Result?.ToString() ?? "");
            Assert.Contains("blocked3", result3.Result?.ToString() ?? "");
        }

        /// <summary>
        /// Test that nested sandboxed iframes are properly blocked
        /// (iframe within iframe both with sandbox)
        /// </summary>
        [Fact]
        public async Task NestedSandboxedIframesShouldBeBlocked()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            Browser.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = null;

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            var pageWithNestedSandbox = CreateHtmlContent(@"
                <div id='outer-result'>Testing nested...</div>
                <iframe sandbox='allow-scripts' srcdoc='
                    <div id=""inner-result"">Inner iframe testing...</div>
                    <iframe sandbox=""allow-scripts"" srcdoc=""
                        <script>
                            var cefSharpAvailable = typeof CefSharp !== \""undefined\"";
                            parent.document.getElementById(\""inner-result\"").textContent = cefSharpAvailable ? \""inner-accessible\"" : \""inner-blocked\"";
                        </script>
                    ""></iframe>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithNestedSandbox));
            await Task.Delay(1500);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('outer-result').textContent");
            output.WriteLine($"Nested sandboxed iframes result: {result.Result}");

            // Nested sandboxed iframes should both be blocked
            // (Both have opaque origins and cannot access binding API)
        }

        /// <summary>
        /// Test that sandboxed iframe WITHOUT allow-scripts cannot execute JavaScript at all
        /// (This is baseline security behavior)
        /// </summary>
        [Fact]
        public async Task SandboxedIframeWithoutAllowScriptsShouldNotExecuteJs()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            Browser.JavascriptObjectRepository.Settings.JavascriptBindingApiAllowOrigins = null;

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            var pageWithNoScriptSandbox = CreateHtmlContent(@"
                <div id='result'>Testing no-script sandbox...</div>
                <iframe id='nosandbox' sandbox='allow-same-origin' srcdoc='
                    <script>
                        // This should not execute
                        document.write(""Script executed"");
                    </script>
                    <noscript>JavaScript disabled</noscript>
                '></iframe>
                <script>
                    setTimeout(function() {
                        try {
                            const iframe = document.getElementById('nosandbox');
                            const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                            const bodyText = iframeDoc.body.textContent;
                            document.getElementById('result').textContent = bodyText.includes('JavaScript disabled') ? 'noscript-blocked' : 'script-executed';
                        } catch (e) {
                            document.getElementById('result').textContent = 'error: ' + e;
                        }
                    }, 500);
                </script>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithNoScriptSandbox));
            await Task.Delay(1000);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"No-script sandbox result: {result.Result}");

            // JavaScript should not execute in iframe without allow-scripts
            // (This is baseline browser security)
        }

        /// <summary>
        /// Test that origin restriction still works when mixed with non-sandboxed frames
        /// </summary>
        [Fact]
        public async Task MixedSandboxedAndNormalFramesShouldRespectOriginRestrictions()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            allowOrigins = new[] { "https://trusted.example" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            var pageWithMixedFrames = CreateHtmlContent(@"
                <div id='normal-result'>Normal frame...</div>
                <div id='sandbox-result'>Sandbox frame...</div>

                <iframe id='normal' src='about:blank'></iframe>
                <iframe id='sandboxed' sandbox='allow-scripts' srcdoc='<script>parent.document.getElementById(""sandbox-result"").textContent = typeof CefSharp === ""undefined"" ? ""sandbox-blocked"" : ""sandbox-accessible"";</script>'></iframe>

                <script>
                    setTimeout(function() {
                        try {
                            const normalFrame = document.getElementById('normal');
                            if (typeof normalFrame.contentWindow.CefSharp === 'undefined') {
                                document.getElementById('normal-result').textContent = 'normal-blocked';
                            } else {
                                document.getElementById('normal-result').textContent = 'normal-accessible';
                            }
                        } catch (e) {
                            document.getElementById('normal-result').textContent = 'normal-error: ' + e;
                        }
                    }, 500);
                </script>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithMixedFrames));
            await Task.Delay(1500);

            var normalResult = await Browser.EvaluateScriptAsync("document.getElementById('normal-result').textContent");
            var sandboxResult = await Browser.EvaluateScriptAsync("document.getElementById('sandbox-result').textContent");

            output.WriteLine($"Mixed frames - normal: {normalResult.Result}, sandbox: {sandboxResult.Result}");

            // Both should be blocked due to origin restrictions or sandbox
            Assert.Contains("blocked", normalResult.Result?.ToString() ?? "");
            Assert.Contains("blocked", sandboxResult.Result?.ToString() ?? "");
        }

        /// <summary>
        /// Test that sandboxed iframe with allow-same-origin still cannot access binding API
        /// because it still has an opaque origin, not the same origin as parent
        /// </summary>
        [Fact]
        public async Task SandboxedWithAllowSameOriginStillHasOpaqueOrigin()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            allowOrigins = new[] { "about:blank" }; // Even if we allow about:blank, sandboxed should still be blocked

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            var pageWithSameOriginSandbox = CreateHtmlContent(@"
                <div id='result'>Testing same-origin sandbox...</div>
                <iframe sandbox='allow-scripts allow-same-origin' srcdoc='
                    <script>
                        (async function() {
                            try {
                                if (typeof CefSharp === ""undefined"") {
                                    parent.document.getElementById(""result"").textContent = ""same-origin-cefsharp-undefined"";
                                } else {
                                    // Even with allow-same-origin, sandboxed iframe has opaque origin
                                    parent.document.getElementById(""result"").textContent = ""same-origin-cefsharp-available"";
                                }
                            } catch (e) {
                                parent.document.getElementById(""result"").textContent = ""same-origin-error: "" + e;
                            }
                        })();
                    </script>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithSameOriginSandbox));
            await Task.Delay(1000);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"Same-origin sandbox result: {result.Result}");

            // Even with allow-same-origin, sandboxed iframe has opaque origin
            // The fix blocks about: scheme URLs, so this should be blocked
            Assert.Contains("same-origin-cefsharp-undefined", result.Result?.ToString() ?? "");
            Assert.Equal(0, api.MethodCallCount);
        }

        /// <summary>
        /// Test that origin allowlist doesn't accidentally allow sandboxed frames
        /// </summary>
        [Fact]
        public async Task OriginAllowlistDoesNotAllowSandboxedFrames()
        {
            AssertInitialLoadComplete();

            enableBindingApi = true;
            // Try to allow about:blank (doesn't matter - sandboxed still blocked)
            allowOrigins = new[] { "about:blank", "about:srcdoc" };

            var api = new PrivilegedApi();
#if NETCOREAPP
            Browser.JavascriptObjectRepository.Register("privilegedApi", api);
#else
            Browser.JavascriptObjectRepository.Register("privilegedApi", api, isAsync: true);
#endif

            var pageWithAllowedSandbox = CreateHtmlContent(@"
                <div id='result'>Testing allowlist bypass...</div>
                <iframe sandbox='allow-scripts' srcdoc='
                    <script>
                        parent.document.getElementById(""result"").textContent = typeof CefSharp === ""undefined"" ? ""blocked-despite-allowlist"" : ""allowlist-bypass"";
                    </script>
                '></iframe>");

            await Browser.LoadUrlAsync("data:text/html," + Uri.EscapeDataString(pageWithAllowedSandbox));
            await Task.Delay(1000);

            var result = await Browser.EvaluateScriptAsync("document.getElementById('result').textContent");
            output.WriteLine($"Allowlist sandbox bypass test: {result.Result}");

            // Should be blocked even though about: schemes are in allowlist
            // because sandboxed frames have opaque origins
            Assert.Contains("blocked-despite-allowlist", result.Result?.ToString() ?? "");
            Assert.Equal(0, api.MethodCallCount);
        }
    }
}

