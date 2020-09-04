// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// This domain exposes DOM read/write operations. Each DOM Node is represented with its mirror object
    public partial class DOM : DevToolsDomainBase
    {
        public DOM(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Describes node given its id, does not require domain to be enabled. Does not start tracking any
        public async System.Threading.Tasks.Task<DescribeNodeResponse> DescribeNodeAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null, int? depth = null, bool? pierce = null)
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

            if (depth.HasValue)
            {
                dict.Add("depth", depth.Value);
            }

            if (pierce.HasValue)
            {
                dict.Add("pierce", pierce.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.describeNode", dict);
            return result.DeserializeJson<DescribeNodeResponse>();
        }

        /// <summary>
        /// Disables DOM agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.disable", dict);
            return result;
        }

        /// <summary>
        /// Enables DOM agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.enable", dict);
            return result;
        }

        /// <summary>
        /// Focuses the given element.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> FocusAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.focus", dict);
            return result;
        }

        /// <summary>
        /// Returns attributes for the specified node.
        /// </summary>
        public async System.Threading.Tasks.Task<GetAttributesResponse> GetAttributesAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.getAttributes", dict);
            return result.DeserializeJson<GetAttributesResponse>();
        }

        /// <summary>
        /// Returns boxes for the given node.
        /// </summary>
        public async System.Threading.Tasks.Task<GetBoxModelResponse> GetBoxModelAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.getBoxModel", dict);
            return result.DeserializeJson<GetBoxModelResponse>();
        }

        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        /// </summary>
        public async System.Threading.Tasks.Task<GetDocumentResponse> GetDocumentAsync(int? depth = null, bool? pierce = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (depth.HasValue)
            {
                dict.Add("depth", depth.Value);
            }

            if (pierce.HasValue)
            {
                dict.Add("pierce", pierce.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.getDocument", dict);
            return result.DeserializeJson<GetDocumentResponse>();
        }

        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        public async System.Threading.Tasks.Task<GetFlattenedDocumentResponse> GetFlattenedDocumentAsync(int? depth = null, bool? pierce = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (depth.HasValue)
            {
                dict.Add("depth", depth.Value);
            }

            if (pierce.HasValue)
            {
                dict.Add("pierce", pierce.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.getFlattenedDocument", dict);
            return result.DeserializeJson<GetFlattenedDocumentResponse>();
        }

        /// <summary>
        /// Returns node id at given location. Depending on whether DOM domain is enabled, nodeId is
        public async System.Threading.Tasks.Task<GetNodeForLocationResponse> GetNodeForLocationAsync(int x, int y, bool? includeUserAgentShadowDOM = null, bool? ignorePointerEventsNone = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            if (includeUserAgentShadowDOM.HasValue)
            {
                dict.Add("includeUserAgentShadowDOM", includeUserAgentShadowDOM.Value);
            }

            if (ignorePointerEventsNone.HasValue)
            {
                dict.Add("ignorePointerEventsNone", ignorePointerEventsNone.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.getNodeForLocation", dict);
            return result.DeserializeJson<GetNodeForLocationResponse>();
        }

        /// <summary>
        /// Returns node's HTML markup.
        /// </summary>
        public async System.Threading.Tasks.Task<GetOuterHTMLResponse> GetOuterHTMLAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.getOuterHTML", dict);
            return result.DeserializeJson<GetOuterHTMLResponse>();
        }

        /// <summary>
        /// Hides any highlight.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HideHighlightAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.hideHighlight", dict);
            return result;
        }

        /// <summary>
        /// Highlights DOM node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HighlightNodeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.highlightNode", dict);
            return result;
        }

        /// <summary>
        /// Highlights given rectangle.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HighlightRectAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.highlightRect", dict);
            return result;
        }

        /// <summary>
        /// Moves node into the new container, places it before the given anchor.
        /// </summary>
        public async System.Threading.Tasks.Task<MoveToResponse> MoveToAsync(int nodeId, int targetNodeId, int? insertBeforeNodeId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("targetNodeId", targetNodeId);
            if (insertBeforeNodeId.HasValue)
            {
                dict.Add("insertBeforeNodeId", insertBeforeNodeId.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.moveTo", dict);
            return result.DeserializeJson<MoveToResponse>();
        }

        /// <summary>
        /// Executes `querySelector` on a given node.
        /// </summary>
        public async System.Threading.Tasks.Task<QuerySelectorResponse> QuerySelectorAsync(int nodeId, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("selector", selector);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.querySelector", dict);
            return result.DeserializeJson<QuerySelectorResponse>();
        }

        /// <summary>
        /// Executes `querySelectorAll` on a given node.
        /// </summary>
        public async System.Threading.Tasks.Task<QuerySelectorAllResponse> QuerySelectorAllAsync(int nodeId, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("selector", selector);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.querySelectorAll", dict);
            return result.DeserializeJson<QuerySelectorAllResponse>();
        }

        /// <summary>
        /// Removes attribute with given name from an element with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveAttributeAsync(int nodeId, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.removeAttribute", dict);
            return result;
        }

        /// <summary>
        /// Removes node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.removeNode", dict);
            return result;
        }

        /// <summary>
        /// Requests that children of the node with given id are returned to the caller in form of
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RequestChildNodesAsync(int nodeId, int? depth = null, bool? pierce = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            if (depth.HasValue)
            {
                dict.Add("depth", depth.Value);
            }

            if (pierce.HasValue)
            {
                dict.Add("pierce", pierce.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.requestChildNodes", dict);
            return result;
        }

        /// <summary>
        /// Requests that the node is sent to the caller given the JavaScript node object reference. All
        public async System.Threading.Tasks.Task<RequestNodeResponse> RequestNodeAsync(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.requestNode", dict);
            return result.DeserializeJson<RequestNodeResponse>();
        }

        /// <summary>
        /// Resolves the JavaScript node object for a given NodeId or BackendNodeId.
        /// </summary>
        public async System.Threading.Tasks.Task<ResolveNodeResponse> ResolveNodeAsync(int? nodeId = null, int? backendNodeId = null, string objectGroup = null, int? executionContextId = null)
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

            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.resolveNode", dict);
            return result.DeserializeJson<ResolveNodeResponse>();
        }

        /// <summary>
        /// Sets attribute for an element with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAttributeValueAsync(int nodeId, string name, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            dict.Add("value", value);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.setAttributeValue", dict);
            return result;
        }

        /// <summary>
        /// Sets attributes on element with given id. This method is useful when user edits some existing
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAttributesAsTextAsync(int nodeId, string text, string name = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("text", text);
            if (!(string.IsNullOrEmpty(name)))
            {
                dict.Add("name", name);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.setAttributesAsText", dict);
            return result;
        }

        /// <summary>
        /// Sets files for the given file input element.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetFileInputFilesAsync(string files, int? nodeId = null, int? backendNodeId = null, string objectId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("files", files);
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

            var result = await _client.ExecuteDevToolsMethodAsync("DOM.setFileInputFiles", dict);
            return result;
        }

        /// <summary>
        /// Sets node name for a node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<SetNodeNameResponse> SetNodeNameAsync(int nodeId, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeName", dict);
            return result.DeserializeJson<SetNodeNameResponse>();
        }

        /// <summary>
        /// Sets node value for a node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetNodeValueAsync(int nodeId, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("value", value);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeValue", dict);
            return result;
        }

        /// <summary>
        /// Sets node HTML markup, returns new node id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetOuterHTMLAsync(int nodeId, string outerHTML)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("outerHTML", outerHTML);
            var result = await _client.ExecuteDevToolsMethodAsync("DOM.setOuterHTML", dict);
            return result;
        }
    }
}