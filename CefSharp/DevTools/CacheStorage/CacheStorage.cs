// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    using System.Linq;

    /// <summary>
    /// CacheStorage
    /// </summary>
    public partial class CacheStorage : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public CacheStorage(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateDeleteCache(string cacheId);
        /// <summary>
        /// Deletes a cache.
        /// </summary>
        /// <param name = "cacheId">Id of cache for deletion.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteCacheAsync(string cacheId)
        {
            ValidateDeleteCache(cacheId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.deleteCache", dict);
            return methodResult;
        }

        partial void ValidateDeleteEntry(string cacheId, string request);
        /// <summary>
        /// Deletes a cache entry.
        /// </summary>
        /// <param name = "cacheId">Id of cache where the entry will be deleted.</param>
        /// <param name = "request">URL spec of the request.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteEntryAsync(string cacheId, string request)
        {
            ValidateDeleteEntry(cacheId, request);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            dict.Add("request", request);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.deleteEntry", dict);
            return methodResult;
        }

        partial void ValidateRequestCacheNames(string securityOrigin);
        /// <summary>
        /// Requests cache names.
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestCacheNamesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestCacheNamesResponse> RequestCacheNamesAsync(string securityOrigin)
        {
            ValidateRequestCacheNames(securityOrigin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.requestCacheNames", dict);
            return methodResult.DeserializeJson<RequestCacheNamesResponse>();
        }

        partial void ValidateRequestCachedResponse(string cacheId, string requestURL, System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Header> requestHeaders);
        /// <summary>
        /// Fetches cache entry.
        /// </summary>
        /// <param name = "cacheId">Id of cache that contains the entry.</param>
        /// <param name = "requestURL">URL spec of the request.</param>
        /// <param name = "requestHeaders">headers of the request.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestCachedResponseResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestCachedResponseResponse> RequestCachedResponseAsync(string cacheId, string requestURL, System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Header> requestHeaders)
        {
            ValidateRequestCachedResponse(cacheId, requestURL, requestHeaders);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            dict.Add("requestURL", requestURL);
            dict.Add("requestHeaders", requestHeaders.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.requestCachedResponse", dict);
            return methodResult.DeserializeJson<RequestCachedResponseResponse>();
        }

        partial void ValidateRequestEntries(string cacheId, int? skipCount = null, int? pageSize = null, string pathFilter = null);
        /// <summary>
        /// Requests data from cache.
        /// </summary>
        /// <param name = "cacheId">ID of cache to get entries from.</param>
        /// <param name = "skipCount">Number of records to skip.</param>
        /// <param name = "pageSize">Number of records to fetch.</param>
        /// <param name = "pathFilter">If present, only return the entries containing this substring in the path</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestEntriesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestEntriesResponse> RequestEntriesAsync(string cacheId, int? skipCount = null, int? pageSize = null, string pathFilter = null)
        {
            ValidateRequestEntries(cacheId, skipCount, pageSize, pathFilter);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            if (skipCount.HasValue)
            {
                dict.Add("skipCount", skipCount.Value);
            }

            if (pageSize.HasValue)
            {
                dict.Add("pageSize", pageSize.Value);
            }

            if (!(string.IsNullOrEmpty(pathFilter)))
            {
                dict.Add("pathFilter", pathFilter);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.requestEntries", dict);
            return methodResult.DeserializeJson<RequestEntriesResponse>();
        }
    }
}