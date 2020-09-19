// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    using System.Linq;

    /// <summary>
    /// IndexedDB
    /// </summary>
    public partial class IndexedDB : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public IndexedDB(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateClearObjectStore(string securityOrigin, string databaseName, string objectStoreName);
        /// <summary>
        /// Clears all entries from an object store.
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <param name = "databaseName">Database name.</param>
        /// <param name = "objectStoreName">Object store name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearObjectStoreAsync(string securityOrigin, string databaseName, string objectStoreName)
        {
            ValidateClearObjectStore(securityOrigin, databaseName, objectStoreName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            dict.Add("databaseName", databaseName);
            dict.Add("objectStoreName", objectStoreName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.clearObjectStore", dict);
            return methodResult;
        }

        partial void ValidateDeleteDatabase(string securityOrigin, string databaseName);
        /// <summary>
        /// Deletes a database.
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <param name = "databaseName">Database name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteDatabaseAsync(string securityOrigin, string databaseName)
        {
            ValidateDeleteDatabase(securityOrigin, databaseName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            dict.Add("databaseName", databaseName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.deleteDatabase", dict);
            return methodResult;
        }

        partial void ValidateDeleteObjectStoreEntries(string securityOrigin, string databaseName, string objectStoreName, CefSharp.DevTools.IndexedDB.KeyRange keyRange);
        /// <summary>
        /// Delete a range of entries from an object store
        /// </summary>
        /// <param name = "securityOrigin">securityOrigin</param>
        /// <param name = "databaseName">databaseName</param>
        /// <param name = "objectStoreName">objectStoreName</param>
        /// <param name = "keyRange">Range of entry keys to delete</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteObjectStoreEntriesAsync(string securityOrigin, string databaseName, string objectStoreName, CefSharp.DevTools.IndexedDB.KeyRange keyRange)
        {
            ValidateDeleteObjectStoreEntries(securityOrigin, databaseName, objectStoreName, keyRange);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            dict.Add("databaseName", databaseName);
            dict.Add("objectStoreName", objectStoreName);
            dict.Add("keyRange", keyRange.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.deleteObjectStoreEntries", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables events from backend.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables events from backend.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.enable", dict);
            return methodResult;
        }

        partial void ValidateRequestData(string securityOrigin, string databaseName, string objectStoreName, string indexName, int skipCount, int pageSize, CefSharp.DevTools.IndexedDB.KeyRange keyRange = null);
        /// <summary>
        /// Requests data from object store or index.
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <param name = "databaseName">Database name.</param>
        /// <param name = "objectStoreName">Object store name.</param>
        /// <param name = "indexName">Index name, empty string for object store data requests.</param>
        /// <param name = "skipCount">Number of records to skip.</param>
        /// <param name = "pageSize">Number of records to fetch.</param>
        /// <param name = "keyRange">Key range.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestDataResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestDataResponse> RequestDataAsync(string securityOrigin, string databaseName, string objectStoreName, string indexName, int skipCount, int pageSize, CefSharp.DevTools.IndexedDB.KeyRange keyRange = null)
        {
            ValidateRequestData(securityOrigin, databaseName, objectStoreName, indexName, skipCount, pageSize, keyRange);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            dict.Add("databaseName", databaseName);
            dict.Add("objectStoreName", objectStoreName);
            dict.Add("indexName", indexName);
            dict.Add("skipCount", skipCount);
            dict.Add("pageSize", pageSize);
            if ((keyRange) != (null))
            {
                dict.Add("keyRange", keyRange.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.requestData", dict);
            return methodResult.DeserializeJson<RequestDataResponse>();
        }

        partial void ValidateGetMetadata(string securityOrigin, string databaseName, string objectStoreName);
        /// <summary>
        /// Gets metadata of an object store
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <param name = "databaseName">Database name.</param>
        /// <param name = "objectStoreName">Object store name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetMetadataResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetMetadataResponse> GetMetadataAsync(string securityOrigin, string databaseName, string objectStoreName)
        {
            ValidateGetMetadata(securityOrigin, databaseName, objectStoreName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            dict.Add("databaseName", databaseName);
            dict.Add("objectStoreName", objectStoreName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.getMetadata", dict);
            return methodResult.DeserializeJson<GetMetadataResponse>();
        }

        partial void ValidateRequestDatabase(string securityOrigin, string databaseName);
        /// <summary>
        /// Requests database with given name in given frame.
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <param name = "databaseName">Database name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestDatabaseResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestDatabaseResponse> RequestDatabaseAsync(string securityOrigin, string databaseName)
        {
            ValidateRequestDatabase(securityOrigin, databaseName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            dict.Add("databaseName", databaseName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.requestDatabase", dict);
            return methodResult.DeserializeJson<RequestDatabaseResponse>();
        }

        partial void ValidateRequestDatabaseNames(string securityOrigin);
        /// <summary>
        /// Requests database names for given security origin.
        /// </summary>
        /// <param name = "securityOrigin">Security origin.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestDatabaseNamesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestDatabaseNamesResponse> RequestDatabaseNamesAsync(string securityOrigin)
        {
            ValidateRequestDatabaseNames(securityOrigin);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("IndexedDB.requestDatabaseNames", dict);
            return methodResult.DeserializeJson<RequestDatabaseNamesResponse>();
        }
    }
}