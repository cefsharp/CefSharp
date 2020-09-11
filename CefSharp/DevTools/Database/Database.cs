// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Database
{
    using System.Linq;

    /// <summary>
    /// Database
    /// </summary>
    public partial class Database : DevToolsDomainBase
    {
        public Database(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Disables database tracking, prevents database events from being sent to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables database tracking, database events will now be delivered to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<ExecuteSQLResponse> ExecuteSQLAsync(string databaseId, string query)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("databaseId", databaseId);
            dict.Add("query", query);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.executeSQL", dict);
            return methodResult.DeserializeJson<ExecuteSQLResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetDatabaseTableNamesResponse> GetDatabaseTableNamesAsync(string databaseId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("databaseId", databaseId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.getDatabaseTableNames", dict);
            return methodResult.DeserializeJson<GetDatabaseTableNamesResponse>();
        }
    }
}