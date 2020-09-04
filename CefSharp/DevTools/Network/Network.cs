// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
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
        /// Tells whether clearing browser cache is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<CanClearBrowserCacheResponse> CanClearBrowserCacheAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.canClearBrowserCache", dict);
            return result.DeserializeJson<CanClearBrowserCacheResponse>();
        }

        /// <summary>
        /// Tells whether clearing browser cookies is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<CanClearBrowserCookiesResponse> CanClearBrowserCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.canClearBrowserCookies", dict);
            return result.DeserializeJson<CanClearBrowserCookiesResponse>();
        }

        /// <summary>
        /// Tells whether emulation of network conditions is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<CanEmulateNetworkConditionsResponse> CanEmulateNetworkConditionsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.canEmulateNetworkConditions", dict);
            return result.DeserializeJson<CanEmulateNetworkConditionsResponse>();
        }

        /// <summary>
        /// Clears browser cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearBrowserCacheAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.clearBrowserCache", dict);
            return result;
        }

        /// <summary>
        /// Clears browser cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearBrowserCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.clearBrowserCookies", dict);
            return result;
        }

        /// <summary>
        /// Deletes browser cookies with matching name and url or domain/path pair.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DeleteCookiesAsync(string name, string url = null, string domain = null, string path = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("Network.deleteCookies", dict);
            return result;
        }

        /// <summary>
        /// Disables network tracking, prevents network events from being sent to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.disable", dict);
            return result;
        }

        /// <summary>
        /// Activates emulation of network conditions.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EmulateNetworkConditionsAsync(bool offline, long latency, long downloadThroughput, long uploadThroughput, string connectionType = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("offline", offline);
            dict.Add("latency", latency);
            dict.Add("downloadThroughput", downloadThroughput);
            dict.Add("uploadThroughput", uploadThroughput);
            if (!(string.IsNullOrEmpty(connectionType)))
            {
                dict.Add("connectionType", connectionType);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Network.emulateNetworkConditions", dict);
            return result;
        }

        /// <summary>
        /// Enables network tracking, network events will now be delivered to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync(int? maxTotalBufferSize = null, int? maxResourceBufferSize = null, int? maxPostDataSize = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("Network.enable", dict);
            return result;
        }

        /// <summary>
        /// Returns all browser cookies. Depending on the backend support, will return detailed cookie
        public async System.Threading.Tasks.Task<GetAllCookiesResponse> GetAllCookiesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Network.getAllCookies", dict);
            return result.DeserializeJson<GetAllCookiesResponse>();
        }

        /// <summary>
        /// Returns all browser cookies for the current URL. Depending on the backend support, will return
        public async System.Threading.Tasks.Task<GetCookiesResponse> GetCookiesAsync(string urls = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(urls)))
            {
                dict.Add("urls", urls);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Network.getCookies", dict);
            return result.DeserializeJson<GetCookiesResponse>();
        }

        /// <summary>
        /// Returns content served for the given request.
        /// </summary>
        public async System.Threading.Tasks.Task<GetResponseBodyResponse> GetResponseBodyAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var result = await _client.ExecuteDevToolsMethodAsync("Network.getResponseBody", dict);
            return result.DeserializeJson<GetResponseBodyResponse>();
        }

        /// <summary>
        /// Returns post data sent with the request. Returns an error when no data was sent with the request.
        /// </summary>
        public async System.Threading.Tasks.Task<GetRequestPostDataResponse> GetRequestPostDataAsync(string requestId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("requestId", requestId);
            var result = await _client.ExecuteDevToolsMethodAsync("Network.getRequestPostData", dict);
            return result.DeserializeJson<GetRequestPostDataResponse>();
        }

        /// <summary>
        /// Toggles ignoring cache for each request. If `true`, cache will not be used.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCacheDisabledAsync(bool cacheDisabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheDisabled", cacheDisabled);
            var result = await _client.ExecuteDevToolsMethodAsync("Network.setCacheDisabled", dict);
            return result;
        }

        /// <summary>
        /// Sets a cookie with the given cookie data; may overwrite equivalent cookies if they exist.
        /// </summary>
        public async System.Threading.Tasks.Task<SetCookieResponse> SetCookieAsync(string name, string value, string url = null, string domain = null, string path = null, bool? secure = null, bool? httpOnly = null, string sameSite = null, long? expires = null, string priority = null)
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

            if (!(string.IsNullOrEmpty(sameSite)))
            {
                dict.Add("sameSite", sameSite);
            }

            if (expires.HasValue)
            {
                dict.Add("expires", expires.Value);
            }

            if (!(string.IsNullOrEmpty(priority)))
            {
                dict.Add("priority", priority);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Network.setCookie", dict);
            return result.DeserializeJson<SetCookieResponse>();
        }

        /// <summary>
        /// Sets given cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCookiesAsync(System.Collections.Generic.IList<CookieParam> cookies)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cookies", cookies);
            var result = await _client.ExecuteDevToolsMethodAsync("Network.setCookies", dict);
            return result;
        }

        /// <summary>
        /// Specifies whether to always send extra HTTP headers with the requests from this page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetExtraHTTPHeadersAsync(Headers headers)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("headers", headers);
            var result = await _client.ExecuteDevToolsMethodAsync("Network.setExtraHTTPHeaders", dict);
            return result;
        }

        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, Emulation.UserAgentMetadata userAgentMetadata = null)
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
                dict.Add("userAgentMetadata", userAgentMetadata);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Network.setUserAgentOverride", dict);
            return result;
        }
    }
}