// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    using System.Linq;

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
        /// Collects class names for the node with given id and all of it's child nodes.
        /// </summary>
        public async System.Threading.Tasks.Task<CollectClassNamesFromSubtreeResponse> CollectClassNamesFromSubtreeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.collectClassNamesFromSubtree", dict);
            return methodResult.DeserializeJson<CollectClassNamesFromSubtreeResponse>();
        }

        /// <summary>
        /// Creates a deep copy of the specified node and places it into the target container before the
        public async System.Threading.Tasks.Task<CopyToResponse> CopyToAsync(int nodeId, int targetNodeId, int? insertBeforeNodeId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("targetNodeId", targetNodeId);
            if (insertBeforeNodeId.HasValue)
            {
                dict.Add("insertBeforeNodeId", insertBeforeNodeId.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.copyTo", dict);
            return methodResult.DeserializeJson<CopyToResponse>();
        }

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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.describeNode", dict);
            return methodResult.DeserializeJson<DescribeNodeResponse>();
        }

        /// <summary>
        /// Scrolls the specified rect of the given node into view if not already visible.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ScrollIntoViewIfNeededAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null, CefSharp.DevTools.DOM.Rect rect = null)
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

            if ((rect) != (null))
            {
                dict.Add("rect", rect.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.scrollIntoViewIfNeeded", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables DOM agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Discards search results from the session with the given id. `getSearchResults` should no longer
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DiscardSearchResultsAsync(string searchId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("searchId", searchId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.discardSearchResults", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables DOM agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Focuses the given element.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> FocusAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.focus", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns attributes for the specified node.
        /// </summary>
        public async System.Threading.Tasks.Task<GetAttributesResponse> GetAttributesAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getAttributes", dict);
            return methodResult.DeserializeJson<GetAttributesResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getBoxModel", dict);
            return methodResult.DeserializeJson<GetBoxModelResponse>();
        }

        /// <summary>
        /// Returns quads that describe node position on the page. This method
        public async System.Threading.Tasks.Task<GetContentQuadsResponse> GetContentQuadsAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getContentQuads", dict);
            return methodResult.DeserializeJson<GetContentQuadsResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getDocument", dict);
            return methodResult.DeserializeJson<GetDocumentResponse>();
        }

        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        /// </summary>
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getFlattenedDocument", dict);
            return methodResult.DeserializeJson<GetFlattenedDocumentResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getNodeForLocation", dict);
            return methodResult.DeserializeJson<GetNodeForLocationResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getOuterHTML", dict);
            return methodResult.DeserializeJson<GetOuterHTMLResponse>();
        }

        /// <summary>
        /// Returns the id of the nearest ancestor that is a relayout boundary.
        /// </summary>
        public async System.Threading.Tasks.Task<GetRelayoutBoundaryResponse> GetRelayoutBoundaryAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getRelayoutBoundary", dict);
            return methodResult.DeserializeJson<GetRelayoutBoundaryResponse>();
        }

        /// <summary>
        /// Returns search results from given `fromIndex` to given `toIndex` from the search with the given
        public async System.Threading.Tasks.Task<GetSearchResultsResponse> GetSearchResultsAsync(string searchId, int fromIndex, int toIndex)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("searchId", searchId);
            dict.Add("fromIndex", fromIndex);
            dict.Add("toIndex", toIndex);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getSearchResults", dict);
            return methodResult.DeserializeJson<GetSearchResultsResponse>();
        }

        /// <summary>
        /// Hides any highlight.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HideHighlightAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.hideHighlight", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights DOM node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightNodeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.highlightNode", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights given rectangle.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightRectAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.highlightRect", dict);
            return methodResult;
        }

        /// <summary>
        /// Marks last undoable state.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> MarkUndoableStateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.markUndoableState", dict);
            return methodResult;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.moveTo", dict);
            return methodResult.DeserializeJson<MoveToResponse>();
        }

        /// <summary>
        /// Searches for a given string in the DOM tree. Use `getSearchResults` to access search results or
        public async System.Threading.Tasks.Task<PerformSearchResponse> PerformSearchAsync(string query, bool? includeUserAgentShadowDOM = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("query", query);
            if (includeUserAgentShadowDOM.HasValue)
            {
                dict.Add("includeUserAgentShadowDOM", includeUserAgentShadowDOM.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.performSearch", dict);
            return methodResult.DeserializeJson<PerformSearchResponse>();
        }

        /// <summary>
        /// Requests that the node is sent to the caller given its path. // FIXME, use XPath
        /// </summary>
        public async System.Threading.Tasks.Task<PushNodeByPathToFrontendResponse> PushNodeByPathToFrontendAsync(string path)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("path", path);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.pushNodeByPathToFrontend", dict);
            return methodResult.DeserializeJson<PushNodeByPathToFrontendResponse>();
        }

        /// <summary>
        /// Requests that a batch of nodes is sent to the caller given their backend node ids.
        /// </summary>
        public async System.Threading.Tasks.Task<PushNodesByBackendIdsToFrontendResponse> PushNodesByBackendIdsToFrontendAsync(int[] backendNodeIds)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("backendNodeIds", backendNodeIds);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.pushNodesByBackendIdsToFrontend", dict);
            return methodResult.DeserializeJson<PushNodesByBackendIdsToFrontendResponse>();
        }

        /// <summary>
        /// Executes `querySelector` on a given node.
        /// </summary>
        public async System.Threading.Tasks.Task<QuerySelectorResponse> QuerySelectorAsync(int nodeId, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("selector", selector);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.querySelector", dict);
            return methodResult.DeserializeJson<QuerySelectorResponse>();
        }

        /// <summary>
        /// Executes `querySelectorAll` on a given node.
        /// </summary>
        public async System.Threading.Tasks.Task<QuerySelectorAllResponse> QuerySelectorAllAsync(int nodeId, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("selector", selector);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.querySelectorAll", dict);
            return methodResult.DeserializeJson<QuerySelectorAllResponse>();
        }

        /// <summary>
        /// Re-does the last undone action.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RedoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.redo", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes attribute with given name from an element with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveAttributeAsync(int nodeId, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.removeAttribute", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.removeNode", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that children of the node with given id are returned to the caller in form of
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RequestChildNodesAsync(int nodeId, int? depth = null, bool? pierce = null)
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.requestChildNodes", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that the node is sent to the caller given the JavaScript node object reference. All
        public async System.Threading.Tasks.Task<RequestNodeResponse> RequestNodeAsync(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.requestNode", dict);
            return methodResult.DeserializeJson<RequestNodeResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.resolveNode", dict);
            return methodResult.DeserializeJson<ResolveNodeResponse>();
        }

        /// <summary>
        /// Sets attribute for an element with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAttributeValueAsync(int nodeId, string name, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setAttributeValue", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets attributes on element with given id. This method is useful when user edits some existing
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAttributesAsTextAsync(int nodeId, string text, string name = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("text", text);
            if (!(string.IsNullOrEmpty(name)))
            {
                dict.Add("name", name);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setAttributesAsText", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets files for the given file input element.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFileInputFilesAsync(string[] files, int? nodeId = null, int? backendNodeId = null, string objectId = null)
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setFileInputFiles", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets if stack traces should be captured for Nodes. See `Node.getNodeStackTraces`. Default is disabled.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetNodeStackTracesEnabledAsync(bool enable)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enable", enable);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeStackTracesEnabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Gets stack traces associated with a Node. As of now, only provides stack trace for Node creation.
        /// </summary>
        public async System.Threading.Tasks.Task<GetNodeStackTracesResponse> GetNodeStackTracesAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getNodeStackTraces", dict);
            return methodResult.DeserializeJson<GetNodeStackTracesResponse>();
        }

        /// <summary>
        /// Returns file information for the given
        public async System.Threading.Tasks.Task<GetFileInfoResponse> GetFileInfoAsync(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getFileInfo", dict);
            return methodResult.DeserializeJson<GetFileInfoResponse>();
        }

        /// <summary>
        /// Enables console to refer to the node with given id via $x (see Command Line API for more details
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInspectedNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setInspectedNode", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets node name for a node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<SetNodeNameResponse> SetNodeNameAsync(int nodeId, string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeName", dict);
            return methodResult.DeserializeJson<SetNodeNameResponse>();
        }

        /// <summary>
        /// Sets node value for a node with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetNodeValueAsync(int nodeId, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeValue", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets node HTML markup, returns new node id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetOuterHTMLAsync(int nodeId, string outerHTML)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("outerHTML", outerHTML);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setOuterHTML", dict);
            return methodResult;
        }

        /// <summary>
        /// Undoes the last performed action.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UndoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.undo", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns iframe node that owns iframe with the given domain.
        /// </summary>
        public async System.Threading.Tasks.Task<GetFrameOwnerResponse> GetFrameOwnerAsync(string frameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getFrameOwner", dict);
            return methodResult.DeserializeJson<GetFrameOwnerResponse>();
        }
    }
}