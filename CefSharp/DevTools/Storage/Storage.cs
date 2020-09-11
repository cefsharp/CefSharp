// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Storage
{
    using System.Linq;

    /// <summary>
    /// Storage
    /// </summary>
    public partial class Storage : DevToolsDomainBase
    {
        public Storage(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Clears storage for origin.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearDataForOriginAsync(string origin, string storageTypes)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            dict.Add("storageTypes", storageTypes);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.clearDataForOrigin", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns all browser cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<GetCookiesResponse> GetCookiesAsync(string browserContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.getCookies", dict);
            return methodResult.DeserializeJson<GetCookiesResponse>();
        }

        /// <summary>
        /// Sets given cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCookiesAsync(System.Collections.Generic.IList<CefSharp.DevTools.Network.CookieParam> cookies, string browserContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cookies", cookies.Select(x => x.ToDictionary()));
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.setCookies", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears cookies.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearCookiesAsync(string browserContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.clearCookies", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns usage and quota in bytes.
        /// </summary>
        public async System.Threading.Tasks.Task<GetUsageAndQuotaResponse> GetUsageAndQuotaAsync(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.getUsageAndQuota", dict);
            return methodResult.DeserializeJson<GetUsageAndQuotaResponse>();
        }

        /// <summary>
        /// Registers origin to be notified when an update occurs to its cache storage list.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TrackCacheStorageForOriginAsync(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.trackCacheStorageForOrigin", dict);
            return methodResult;
        }

        /// <summary>
        /// Registers origin to be notified when an update occurs to its IndexedDB.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TrackIndexedDBForOriginAsync(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.trackIndexedDBForOrigin", dict);
            return methodResult;
        }

        /// <summary>
        /// Unregisters origin from receiving notifications for cache storage.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UntrackCacheStorageForOriginAsync(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.untrackCacheStorageForOrigin", dict);
            return methodResult;
        }

        /// <summary>
        /// Unregisters origin from receiving notifications for IndexedDB.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UntrackIndexedDBForOriginAsync(string origin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.untrackIndexedDBForOrigin", dict);
            return methodResult;
        }
    }
}