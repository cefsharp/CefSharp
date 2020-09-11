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
        public DOMStorage(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearAsync(CefSharp.DevTools.DOMStorage.StorageId storageId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.clear", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables storage tracking, prevents storage events from being sent to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables storage tracking, storage events will now be delivered to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetDOMStorageItemsResponse> GetDOMStorageItemsAsync(CefSharp.DevTools.DOMStorage.StorageId storageId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.getDOMStorageItems", dict);
            return methodResult.DeserializeJson<GetDOMStorageItemsResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveDOMStorageItemAsync(CefSharp.DevTools.DOMStorage.StorageId storageId, string key)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            dict.Add("key", key);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.removeDOMStorageItem", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDOMStorageItemAsync(CefSharp.DevTools.DOMStorage.StorageId storageId, string key, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("storageId", storageId.ToDictionary());
            dict.Add("key", key);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMStorage.setDOMStorageItem", dict);
            return methodResult;
        }
    }
}