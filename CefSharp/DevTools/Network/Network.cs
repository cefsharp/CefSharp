// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    using System.Linq;

    /// <summary>
    /// Network domain allows tracking network activities of the page. It exposes information about http,
    public partial class Network : DevToolsDomainBase
    {
        public Network(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Clears browser cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearBrowserCacheAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.clearBrowserCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears browser cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearBrowserCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.clearBrowserCookies", dict);
            return methodResult;
        }

        /// <summary>
        /// Deletes browser cookies with matching name and url or domain/path pair.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteCookiesAsync(string name, string url = null, string domain = null, string path = null)
        {
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
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Activates emulation of network conditions.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EmulateNetworkConditionsAsync(bool offline, long latency, long downloadThroughput, long uploadThroughput, CefSharp.DevTools.Network.ConnectionType? connectionType = null)
        {
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

        /// <summary>
        /// Enables network tracking, network events will now be delivered to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(int? maxTotalBufferSize = null, int? maxResourceBufferSize = null, int? maxPostDataSize = null)
        {
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
        public async System.Threading.Tasks.Task<GetAllCookiesResponse> GetAllCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getAllCookies", dict);
            return methodResult.DeserializeJson<GetAllCookiesResponse>();
        }

        /// <summary>
        /// Returns the DER-encoded certificate.
        /// </summary>
        public async System.Threading.Tasks.Task<GetCertificateResponse> GetCertificateAsync(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getCertificate", dict);
            return methodResult.DeserializeJson<GetCertificateResponse>();
        }

        /// <summary>
        /// Returns all browser cookies for the current URL. Depending on the backend support, will return
        public async System.Threading.Tasks.Task<GetCookiesResponse> GetCookiesAsync(string[] urls = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((urls) != (null))
            {
                dict.Add("urls", urls);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getCookies", dict);
            return methodResult.DeserializeJson<GetCookiesResponse>();
        }

        /// <summary>
        /// Returns content served for the given request.
        /// </summary>
        public async System.Threading.Tasks.Task<GetResponseBodyResponse> GetResponseBodyAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getResponseBody", dict);
            return methodResult.DeserializeJson<GetResponseBodyResponse>();
        }

        /// <summary>
        /// Returns post data sent with the request. Returns an error when no data was sent with the request.
        /// </summary>
        public async System.Threading.Tasks.Task<GetRequestPostDataResponse> GetRequestPostDataAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getRequestPostData", dict);
            return methodResult.DeserializeJson<GetRequestPostDataResponse>();
        }

        /// <summary>
        /// Returns content served for the given currently intercepted request.
        /// </summary>
        public async System.Threading.Tasks.Task<GetResponseBodyForInterceptionResponse> GetResponseBodyForInterceptionAsync(string interceptionId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("interceptionId", interceptionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.getResponseBodyForInterception", dict);
            return methodResult.DeserializeJson<GetResponseBodyForInterceptionResponse>();
        }

        /// <summary>
        /// Returns a handle to the stream representing the response body. Note that after this command,
        public async System.Threading.Tasks.Task<TakeResponseBodyForInterceptionAsStreamResponse> TakeResponseBodyForInterceptionAsStreamAsync(string interceptionId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("interceptionId", interceptionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.takeResponseBodyForInterceptionAsStream", dict);
            return methodResult.DeserializeJson<TakeResponseBodyForInterceptionAsStreamResponse>();
        }

        /// <summary>
        /// This method sends a new XMLHttpRequest which is identical to the original one. The following
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReplayXHRAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.replayXHR", dict);
            return methodResult;
        }

        /// <summary>
        /// Searches for given string in response content.
        /// </summary>
        public async System.Threading.Tasks.Task<SearchInResponseBodyResponse> SearchInResponseBodyAsync(string requestId, string query, bool? caseSensitive = null, bool? isRegex = null)
        {
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

        /// <summary>
        /// Blocks URLs from loading.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBlockedURLsAsync(string[] urls)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("urls", urls);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setBlockedURLs", dict);
            return methodResult;
        }

        /// <summary>
        /// Toggles ignoring of service worker for each request.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBypassServiceWorkerAsync(bool bypass)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("bypass", bypass);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setBypassServiceWorker", dict);
            return methodResult;
        }

        /// <summary>
        /// Toggles ignoring cache for each request. If `true`, cache will not be used.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCacheDisabledAsync(bool cacheDisabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheDisabled", cacheDisabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setCacheDisabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets a cookie with the given cookie data; may overwrite equivalent cookies if they exist.
        /// </summary>
        public async System.Threading.Tasks.Task<SetCookieResponse> SetCookieAsync(string name, string value, string url = null, string domain = null, string path = null, bool? secure = null, bool? httpOnly = null, CefSharp.DevTools.Network.CookieSameSite? sameSite = null, long? expires = null, CefSharp.DevTools.Network.CookiePriority? priority = null)
        {
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

        /// <summary>
        /// Sets given cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCookiesAsync(System.Collections.Generic.IList<CefSharp.DevTools.Network.CookieParam> cookies)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cookies", cookies.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setCookies", dict);
            return methodResult;
        }

        /// <summary>
        /// For testing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDataSizeLimitsForTestAsync(int maxTotalSize, int maxResourceSize)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxTotalSize", maxTotalSize);
            dict.Add("maxResourceSize", maxResourceSize);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setDataSizeLimitsForTest", dict);
            return methodResult;
        }

        /// <summary>
        /// Specifies whether to always send extra HTTP headers with the requests from this page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetExtraHTTPHeadersAsync(CefSharp.DevTools.Network.Headers headers)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("headers", headers.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Network.setExtraHTTPHeaders", dict);
            return methodResult;
        }

        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, CefSharp.DevTools.Emulation.UserAgentMetadata userAgentMetadata = null)
        {
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