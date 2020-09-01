// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Network domain allows tracking network activities of the page. It exposes information about http,
    public partial class Network
    {
        public Network(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Tells whether clearing browser cache is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CanClearBrowserCache()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.CanClearBrowserCache", dict);
            return result;
        }

        /// <summary>
        /// Tells whether clearing browser cookies is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CanClearBrowserCookies()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.CanClearBrowserCookies", dict);
            return result;
        }

        /// <summary>
        /// Tells whether emulation of network conditions is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CanEmulateNetworkConditions()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.CanEmulateNetworkConditions", dict);
            return result;
        }

        /// <summary>
        /// Clears browser cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearBrowserCache()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.ClearBrowserCache", dict);
            return result;
        }

        /// <summary>
        /// Clears browser cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearBrowserCookies()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.ClearBrowserCookies", dict);
            return result;
        }

        /// <summary>
        /// Response to Network.requestIntercepted which either modifies the request to continue with any
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ContinueInterceptedRequest(string interceptionId, string errorReason, string rawResponse, string url, string method, string postData, Headers headers, AuthChallengeResponse authChallengeResponse)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"interceptionId", interceptionId}, {"errorReason", errorReason}, {"rawResponse", rawResponse}, {"url", url}, {"method", method}, {"postData", postData}, {"headers", headers}, {"authChallengeResponse", authChallengeResponse}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.ContinueInterceptedRequest", dict);
            return result;
        }

        /// <summary>
        /// Deletes browser cookies with matching name and url or domain/path pair.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DeleteCookies(string name, string url, string domain, string path)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"name", name}, {"url", url}, {"domain", domain}, {"path", path}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.DeleteCookies", dict);
            return result;
        }

        /// <summary>
        /// Disables network tracking, prevents network events from being sent to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.Disable", dict);
            return result;
        }

        /// <summary>
        /// Activates emulation of network conditions.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EmulateNetworkConditions(bool offline, long latency, long downloadThroughput, long uploadThroughput, string connectionType)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"offline", offline}, {"latency", latency}, {"downloadThroughput", downloadThroughput}, {"uploadThroughput", uploadThroughput}, {"connectionType", connectionType}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.EmulateNetworkConditions", dict);
            return result;
        }

        /// <summary>
        /// Enables network tracking, network events will now be delivered to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable(int maxTotalBufferSize, int maxResourceBufferSize, int maxPostDataSize)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"maxTotalBufferSize", maxTotalBufferSize}, {"maxResourceBufferSize", maxResourceBufferSize}, {"maxPostDataSize", maxPostDataSize}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.Enable", dict);
            return result;
        }

        /// <summary>
        /// Returns all browser cookies. Depending on the backend support, will return detailed cookie
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetAllCookies()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetAllCookies", dict);
            return result;
        }

        /// <summary>
        /// Returns the DER-encoded certificate.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetCertificate(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"origin", origin}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetCertificate", dict);
            return result;
        }

        /// <summary>
        /// Returns all browser cookies for the current URL. Depending on the backend support, will return
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetCookies(string urls)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"urls", urls}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetCookies", dict);
            return result;
        }

        /// <summary>
        /// Returns content served for the given request.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetResponseBody(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"requestId", requestId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetResponseBody", dict);
            return result;
        }

        /// <summary>
        /// Returns post data sent with the request. Returns an error when no data was sent with the request.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetRequestPostData(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"requestId", requestId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetRequestPostData", dict);
            return result;
        }

        /// <summary>
        /// Returns content served for the given currently intercepted request.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetResponseBodyForInterception(string interceptionId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"interceptionId", interceptionId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetResponseBodyForInterception", dict);
            return result;
        }

        /// <summary>
        /// Returns a handle to the stream representing the response body. Note that after this command,
        public async System.Threading.Tasks.Task<DevToolsMethodResult> TakeResponseBodyForInterceptionAsStream(string interceptionId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"interceptionId", interceptionId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.TakeResponseBodyForInterceptionAsStream", dict);
            return result;
        }

        /// <summary>
        /// This method sends a new XMLHttpRequest which is identical to the original one. The following
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ReplayXHR(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"requestId", requestId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.ReplayXHR", dict);
            return result;
        }

        /// <summary>
        /// Searches for given string in response content.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SearchInResponseBody(string requestId, string query, bool caseSensitive, bool isRegex)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"requestId", requestId}, {"query", query}, {"caseSensitive", caseSensitive}, {"isRegex", isRegex}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SearchInResponseBody", dict);
            return result;
        }

        /// <summary>
        /// Blocks URLs from loading.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBlockedURLs(string urls)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"urls", urls}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetBlockedURLs", dict);
            return result;
        }

        /// <summary>
        /// Toggles ignoring of service worker for each request.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBypassServiceWorker(bool bypass)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"bypass", bypass}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetBypassServiceWorker", dict);
            return result;
        }

        /// <summary>
        /// Toggles ignoring cache for each request. If `true`, cache will not be used.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCacheDisabled(bool cacheDisabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"cacheDisabled", cacheDisabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetCacheDisabled", dict);
            return result;
        }

        /// <summary>
        /// Sets a cookie with the given cookie data; may overwrite equivalent cookies if they exist.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCookie(string name, string value, string url, string domain, string path, bool secure, bool httpOnly, string sameSite, long expires, string priority)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"name", name}, {"value", value}, {"url", url}, {"domain", domain}, {"path", path}, {"secure", secure}, {"httpOnly", httpOnly}, {"sameSite", sameSite}, {"expires", expires}, {"priority", priority}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetCookie", dict);
            return result;
        }

        /// <summary>
        /// Sets given cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCookies(System.Collections.Generic.IList<CookieParam> cookies)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"cookies", cookies}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetCookies", dict);
            return result;
        }

        /// <summary>
        /// For testing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDataSizeLimitsForTest(int maxTotalSize, int maxResourceSize)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"maxTotalSize", maxTotalSize}, {"maxResourceSize", maxResourceSize}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetDataSizeLimitsForTest", dict);
            return result;
        }

        /// <summary>
        /// Specifies whether to always send extra HTTP headers with the requests from this page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetExtraHTTPHeaders(Headers headers)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"headers", headers}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetExtraHTTPHeaders", dict);
            return result;
        }

        /// <summary>
        /// Specifies whether to sned a debug header to all outgoing requests.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAttachDebugHeader(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetAttachDebugHeader", dict);
            return result;
        }

        /// <summary>
        /// Sets the requests to intercept that match the provided patterns and optionally resource types.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetRequestInterception(System.Collections.Generic.IList<RequestPattern> patterns)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"patterns", patterns}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetRequestInterception", dict);
            return result;
        }

        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetUserAgentOverride(string userAgent, string acceptLanguage, string platform, Emulation.UserAgentMetadata userAgentMetadata)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"userAgent", userAgent}, {"acceptLanguage", acceptLanguage}, {"platform", platform}, {"userAgentMetadata", userAgentMetadata}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.SetUserAgentOverride", dict);
            return result;
        }

        /// <summary>
        /// Returns information about the COEP/COOP isolation status.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetSecurityIsolationStatus(string frameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"frameId", frameId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Network.GetSecurityIsolationStatus", dict);
            return result;
        }
    }
}