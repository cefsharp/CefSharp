// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMStorage
{
    using System.Linq;

    /// <summary>
    /// Query and modify DOM storage.
    /// </summary>
    public partial class DOMStorage : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public DOMStorage(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateClear(CefSharp.DevTools.DOMStorage.StorageId storageId);
        /// <summary>
        /// Clear
        /// </summary>
        /// <param name = "storageId">storageId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearAsync(CefSharp.DevTools.DOMStorage.StorageId storageId)
        {
            ValidateClear(storageId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.clear", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables storage tracking, prevents storage events from being sent to the client.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables storage tracking, storage events will now be delivered to the client.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.enable", dict);
            return methodResult;
        }

        partial void ValidateGetDOMStorageItems(CefSharp.DevTools.DOMStorage.StorageId storageId);
        /// <summary>
        /// GetDOMStorageItems
        /// </summary>
        /// <param name = "storageId">storageId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetDOMStorageItemsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetDOMStorageItemsResponse> GetDOMStorageItemsAsync(CefSharp.DevTools.DOMStorage.StorageId storageId)
        {
            ValidateGetDOMStorageItems(storageId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.getDOMStorageItems", dict);
            return methodResult.DeserializeJson<GetDOMStorageItemsResponse>();
        }

        partial void ValidateRemoveDOMStorageItem(CefSharp.DevTools.DOMStorage.StorageId storageId, string key);
        /// <summary>
        /// RemoveDOMStorageItem
        /// </summary>
        /// <param name = "storageId">storageId</param>
        /// <param name = "key">key</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveDOMStorageItemAsync(CefSharp.DevTools.DOMStorage.StorageId storageId, string key)
        {
            ValidateRemoveDOMStorageItem(storageId, key);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            dict.Add("key", key);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.removeDOMStorageItem", dict);
            return methodResult;
        }

        partial void ValidateSetDOMStorageItem(CefSharp.DevTools.DOMStorage.StorageId storageId, string key, string value);
        /// <summary>
        /// SetDOMStorageItem
        /// </summary>
        /// <param name = "storageId">storageId</param>
        /// <param name = "key">key</param>
        /// <param name = "value">value</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDOMStorageItemAsync(CefSharp.DevTools.DOMStorage.StorageId storageId, string key, string value)
        {
            ValidateSetDOMStorageItem(storageId, key, value);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            dict.Add("key", key);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.setDOMStorageItem", dict);
            return methodResult;
        }
    }
}