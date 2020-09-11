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
        public Accessibility(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Disables the accessibility domain.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables the accessibility domain which causes `AXNodeId`s to remain consistent between method calls.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Fetches the accessibility node and partial accessibility tree for this DOM node, if it exists.
        /// </summary>
        public async System.Threading.Tasks.Task<GetPartialAXTreeResponse> GetPartialAXTreeAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null, bool? fetchRelatives = null)
        {
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
        public async System.Threading.Tasks.Task<GetFullAXTreeResponse> GetFullAXTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Accessibility.getFullAXTree", dict);
            return methodResult.DeserializeJson<GetFullAXTreeResponse>();
        }
    }
}