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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Storage(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateClearDataForOrigin(string origin, string storageTypes);
        /// <summary>
        /// Clears storage for origin.
        /// </summary>
        /// <param name = "origin">Security origin.</param>
        /// <param name = "storageTypes">Comma separated list of StorageType to clear.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearDataForOriginAsync(string origin, string storageTypes)
        {
            ValidateClearDataForOrigin(origin, storageTypes);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            dict.Add("storageTypes", storageTypes);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.clearDataForOrigin", dict);
            return methodResult;
        }

        partial void ValidateGetCookies(string browserContextId = null);
        /// <summary>
        /// Returns all browser cookies.
        /// </summary>
        /// <param name = "browserContextId">Browser context to use when called on the browser endpoint.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCookiesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCookiesResponse> GetCookiesAsync(string browserContextId = null)
        {
            ValidateGetCookies(browserContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.getCookies", dict);
            return methodResult.DeserializeJson<GetCookiesResponse>();
        }

        partial void ValidateSetCookies(System.Collections.Generic.IList<CefSharp.DevTools.Network.CookieParam> cookies, string browserContextId = null);
        /// <summary>
        /// Sets given cookies.
        /// </summary>
        /// <param name = "cookies">Cookies to be set.</param>
        /// <param name = "browserContextId">Browser context to use when called on the browser endpoint.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCookiesAsync(System.Collections.Generic.IList<CefSharp.DevTools.Network.CookieParam> cookies, string browserContextId = null)
        {
            ValidateSetCookies(cookies, browserContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cookies", cookies.Select(x => x.ToDictionary()));
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.setCookies", dict);
            return methodResult;
        }

        partial void ValidateClearCookies(string browserContextId = null);
        /// <summary>
        /// Clears cookies.
        /// </summary>
        /// <param name = "browserContextId">Browser context to use when called on the browser endpoint.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearCookiesAsync(string browserContextId = null)
        {
            ValidateClearCookies(browserContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.clearCookies", dict);
            return methodResult;
        }

        partial void ValidateGetUsageAndQuota(string origin);
        /// <summary>
        /// Returns usage and quota in bytes.
        /// </summary>
        /// <param name = "origin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetUsageAndQuotaResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetUsageAndQuotaResponse> GetUsageAndQuotaAsync(string origin)
        {
            ValidateGetUsageAndQuota(origin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.getUsageAndQuota", dict);
            return methodResult.DeserializeJson<GetUsageAndQuotaResponse>();
        }

        partial void ValidateTrackCacheStorageForOrigin(string origin);
        /// <summary>
        /// Registers origin to be notified when an update occurs to its cache storage list.
        /// </summary>
        /// <param name = "origin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TrackCacheStorageForOriginAsync(string origin)
        {
            ValidateTrackCacheStorageForOrigin(origin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.trackCacheStorageForOrigin", dict);
            return methodResult;
        }

        partial void ValidateTrackIndexedDBForOrigin(string origin);
        /// <summary>
        /// Registers origin to be notified when an update occurs to its IndexedDB.
        /// </summary>
        /// <param name = "origin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TrackIndexedDBForOriginAsync(string origin)
        {
            ValidateTrackIndexedDBForOrigin(origin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.trackIndexedDBForOrigin", dict);
            return methodResult;
        }

        partial void ValidateUntrackCacheStorageForOrigin(string origin);
        /// <summary>
        /// Unregisters origin from receiving notifications for cache storage.
        /// </summary>
        /// <param name = "origin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UntrackCacheStorageForOriginAsync(string origin)
        {
            ValidateUntrackCacheStorageForOrigin(origin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.untrackCacheStorageForOrigin", dict);
            return methodResult;
        }

        partial void ValidateUntrackIndexedDBForOrigin(string origin);
        /// <summary>
        /// Unregisters origin from receiving notifications for IndexedDB.
        /// </summary>
        /// <param name = "origin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UntrackIndexedDBForOriginAsync(string origin)
        {
            ValidateUntrackIndexedDBForOrigin(origin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Storage.untrackIndexedDBForOrigin", dict);
            return methodResult;
        }
    }
}