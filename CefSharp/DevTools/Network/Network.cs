// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    using System.Linq;

    /// <summary>
    /// Network domain allows tracking network activities of the page. It exposes information about http,
    /// file, data and other requests and responses, their headers, bodies, timing, etc.
    /// </summary>
    public partial class Network : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Network(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Clears browser cache.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearBrowserCacheAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.clearBrowserCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears browser cookies.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearBrowserCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.clearBrowserCookies", dict);
            return methodResult;
        }

        partial void ValidateDeleteCookies(string name, string url = null, string domain = null, string path = null);
        /// <summary>
        /// Deletes browser cookies with matching name and url or domain/path pair.
        /// </summary>
        /// <param name = "name">Name of the cookies to remove.</param>
        /// <param name = "url">If specified, deletes all the cookies with the given name where domain and path match
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteCookiesAsync(string name, string url = null, string domain = null, string path = null)
        {
            ValidateDeleteCookies(name, url, domain, path);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            if (!(string.IsNullOrEmpty(url)))
            {
                dict.Add("url", url);
            }

            if (!(string.IsNullOrEmpty(domain)))
            {
                dict.Add("domain", domain);
            }

            if (!(string.IsNullOrEmpty(path)))
            {
                dict.Add("path", path);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.deleteCookies", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables network tracking, prevents network events from being sent to the client.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.disable", dict);
            return methodResult;
        }

        partial void ValidateEmulateNetworkConditions(bool offline, long latency, long downloadThroughput, long uploadThroughput, CefSharp.DevTools.Network.ConnectionType? connectionType = null);
        /// <summary>
        /// Activates emulation of network conditions.
        /// </summary>
        /// <param name = "offline">True to emulate internet disconnection.</param>
        /// <param name = "latency">Minimum latency from request sent to response headers received (ms).</param>
        /// <param name = "downloadThroughput">Maximal aggregated download throughput (bytes/sec). -1 disables download throttling.</param>
        /// <param name = "uploadThroughput">Maximal aggregated upload throughput (bytes/sec).  -1 disables upload throttling.</param>
        /// <param name = "connectionType">Connection type if known.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EmulateNetworkConditionsAsync(bool offline, long latency, long downloadThroughput, long uploadThroughput, CefSharp.DevTools.Network.ConnectionType? connectionType = null)
        {
            ValidateEmulateNetworkConditions(offline, latency, downloadThroughput, uploadThroughput, connectionType);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("offline", offline);
            dict.Add("latency", latency);
            dict.Add("downloadThroughput", downloadThroughput);
            dict.Add("uploadThroughput", uploadThroughput);
            if (connectionType.HasValue)
            {
                dict.Add("connectionType", this.EnumToString(connectionType));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.emulateNetworkConditions", dict);
            return methodResult;
        }

        partial void ValidateEnable(int? maxTotalBufferSize = null, int? maxResourceBufferSize = null, int? maxPostDataSize = null);
        /// <summary>
        /// Enables network tracking, network events will now be delivered to the client.
        /// </summary>
        /// <param name = "maxTotalBufferSize">Buffer size in bytes to use when preserving network payloads (XHRs, etc).</param>
        /// <param name = "maxResourceBufferSize">Per-resource buffer size in bytes to use when preserving network payloads (XHRs, etc).</param>
        /// <param name = "maxPostDataSize">Longest post body size (in bytes) that would be included in requestWillBeSent notification</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(int? maxTotalBufferSize = null, int? maxResourceBufferSize = null, int? maxPostDataSize = null)
        {
            ValidateEnable(maxTotalBufferSize, maxResourceBufferSize, maxPostDataSize);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (maxTotalBufferSize.HasValue)
            {
                dict.Add("maxTotalBufferSize", maxTotalBufferSize.Value);
            }

            if (maxResourceBufferSize.HasValue)
            {
                dict.Add("maxResourceBufferSize", maxResourceBufferSize.Value);
            }

            if (maxPostDataSize.HasValue)
            {
                dict.Add("maxPostDataSize", maxPostDataSize.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns all browser cookies. Depending on the backend support, will return detailed cookie
        /// information in the `cookies` field.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetAllCookiesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetAllCookiesResponse> GetAllCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getAllCookies", dict);
            return methodResult.DeserializeJson<GetAllCookiesResponse>();
        }

        partial void ValidateGetCertificate(string origin);
        /// <summary>
        /// Returns the DER-encoded certificate.
        /// </summary>
        /// <param name = "origin">Origin to get certificate for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCertificateResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCertificateResponse> GetCertificateAsync(string origin)
        {
            ValidateGetCertificate(origin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getCertificate", dict);
            return methodResult.DeserializeJson<GetCertificateResponse>();
        }

        partial void ValidateGetCookies(string[] urls = null);
        /// <summary>
        /// Returns all browser cookies for the current URL. Depending on the backend support, will return
        /// detailed cookie information in the `cookies` field.
        /// </summary>
        /// <param name = "urls">The list of URLs for which applicable cookies will be fetched</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCookiesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCookiesResponse> GetCookiesAsync(string[] urls = null)
        {
            ValidateGetCookies(urls);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((urls) != (null))
            {
                dict.Add("urls", urls);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getCookies", dict);
            return methodResult.DeserializeJson<GetCookiesResponse>();
        }

        partial void ValidateGetResponseBody(string requestId);
        /// <summary>
        /// Returns content served for the given request.
        /// </summary>
        /// <param name = "requestId">Identifier of the network request to get content for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetResponseBodyResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetResponseBodyResponse> GetResponseBodyAsync(string requestId)
        {
            ValidateGetResponseBody(requestId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getResponseBody", dict);
            return methodResult.DeserializeJson<GetResponseBodyResponse>();
        }

        partial void ValidateGetRequestPostData(string requestId);
        /// <summary>
        /// Returns post data sent with the request. Returns an error when no data was sent with the request.
        /// </summary>
        /// <param name = "requestId">Identifier of the network request to get content for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetRequestPostDataResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetRequestPostDataResponse> GetRequestPostDataAsync(string requestId)
        {
            ValidateGetRequestPostData(requestId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getRequestPostData", dict);
            return methodResult.DeserializeJson<GetRequestPostDataResponse>();
        }

        partial void ValidateGetResponseBodyForInterception(string interceptionId);
        /// <summary>
        /// Returns content served for the given currently intercepted request.
        /// </summary>
        /// <param name = "interceptionId">Identifier for the intercepted request to get body for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetResponseBodyForInterceptionResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetResponseBodyForInterceptionResponse> GetResponseBodyForInterceptionAsync(string interceptionId)
        {
            ValidateGetResponseBodyForInterception(interceptionId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("interceptionId", interceptionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getResponseBodyForInterception", dict);
            return methodResult.DeserializeJson<GetResponseBodyForInterceptionResponse>();
        }

        partial void ValidateTakeResponseBodyForInterceptionAsStream(string interceptionId);
        /// <summary>
        /// Returns a handle to the stream representing the response body. Note that after this command,
        /// the intercepted request can't be continued as is -- you either need to cancel it or to provide
        /// the response body. The stream only supports sequential read, IO.read will fail if the position
        /// is specified.
        /// </summary>
        /// <param name = "interceptionId">interceptionId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;TakeResponseBodyForInterceptionAsStreamResponse&gt;</returns>
        public async System.Threading.Tasks.Task<TakeResponseBodyForInterceptionAsStreamResponse> TakeResponseBodyForInterceptionAsStreamAsync(string interceptionId)
        {
            ValidateTakeResponseBodyForInterceptionAsStream(interceptionId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("interceptionId", interceptionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.takeResponseBodyForInterceptionAsStream", dict);
            return methodResult.DeserializeJson<TakeResponseBodyForInterceptionAsStreamResponse>();
        }

        partial void ValidateReplayXHR(string requestId);
        /// <summary>
        /// This method sends a new XMLHttpRequest which is identical to the original one. The following
        /// parameters should be identical: method, url, async, request body, extra headers, withCredentials
        /// attribute, user, password.
        /// </summary>
        /// <param name = "requestId">Identifier of XHR to replay.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReplayXHRAsync(string requestId)
        {
            ValidateReplayXHR(requestId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.replayXHR", dict);
            return methodResult;
        }

        partial void ValidateSearchInResponseBody(string requestId, string query, bool? caseSensitive = null, bool? isRegex = null);
        /// <summary>
        /// Searches for given string in response content.
        /// </summary>
        /// <param name = "requestId">Identifier of the network response to search.</param>
        /// <param name = "query">String to search for.</param>
        /// <param name = "caseSensitive">If true, search is case sensitive.</param>
        /// <param name = "isRegex">If true, treats string parameter as regex.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SearchInResponseBodyResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SearchInResponseBodyResponse> SearchInResponseBodyAsync(string requestId, string query, bool? caseSensitive = null, bool? isRegex = null)
        {
            ValidateSearchInResponseBody(requestId, query, caseSensitive, isRegex);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            dict.Add("query", query);
            if (caseSensitive.HasValue)
            {
                dict.Add("caseSensitive", caseSensitive.Value);
            }

            if (isRegex.HasValue)
            {
                dict.Add("isRegex", isRegex.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.searchInResponseBody", dict);
            return methodResult.DeserializeJson<SearchInResponseBodyResponse>();
        }

        partial void ValidateSetBlockedURLs(string[] urls);
        /// <summary>
        /// Blocks URLs from loading.
        /// </summary>
        /// <param name = "urls">URL patterns to block. Wildcards ('*') are allowed.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBlockedURLsAsync(string[] urls)
        {
            ValidateSetBlockedURLs(urls);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("urls", urls);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setBlockedURLs", dict);
            return methodResult;
        }

        partial void ValidateSetBypassServiceWorker(bool bypass);
        /// <summary>
        /// Toggles ignoring of service worker for each request.
        /// </summary>
        /// <param name = "bypass">Bypass service worker and load from network.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBypassServiceWorkerAsync(bool bypass)
        {
            ValidateSetBypassServiceWorker(bypass);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("bypass", bypass);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setBypassServiceWorker", dict);
            return methodResult;
        }

        partial void ValidateSetCacheDisabled(bool cacheDisabled);
        /// <summary>
        /// Toggles ignoring cache for each request. If `true`, cache will not be used.
        /// </summary>
        /// <param name = "cacheDisabled">Cache disabled state.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCacheDisabledAsync(bool cacheDisabled)
        {
            ValidateSetCacheDisabled(cacheDisabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheDisabled", cacheDisabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setCacheDisabled", dict);
            return methodResult;
        }

        partial void ValidateSetCookie(string name, string value, string url = null, string domain = null, string path = null, bool? secure = null, bool? httpOnly = null, CefSharp.DevTools.Network.CookieSameSite? sameSite = null, long? expires = null, CefSharp.DevTools.Network.CookiePriority? priority = null);
        /// <summary>
        /// Sets a cookie with the given cookie data; may overwrite equivalent cookies if they exist.
        /// </summary>
        /// <param name = "name">Cookie name.</param>
        /// <param name = "value">Cookie value.</param>
        /// <param name = "url">The request-URI to associate with the setting of the cookie. This value can affect the
        public async System.Threading.Tasks.Task<SetCookieResponse> SetCookieAsync(string name, string value, string url = null, string domain = null, string path = null, bool? secure = null, bool? httpOnly = null, CefSharp.DevTools.Network.CookieSameSite? sameSite = null, long? expires = null, CefSharp.DevTools.Network.CookiePriority? priority = null)
        {
            ValidateSetCookie(name, value, url, domain, path, secure, httpOnly, sameSite, expires, priority);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            dict.Add("value", value);
            if (!(string.IsNullOrEmpty(url)))
            {
                dict.Add("url", url);
            }

            if (!(string.IsNullOrEmpty(domain)))
            {
                dict.Add("domain", domain);
            }

            if (!(string.IsNullOrEmpty(path)))
            {
                dict.Add("path", path);
            }

            if (secure.HasValue)
            {
                dict.Add("secure", secure.Value);
            }

            if (httpOnly.HasValue)
            {
                dict.Add("httpOnly", httpOnly.Value);
            }

            if (sameSite.HasValue)
            {
                dict.Add("sameSite", this.EnumToString(sameSite));
            }

            if (expires.HasValue)
            {
                dict.Add("expires", expires.Value);
            }

            if (priority.HasValue)
            {
                dict.Add("priority", this.EnumToString(priority));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setCookie", dict);
            return methodResult.DeserializeJson<SetCookieResponse>();
        }

        partial void ValidateSetCookies(System.Collections.Generic.IList<CefSharp.DevTools.Network.CookieParam> cookies);
        /// <summary>
        /// Sets given cookies.
        /// </summary>
        /// <param name = "cookies">Cookies to be set.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCookiesAsync(System.Collections.Generic.IList<CefSharp.DevTools.Network.CookieParam> cookies)
        {
            ValidateSetCookies(cookies);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cookies", cookies.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setCookies", dict);
            return methodResult;
        }

        partial void ValidateSetDataSizeLimitsForTest(int maxTotalSize, int maxResourceSize);
        /// <summary>
        /// For testing.
        /// </summary>
        /// <param name = "maxTotalSize">Maximum total buffer size.</param>
        /// <param name = "maxResourceSize">Maximum per-resource size.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDataSizeLimitsForTestAsync(int maxTotalSize, int maxResourceSize)
        {
            ValidateSetDataSizeLimitsForTest(maxTotalSize, maxResourceSize);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxTotalSize", maxTotalSize);
            dict.Add("maxResourceSize", maxResourceSize);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setDataSizeLimitsForTest", dict);
            return methodResult;
        }

        partial void ValidateSetExtraHTTPHeaders(CefSharp.DevTools.Network.Headers headers);
        /// <summary>
        /// Specifies whether to always send extra HTTP headers with the requests from this page.
        /// </summary>
        /// <param name = "headers">Map with extra HTTP headers.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetExtraHTTPHeadersAsync(CefSharp.DevTools.Network.Headers headers)
        {
            ValidateSetExtraHTTPHeaders(headers);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("headers", headers.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setExtraHTTPHeaders", dict);
            return methodResult;
        }

        partial void ValidateSetUserAgentOverride(string userAgent, string acceptLanguage = null, string platform = null, CefSharp.DevTools.Emulation.UserAgentMetadata userAgentMetadata = null);
        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        /// <param name = "userAgent">User agent to use.</param>
        /// <param name = "acceptLanguage">Browser langugage to emulate.</param>
        /// <param name = "platform">The platform navigator.platform should return.</param>
        /// <param name = "userAgentMetadata">To be sent in Sec-CH-UA-* headers and returned in navigator.userAgentData</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, CefSharp.DevTools.Emulation.UserAgentMetadata userAgentMetadata = null)
        {
            ValidateSetUserAgentOverride(userAgent, acceptLanguage, platform, userAgentMetadata);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("userAgent", userAgent);
            if (!(string.IsNullOrEmpty(acceptLanguage)))
            {
                dict.Add("acceptLanguage", acceptLanguage);
            }

            if (!(string.IsNullOrEmpty(platform)))
            {
                dict.Add("platform", platform);
            }

            if ((userAgentMetadata) != (null))
            {
                dict.Add("userAgentMetadata", userAgentMetadata.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setUserAgentOverride", dict);
            return methodResult;
        }
    }
}