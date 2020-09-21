// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    using System.Linq;

    /// <summary>
    /// This domain exposes DOM read/write operations. Each DOM Node is represented with its mirror object
    /// that has an `id`. This `id` can be used to get additional information on the Node, resolve it into
    /// the JavaScript object wrapper, etc. It is important that client receives DOM events only for the
    /// nodes that are known to the client. Backend keeps track of the nodes that were sent to the client
    /// and never sends the same node twice. It is client's responsibility to collect information about
    /// the nodes that were sent to the client.<p>Note that `iframe` owner elements will return
    /// corresponding document elements as their child nodes.</p>
    /// </summary>
    public partial class DOM : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public DOM(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateCollectClassNamesFromSubtree(int nodeId);
        /// <summary>
        /// Collects class names for the node with given id and all of it's child nodes.
        /// </summary>
        /// <param name = "nodeId">Id of the node to collect class names.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CollectClassNamesFromSubtreeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CollectClassNamesFromSubtreeResponse> CollectClassNamesFromSubtreeAsync(int nodeId)
        {
            ValidateCollectClassNamesFromSubtree(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.collectClassNamesFromSubtree", dict);
            return methodResult.DeserializeJson<CollectClassNamesFromSubtreeResponse>();
        }

        partial void ValidateCopyTo(int nodeId, int targetNodeId, int? insertBeforeNodeId = null);
        /// <summary>
        /// Creates a deep copy of the specified node and places it into the target container before the
        /// given anchor.
        /// </summary>
        /// <param name = "nodeId">Id of the node to copy.</param>
        /// <param name = "targetNodeId">Id of the element to drop the copy into.</param>
        /// <param name = "insertBeforeNodeId">Drop the copy before this node (if absent, the copy becomes the last child of
        public async System.Threading.Tasks.Task<CopyToResponse> CopyToAsync(int nodeId, int targetNodeId, int? insertBeforeNodeId = null)
        {
            ValidateCopyTo(nodeId, targetNodeId, insertBeforeNodeId);
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

        partial void ValidateDescribeNode(int? nodeId = null, int? backendNodeId = null, string objectId = null, int? depth = null, bool? pierce = null);
        /// <summary>
        /// Describes node given its id, does not require domain to be enabled. Does not start tracking any
        /// objects, can be used for automation.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <param name = "depth">The maximum depth at which children should be retrieved, defaults to 1. Use -1 for the
        public async System.Threading.Tasks.Task<DescribeNodeResponse> DescribeNodeAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null, int? depth = null, bool? pierce = null)
        {
            ValidateDescribeNode(nodeId, backendNodeId, objectId, depth, pierce);
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

        partial void ValidateScrollIntoViewIfNeeded(int? nodeId = null, int? backendNodeId = null, string objectId = null, CefSharp.DevTools.DOM.Rect rect = null);
        /// <summary>
        /// Scrolls the specified rect of the given node into view if not already visible.
        /// Note: exactly one between nodeId, backendNodeId and objectId should be passed
        /// to identify the node.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <param name = "rect">The rect to be scrolled into view, relative to the node's border box, in CSS pixels.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ScrollIntoViewIfNeededAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null, CefSharp.DevTools.DOM.Rect rect = null)
        {
            ValidateScrollIntoViewIfNeeded(nodeId, backendNodeId, objectId, rect);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.disable", dict);
            return methodResult;
        }

        partial void ValidateDiscardSearchResults(string searchId);
        /// <summary>
        /// Discards search results from the session with the given id. `getSearchResults` should no longer
        /// be called for that search.
        /// </summary>
        /// <param name = "searchId">Unique search session identifier.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DiscardSearchResultsAsync(string searchId)
        {
            ValidateDiscardSearchResults(searchId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("searchId", searchId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.discardSearchResults", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables DOM agent for the given page.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.enable", dict);
            return methodResult;
        }

        partial void ValidateFocus(int? nodeId = null, int? backendNodeId = null, string objectId = null);
        /// <summary>
        /// Focuses the given element.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> FocusAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
        {
            ValidateFocus(nodeId, backendNodeId, objectId);
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

        partial void ValidateGetAttributes(int nodeId);
        /// <summary>
        /// Returns attributes for the specified node.
        /// </summary>
        /// <param name = "nodeId">Id of the node to retrieve attibutes for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetAttributesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetAttributesResponse> GetAttributesAsync(int nodeId)
        {
            ValidateGetAttributes(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getAttributes", dict);
            return methodResult.DeserializeJson<GetAttributesResponse>();
        }

        partial void ValidateGetBoxModel(int? nodeId = null, int? backendNodeId = null, string objectId = null);
        /// <summary>
        /// Returns boxes for the given node.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetBoxModelResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetBoxModelResponse> GetBoxModelAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
        {
            ValidateGetBoxModel(nodeId, backendNodeId, objectId);
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

        partial void ValidateGetContentQuads(int? nodeId = null, int? backendNodeId = null, string objectId = null);
        /// <summary>
        /// Returns quads that describe node position on the page. This method
        /// might return multiple quads for inline nodes.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetContentQuadsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetContentQuadsResponse> GetContentQuadsAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
        {
            ValidateGetContentQuads(nodeId, backendNodeId, objectId);
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

        partial void ValidateGetDocument(int? depth = null, bool? pierce = null);
        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        /// </summary>
        /// <param name = "depth">The maximum depth at which children should be retrieved, defaults to 1. Use -1 for the
        public async System.Threading.Tasks.Task<GetDocumentResponse> GetDocumentAsync(int? depth = null, bool? pierce = null)
        {
            ValidateGetDocument(depth, pierce);
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

        partial void ValidateGetFlattenedDocument(int? depth = null, bool? pierce = null);
        /// <summary>
        /// Returns the root DOM node (and optionally the subtree) to the caller.
        /// </summary>
        /// <param name = "depth">The maximum depth at which children should be retrieved, defaults to 1. Use -1 for the
        public async System.Threading.Tasks.Task<GetFlattenedDocumentResponse> GetFlattenedDocumentAsync(int? depth = null, bool? pierce = null)
        {
            ValidateGetFlattenedDocument(depth, pierce);
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

        partial void ValidateGetNodeForLocation(int x, int y, bool? includeUserAgentShadowDOM = null, bool? ignorePointerEventsNone = null);
        /// <summary>
        /// Returns node id at given location. Depending on whether DOM domain is enabled, nodeId is
        /// either returned or not.
        /// </summary>
        /// <param name = "x">X coordinate.</param>
        /// <param name = "y">Y coordinate.</param>
        /// <param name = "includeUserAgentShadowDOM">False to skip to the nearest non-UA shadow root ancestor (default: false).</param>
        /// <param name = "ignorePointerEventsNone">Whether to ignore pointer-events: none on elements and hit test them.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetNodeForLocationResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetNodeForLocationResponse> GetNodeForLocationAsync(int x, int y, bool? includeUserAgentShadowDOM = null, bool? ignorePointerEventsNone = null)
        {
            ValidateGetNodeForLocation(x, y, includeUserAgentShadowDOM, ignorePointerEventsNone);
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

        partial void ValidateGetOuterHTML(int? nodeId = null, int? backendNodeId = null, string objectId = null);
        /// <summary>
        /// Returns node's HTML markup.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetOuterHTMLResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetOuterHTMLResponse> GetOuterHTMLAsync(int? nodeId = null, int? backendNodeId = null, string objectId = null)
        {
            ValidateGetOuterHTML(nodeId, backendNodeId, objectId);
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

        partial void ValidateGetRelayoutBoundary(int nodeId);
        /// <summary>
        /// Returns the id of the nearest ancestor that is a relayout boundary.
        /// </summary>
        /// <param name = "nodeId">Id of the node.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetRelayoutBoundaryResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetRelayoutBoundaryResponse> GetRelayoutBoundaryAsync(int nodeId)
        {
            ValidateGetRelayoutBoundary(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getRelayoutBoundary", dict);
            return methodResult.DeserializeJson<GetRelayoutBoundaryResponse>();
        }

        partial void ValidateGetSearchResults(string searchId, int fromIndex, int toIndex);
        /// <summary>
        /// Returns search results from given `fromIndex` to given `toIndex` from the search with the given
        /// identifier.
        /// </summary>
        /// <param name = "searchId">Unique search session identifier.</param>
        /// <param name = "fromIndex">Start index of the search result to be returned.</param>
        /// <param name = "toIndex">End index of the search result to be returned.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetSearchResultsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetSearchResultsResponse> GetSearchResultsAsync(string searchId, int fromIndex, int toIndex)
        {
            ValidateGetSearchResults(searchId, fromIndex, toIndex);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HideHighlightAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.hideHighlight", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights DOM node.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightNodeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.highlightNode", dict);
            return methodResult;
        }

        /// <summary>
        /// Highlights given rectangle.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightRectAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.highlightRect", dict);
            return methodResult;
        }

        /// <summary>
        /// Marks last undoable state.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> MarkUndoableStateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.markUndoableState", dict);
            return methodResult;
        }

        partial void ValidateMoveTo(int nodeId, int targetNodeId, int? insertBeforeNodeId = null);
        /// <summary>
        /// Moves node into the new container, places it before the given anchor.
        /// </summary>
        /// <param name = "nodeId">Id of the node to move.</param>
        /// <param name = "targetNodeId">Id of the element to drop the moved node into.</param>
        /// <param name = "insertBeforeNodeId">Drop node before this one (if absent, the moved node becomes the last child of
        public async System.Threading.Tasks.Task<MoveToResponse> MoveToAsync(int nodeId, int targetNodeId, int? insertBeforeNodeId = null)
        {
            ValidateMoveTo(nodeId, targetNodeId, insertBeforeNodeId);
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

        partial void ValidatePerformSearch(string query, bool? includeUserAgentShadowDOM = null);
        /// <summary>
        /// Searches for a given string in the DOM tree. Use `getSearchResults` to access search results or
        /// `cancelSearch` to end this search session.
        /// </summary>
        /// <param name = "query">Plain text or query selector or XPath search query.</param>
        /// <param name = "includeUserAgentShadowDOM">True to search in user agent shadow DOM.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;PerformSearchResponse&gt;</returns>
        public async System.Threading.Tasks.Task<PerformSearchResponse> PerformSearchAsync(string query, bool? includeUserAgentShadowDOM = null)
        {
            ValidatePerformSearch(query, includeUserAgentShadowDOM);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("query", query);
            if (includeUserAgentShadowDOM.HasValue)
            {
                dict.Add("includeUserAgentShadowDOM", includeUserAgentShadowDOM.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.performSearch", dict);
            return methodResult.DeserializeJson<PerformSearchResponse>();
        }

        partial void ValidatePushNodeByPathToFrontend(string path);
        /// <summary>
        /// Requests that the node is sent to the caller given its path. // FIXME, use XPath
        /// </summary>
        /// <param name = "path">Path to node in the proprietary format.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;PushNodeByPathToFrontendResponse&gt;</returns>
        public async System.Threading.Tasks.Task<PushNodeByPathToFrontendResponse> PushNodeByPathToFrontendAsync(string path)
        {
            ValidatePushNodeByPathToFrontend(path);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("path", path);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.pushNodeByPathToFrontend", dict);
            return methodResult.DeserializeJson<PushNodeByPathToFrontendResponse>();
        }

        partial void ValidatePushNodesByBackendIdsToFrontend(int[] backendNodeIds);
        /// <summary>
        /// Requests that a batch of nodes is sent to the caller given their backend node ids.
        /// </summary>
        /// <param name = "backendNodeIds">The array of backend node ids.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;PushNodesByBackendIdsToFrontendResponse&gt;</returns>
        public async System.Threading.Tasks.Task<PushNodesByBackendIdsToFrontendResponse> PushNodesByBackendIdsToFrontendAsync(int[] backendNodeIds)
        {
            ValidatePushNodesByBackendIdsToFrontend(backendNodeIds);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("backendNodeIds", backendNodeIds);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.pushNodesByBackendIdsToFrontend", dict);
            return methodResult.DeserializeJson<PushNodesByBackendIdsToFrontendResponse>();
        }

        partial void ValidateQuerySelector(int nodeId, string selector);
        /// <summary>
        /// Executes `querySelector` on a given node.
        /// </summary>
        /// <param name = "nodeId">Id of the node to query upon.</param>
        /// <param name = "selector">Selector string.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;QuerySelectorResponse&gt;</returns>
        public async System.Threading.Tasks.Task<QuerySelectorResponse> QuerySelectorAsync(int nodeId, string selector)
        {
            ValidateQuerySelector(nodeId, selector);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("selector", selector);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.querySelector", dict);
            return methodResult.DeserializeJson<QuerySelectorResponse>();
        }

        partial void ValidateQuerySelectorAll(int nodeId, string selector);
        /// <summary>
        /// Executes `querySelectorAll` on a given node.
        /// </summary>
        /// <param name = "nodeId">Id of the node to query upon.</param>
        /// <param name = "selector">Selector string.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;QuerySelectorAllResponse&gt;</returns>
        public async System.Threading.Tasks.Task<QuerySelectorAllResponse> QuerySelectorAllAsync(int nodeId, string selector)
        {
            ValidateQuerySelectorAll(nodeId, selector);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("selector", selector);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.querySelectorAll", dict);
            return methodResult.DeserializeJson<QuerySelectorAllResponse>();
        }

        /// <summary>
        /// Re-does the last undone action.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RedoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.redo", dict);
            return methodResult;
        }

        partial void ValidateRemoveAttribute(int nodeId, string name);
        /// <summary>
        /// Removes attribute with given name from an element with given id.
        /// </summary>
        /// <param name = "nodeId">Id of the element to remove attribute from.</param>
        /// <param name = "name">Name of the attribute to remove.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveAttributeAsync(int nodeId, string name)
        {
            ValidateRemoveAttribute(nodeId, name);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.removeAttribute", dict);
            return methodResult;
        }

        partial void ValidateRemoveNode(int nodeId);
        /// <summary>
        /// Removes node with given id.
        /// </summary>
        /// <param name = "nodeId">Id of the node to remove.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveNodeAsync(int nodeId)
        {
            ValidateRemoveNode(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.removeNode", dict);
            return methodResult;
        }

        partial void ValidateRequestChildNodes(int nodeId, int? depth = null, bool? pierce = null);
        /// <summary>
        /// Requests that children of the node with given id are returned to the caller in form of
        /// `setChildNodes` events where not only immediate children are retrieved, but all children down to
        /// the specified depth.
        /// </summary>
        /// <param name = "nodeId">Id of the node to get children for.</param>
        /// <param name = "depth">The maximum depth at which children should be retrieved, defaults to 1. Use -1 for the
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RequestChildNodesAsync(int nodeId, int? depth = null, bool? pierce = null)
        {
            ValidateRequestChildNodes(nodeId, depth, pierce);
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

        partial void ValidateRequestNode(string objectId);
        /// <summary>
        /// Requests that the node is sent to the caller given the JavaScript node object reference. All
        /// nodes that form the path from the node to the root are also sent to the client as a series of
        /// `setChildNodes` notifications.
        /// </summary>
        /// <param name = "objectId">JavaScript object id to convert into node.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestNodeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestNodeResponse> RequestNodeAsync(string objectId)
        {
            ValidateRequestNode(objectId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.requestNode", dict);
            return methodResult.DeserializeJson<RequestNodeResponse>();
        }

        partial void ValidateResolveNode(int? nodeId = null, int? backendNodeId = null, string objectGroup = null, int? executionContextId = null);
        /// <summary>
        /// Resolves the JavaScript node object for a given NodeId or BackendNodeId.
        /// </summary>
        /// <param name = "nodeId">Id of the node to resolve.</param>
        /// <param name = "backendNodeId">Backend identifier of the node to resolve.</param>
        /// <param name = "objectGroup">Symbolic group name that can be used to release multiple objects.</param>
        /// <param name = "executionContextId">Execution context in which to resolve the node.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ResolveNodeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ResolveNodeResponse> ResolveNodeAsync(int? nodeId = null, int? backendNodeId = null, string objectGroup = null, int? executionContextId = null)
        {
            ValidateResolveNode(nodeId, backendNodeId, objectGroup, executionContextId);
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

        partial void ValidateSetAttributeValue(int nodeId, string name, string value);
        /// <summary>
        /// Sets attribute for an element with given id.
        /// </summary>
        /// <param name = "nodeId">Id of the element to set attribute for.</param>
        /// <param name = "name">Attribute name.</param>
        /// <param name = "value">Attribute value.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAttributeValueAsync(int nodeId, string name, string value)
        {
            ValidateSetAttributeValue(nodeId, name, value);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setAttributeValue", dict);
            return methodResult;
        }

        partial void ValidateSetAttributesAsText(int nodeId, string text, string name = null);
        /// <summary>
        /// Sets attributes on element with given id. This method is useful when user edits some existing
        /// attribute value and types in several attribute name/value pairs.
        /// </summary>
        /// <param name = "nodeId">Id of the element to set attributes for.</param>
        /// <param name = "text">Text with a number of attributes. Will parse this text using HTML parser.</param>
        /// <param name = "name">Attribute name to replace with new attributes derived from text in case text parsed
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAttributesAsTextAsync(int nodeId, string text, string name = null)
        {
            ValidateSetAttributesAsText(nodeId, text, name);
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

        partial void ValidateSetFileInputFiles(string[] files, int? nodeId = null, int? backendNodeId = null, string objectId = null);
        /// <summary>
        /// Sets files for the given file input element.
        /// </summary>
        /// <param name = "files">Array of file paths to set.</param>
        /// <param name = "nodeId">Identifier of the node.</param>
        /// <param name = "backendNodeId">Identifier of the backend node.</param>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFileInputFilesAsync(string[] files, int? nodeId = null, int? backendNodeId = null, string objectId = null)
        {
            ValidateSetFileInputFiles(files, nodeId, backendNodeId, objectId);
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

        partial void ValidateSetNodeStackTracesEnabled(bool enable);
        /// <summary>
        /// Sets if stack traces should be captured for Nodes. See `Node.getNodeStackTraces`. Default is disabled.
        /// </summary>
        /// <param name = "enable">Enable or disable.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetNodeStackTracesEnabledAsync(bool enable)
        {
            ValidateSetNodeStackTracesEnabled(enable);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enable", enable);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeStackTracesEnabled", dict);
            return methodResult;
        }

        partial void ValidateGetNodeStackTraces(int nodeId);
        /// <summary>
        /// Gets stack traces associated with a Node. As of now, only provides stack trace for Node creation.
        /// </summary>
        /// <param name = "nodeId">Id of the node to get stack traces for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetNodeStackTracesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetNodeStackTracesResponse> GetNodeStackTracesAsync(int nodeId)
        {
            ValidateGetNodeStackTraces(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getNodeStackTraces", dict);
            return methodResult.DeserializeJson<GetNodeStackTracesResponse>();
        }

        partial void ValidateGetFileInfo(string objectId);
        /// <summary>
        /// Returns file information for the given
        /// File wrapper.
        /// </summary>
        /// <param name = "objectId">JavaScript object id of the node wrapper.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetFileInfoResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetFileInfoResponse> GetFileInfoAsync(string objectId)
        {
            ValidateGetFileInfo(objectId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getFileInfo", dict);
            return methodResult.DeserializeJson<GetFileInfoResponse>();
        }

        partial void ValidateSetInspectedNode(int nodeId);
        /// <summary>
        /// Enables console to refer to the node with given id via $x (see Command Line API for more details
        /// $x functions).
        /// </summary>
        /// <param name = "nodeId">DOM node id to be accessible by means of $x command line API.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInspectedNodeAsync(int nodeId)
        {
            ValidateSetInspectedNode(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setInspectedNode", dict);
            return methodResult;
        }

        partial void ValidateSetNodeName(int nodeId, string name);
        /// <summary>
        /// Sets node name for a node with given id.
        /// </summary>
        /// <param name = "nodeId">Id of the node to set name for.</param>
        /// <param name = "name">New node's name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetNodeNameResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetNodeNameResponse> SetNodeNameAsync(int nodeId, string name)
        {
            ValidateSetNodeName(nodeId, name);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("name", name);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeName", dict);
            return methodResult.DeserializeJson<SetNodeNameResponse>();
        }

        partial void ValidateSetNodeValue(int nodeId, string value);
        /// <summary>
        /// Sets node value for a node with given id.
        /// </summary>
        /// <param name = "nodeId">Id of the node to set value for.</param>
        /// <param name = "value">New node's value.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetNodeValueAsync(int nodeId, string value)
        {
            ValidateSetNodeValue(nodeId, value);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setNodeValue", dict);
            return methodResult;
        }

        partial void ValidateSetOuterHTML(int nodeId, string outerHTML);
        /// <summary>
        /// Sets node HTML markup, returns new node id.
        /// </summary>
        /// <param name = "nodeId">Id of the node to set markup for.</param>
        /// <param name = "outerHTML">Outer HTML markup to set.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetOuterHTMLAsync(int nodeId, string outerHTML)
        {
            ValidateSetOuterHTML(nodeId, outerHTML);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("outerHTML", outerHTML);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.setOuterHTML", dict);
            return methodResult;
        }

        /// <summary>
        /// Undoes the last performed action.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UndoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.undo", dict);
            return methodResult;
        }

        partial void ValidateGetFrameOwner(string frameId);
        /// <summary>
        /// Returns iframe node that owns iframe with the given domain.
        /// </summary>
        /// <param name = "frameId">frameId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetFrameOwnerResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetFrameOwnerResponse> GetFrameOwnerAsync(string frameId)
        {
            ValidateGetFrameOwner(frameId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOM.getFrameOwner", dict);
            return methodResult.DeserializeJson<GetFrameOwnerResponse>();
        }
    }
}