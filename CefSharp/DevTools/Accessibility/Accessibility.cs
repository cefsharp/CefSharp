// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    using System.Linq;

    /// <summary>
    /// Accessibility
    /// </summary>
    public partial class Accessibility : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Accessibility(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables the accessibility domain.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables the accessibility domain which causes `AXNodeId`s to remain consistent between method calls.
        /// This turns on accessibility for the page, which can impact performance until accessibility is disabled.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.enable", dict);
            return methodResult;
        }

        partial void ValidateGetPartialAXTree(int? nodeId = null, int? backendNodeId = null, string objectId = null, bool? fetchRelatives = null);
        /// <summary>
        /// Fetches the accessibility node and partial accessibility tree for this DOM node, if it exists.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node to get the partial accessibility tree for.</param>
        /// <param name = "backendNodeId">Identifier of the backend node to get the partial accessibility tree for.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper to get the partial accessibility tree for.</param>
        /// <param name = "fetchRelatives">Whether to fetch this nodes ancestors, siblings and children. Defaults to true.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetPartialAXTreeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetPartialAXTreeResponse> GetPartialAXTreeAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null, bool? fetchRelatives = null)
        {
            ValidateGetPartialAXTree(nodeId, backendNodeId, objectId, fetchRelatives);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (nodeId.HasValue)
            {
                dict.Add("nodeId", nodeId.Value);
            }

            if (backendNodeId.HasValue)
            {
                dict.Add("backendNodeId", backendNodeId.Value);
            }

            if (!(string.IsNullOrEmpty(objectId)))
            {
                dict.Add("objectId", objectId);
            }

            if (fetchRelatives.HasValue)
            {
                dict.Add("fetchRelatives", fetchRelatives.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.getPartialAXTree", dict);
            return methodResult.DeserializeJson<GetPartialAXTreeResponse>();
        }

        /// <summary>
        /// Fetches the entire accessibility tree
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetFullAXTreeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetFullAXTreeResponse> GetFullAXTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.getFullAXTree", dict);
            return methodResult.DeserializeJson<GetFullAXTreeResponse>();
        }
    }
}