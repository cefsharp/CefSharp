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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Database(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables database tracking, prevents database events from being sent to the client.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables database tracking, database events will now be delivered to the client.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.enable", dict);
            return methodResult;
        }

        partial void ValidateExecuteSQL(string databaseId, string query);
        /// <summary>
        /// ExecuteSQL
        /// </summary>
        /// <param name = "databaseId">databaseId</param>
        /// <param name = "query">query</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ExecuteSQLResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ExecuteSQLResponse> ExecuteSQLAsync(string databaseId, string query)
        {
            ValidateExecuteSQL(databaseId, query);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("databaseId", databaseId);
            dict.Add("query", query);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.executeSQL", dict);
            return methodResult.DeserializeJson<ExecuteSQLResponse>();
        }

        partial void ValidateGetDatabaseTableNames(string databaseId);
        /// <summary>
        /// GetDatabaseTableNames
        /// </summary>
        /// <param name = "databaseId">databaseId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetDatabaseTableNamesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetDatabaseTableNamesResponse> GetDatabaseTableNamesAsync(string databaseId)
        {
            ValidateGetDatabaseTableNames(databaseId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("databaseId", databaseId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Database.getDatabaseTableNames", dict);
            return methodResult.DeserializeJson<GetDatabaseTableNamesResponse>();
        }
    }
}